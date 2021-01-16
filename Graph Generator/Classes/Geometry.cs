using System;
using System.Drawing;
using System.Collections.Generic;

namespace GraphGenerator
{
    class Geometry
    {
        public static Vector[] GetRandomPoints(int size, double width, double height)
        {
            Random rng = new Random();
            Vector[] points = new Vector[size];

            for (int i = 0; i < size; i++)
            {
                points[i] = new Vector
                (
                    rng.NextDouble() * width,
                    rng.NextDouble() * height
                );
            }

            return points;
        }

        /// <summary>
        /// Computes a Delaunay triangulation for the given set of points.
        /// </summary>
        public static TriData Triangulate(Vector[] points)
        {
            // --- Triangles and their Neighbourhoods. ---

            // Terminology:
            //  An *ID* is the index of an entry in points[].
            //  A *key* a non-negative int that identifies a triangle.
            //  If a triangle gets deleted, its key will not be used again.

            // A triangle is represented by an int-array of size 3.
            // Each entry is the ID of a point of the triangle.
            // They are in counter-clockwise order.

            // The neighbourhood of a triangle is represented by an int-array of size 3.
            // Each entry is the key of a neighbouring triangle.
            // Neighbours are in counter-clockwise order and corolate with the order of point-ID.
            // That is, the first neighbour shares the first and second point with the current triangle.
            // A negative entry in the neighbourhood states that the triangle has no neighbour on that side.

            //         P_2
            //         / \
            //    N_2 /   \  N_1
            //       /     \
            //      /       \
            // P_0 ----------- P_1
            //         N_0


            // Data structure to store triangles, their neighbourhoods, and to manage keys.
            TriData triData = new TriData(points);


            // --- Compute convex hull and triangulate it. ---

            int[] convHull = GetConvexHull(points);

            // Allows to later check quickly if a point is part of the conv. hull or not.
            HashSet<int> convHullSet = new HashSet<int>(convHull);

            // Triangulate set.
            for (int i = 2; i < convHull.Length; i++)
            {
                int orId = convHull[0];
                int preId = convHull[i - 1];
                int curId = convHull[i];

                int[] triIds = new int[] { orId, preId, curId };
                int[] neiKys = new int[3];

                int curKey = triData.Add(triIds, neiKys);

                int preKey = i == 2 ? -1 : (curKey - 1);
                int nexKey = (i + 1 < convHull.Length) ? (curKey + 1) : -1;

                neiKys[0] = preKey; // previous
                neiKys[1] = -1;     // outside
                neiKys[2] = nexKey; // next
            }


            // --- Iteratively add points the the triangulation. --

            for (int i = 0; i < points.Length; i++)
            {
                if (convHullSet.Contains(i)) continue;
                Vector pt = points[i];

                // Find triangle containing point.
                // (not efficient, but whatever)
                int tKey = -1;
                foreach (int key in triData.Keys)
                {
                    int[] triIds = triData[key][0];
                    Vector[] triVecs = new Vector[]
                    {
                        points[triIds[0]],
                        points[triIds[1]],
                        points[triIds[2]]
                    };

                    if (Geometry.InTriangle(triVecs, pt))
                    {
                        tKey = key;
                        break;
                    }
                }


                // -- Split triangle into three. --

                int[][] triInfo = triData[tKey];
                int[] tri = triInfo[0];
                int[] tNe = triInfo[1];

                // Remove from list (swap with last).
                triData.Remove(tKey);

                // New triangles ...
                int[] tri1 = new int[] { tri[0], tri[1], i };
                int[] tri2 = new int[] { tri[1], tri[2], i };
                int[] tri3 = new int[] { tri[2], tri[0], i };

                // ... and their neighbourhoods.
                int[] nei1 = new int[] { tNe[0], -1, -1 }; // will become nTKey2, nTKey3
                int[] nei2 = new int[] { tNe[1], -1, -1 }; // will become nTKey3, nTKey1
                int[] nei3 = new int[] { tNe[2], -1, -1 }; // will become nTKey1, nTKey2

                int nTKey1 = triData.Add(tri1, nei1);
                int nTKey2 = triData.Add(tri2, nei2);
                int nTKey3 = triData.Add(tri3, nei3);

                nei1[1] = nTKey2; nei1[2] = nTKey3;
                nei2[1] = nTKey3; nei2[2] = nTKey1;
                nei3[1] = nTKey1; nei3[2] = nTKey2;


                // Update neighbourhoods of neighbours.
                int[] newNeigKeys = new int[] { nTKey1, nTKey2, nTKey3 };

                for (int j = 0; j < 3; j++)
                {
                    int nKey = tNe[j];
                    if (nKey < 0) continue;

                    int[] nN = triData[nKey][1];
                    int ind = Array.IndexOf(nN, tKey);
                    nN[ind] = newNeigKeys[j];
                }

                // Check created triangles. Flip if needed.
                ChecksTriangles(triData, new int[] { nTKey1, nTKey2, nTKey3 });
            }

            return triData;
        }

        /// <summary>
        /// Checks if the specified triangles satisfy the Delaunay condition with respect to their neighbours.
        /// If not, triangles will be flipped.
        /// Filipped triangles will also be checked.
        /// </summary>
        public static bool ChecksTriangles(TriData triData, ICollection<int> keysToCheck)
        {
            bool flipped = false;
            Queue<int> q = new Queue<int>();

            foreach (int key in keysToCheck)
            {
                q.Enqueue(key);
            }

            while (q.Count > 0)
            {
                int ownKey = q.Dequeue();
                if (!triData.ContainsKey(ownKey)) continue;

                int[][] ownInfo = triData[ownKey];
                int[] ownTriIds = ownInfo[0];
                int[] ownNeiKys = ownInfo[1];

                for (int ownJ = 0; ownJ < 3; ownJ++)
                {
                    int neiKey = ownNeiKys[ownJ];
                    if (neiKey < 0) continue;

                    //          / \
                    //     j+2 /   \ j+1
                    //        / own \
                    //       /  j+0  \
                    //  P_j -----------
                    //       \  j+2  /
                    //        \ nei /
                    //     j+0 \   / j+1 
                    //          \ /

                    int[][] neiInfo = triData[neiKey];
                    int[] neiTriIds = neiInfo[0];
                    int[] neiNeiKys = neiInfo[1];

                    int jId = ownTriIds[ownJ];

                    // Opposing point in current triangle is previous.
                    int ownFarPt = ownTriIds[(ownJ + 2) % 3];

                    // Opposing point in neighbour triangle is next.
                    int neiJ = Array.IndexOf(neiTriIds, jId);
                    int neiFarPt = neiTriIds[(neiJ + 1) % 3];

                    int[] bothPts = new int[]
                    {
                            jId,
                            ownTriIds[(ownJ + 1) % 3]
                    };


                    double a1 = (triData.Points[ownFarPt] - triData.Points[bothPts[0]]).Length;
                    double b1 = (triData.Points[ownFarPt] - triData.Points[bothPts[1]]).Length;

                    double a2 = (triData.Points[neiFarPt] - triData.Points[bothPts[0]]).Length;
                    double b2 = (triData.Points[neiFarPt] - triData.Points[bothPts[1]]).Length;

                    double c = (triData.Points[bothPts[0]] - triData.Points[bothPts[1]]).Length;

                    double ang1 = Math.Acos((a1 * a1 + b1 * b1 - c * c) / (2.0 * a1 * b1));
                    double ang2 = Math.Acos((a2 * a2 + b2 * b2 - c * c) / (2.0 * a2 * b2));

                    if (ang1 + ang2 <= Math.PI)
                    {
                        // Dont flip.
                        continue;
                    }

                    int[] newTris = FlipTriangle(triData, ownKey, ownJ);
                    int keyL = newTris[0];
                    int keyR = newTris[1];

                    q.Enqueue(keyL);
                    q.Enqueue(keyR);

                    flipped = true;
                    break;
                }
            }

            return flipped;
        }

        private static int[] FlipTriangle(TriData triData, int ownKey, int neiInd)
        {
            //          / \                     /|\
            //     j+2 /   \ j+1               / | \
            //        / own \                 /  |  \
            //       /  j+0  \               /   |   \
            //  P_j -----------    =>   P_j <  L | R  >
            //       \  j+2  /               \   |   /
            //        \ nei /                 \  |  /
            //     j+0 \   / j+1               \ | / 
            //          \ /                     \|/


            // Data for own triangle.
            int[][] ownInfo = triData[ownKey];
            int[] ownTriIds = ownInfo[0];
            int[] ownNeiKys = ownInfo[1];

            // Data for neighbour.
            int neiKey = ownNeiKys[neiInd];
            if (neiKey < 0) return null;

            int[][] neiInfo = triData[neiKey];
            int[] neiTriIds = neiInfo[0];
            int[] neiNeiKys = neiInfo[1];

            int jId = ownTriIds[neiInd];

            // Opposing point in current triangle is previous.
            int ownFarPt = ownTriIds[(neiInd + 2) % 3];

            // Opposing point in neighbour triangle is next.
            int neiJ = Array.IndexOf(neiTriIds, jId);
            int neiFarPt = neiTriIds[(neiJ + 1) % 3];

            int[] bothPts = new int[]
            {
                jId,
                ownTriIds[(neiInd + 1) % 3]
            };

            int[] newTriL = new int[]
            {
                bothPts[0],
                neiFarPt,
                ownFarPt
            };

            int[] newTriR = new int[]
            {
                bothPts[1],
                ownFarPt,
                neiFarPt
            };

            int[] newNeighL =
            {
                neiNeiKys[(neiJ + 0) % 3],
                -1, // will become keyR
                ownNeiKys[(neiInd + 2) % 3]
            };

            int[] newNeighR =
            {
                ownNeiKys[(neiInd + 1) % 3],
                -1, // will become keyL
                neiNeiKys[(neiJ + 1) % 3]
            };


            int keyL = triData.Add(newTriL, newNeighL);
            int keyR = triData.Add(newTriR, newNeighR);

            newNeighL[1] = keyR;
            newNeighR[1] = keyL;

            triData.Remove(ownKey);
            triData.Remove(neiKey);


            // --- Update neighbourhood of neighbours. ---

            // Own (j + 1) neighbour
            int nnInd = ownNeiKys[(neiInd + 1) % 3];
            if (nnInd >= 0)
            {
                int[] nN = triData[nnInd][1];
                int ind = Array.IndexOf(nN, ownKey);
                nN[ind] = keyR;
            }

            // Own (j + 2) neighbour
            nnInd = ownNeiKys[(neiInd + 2) % 3];
            if (nnInd >= 0)
            {
                int[] nN = triData[nnInd][1];
                int ind = Array.IndexOf(nN, ownKey);
                nN[ind] = keyL;
            }

            // Neighbours (j + 0) neighbour
            nnInd = neiNeiKys[(neiJ + 0) % 3];
            if (nnInd >= 0)
            {
                int[] nN = triData[nnInd][1];
                int ind = Array.IndexOf(nN, neiKey);
                nN[ind] = keyL;
            }

            // Neighbours (j + 1) neighbour
            nnInd = neiNeiKys[(neiJ + 1) % 3];
            if (nnInd >= 0)
            {
                int[] nN = triData[nnInd][1];
                int ind = Array.IndexOf(nN, neiKey);
                nN[ind] = keyR;
            }

            return new int[] { keyL, keyR };
        }

        /// <summary>
        /// Returns a rectangle that sorounds the given points.
        /// </summary>
        public static RectangleF GetSurroundingRectangle(Vector[] points)
        {
            if (points == null || points.Length == 0)
            {
                return new RectangleF();
            }


            // --- Determine sorounding rectangle. ---

            float minX = float.MaxValue;
            float minY = float.MaxValue;

            float maxX = float.MinValue;
            float maxY = float.MinValue;

            for (int i = 0; i < points.Length; i++)
            {
                Vector vecV = points[i];

                minX = Math.Min(minX, (float)vecV.X);
                minY = Math.Min(minY, (float)vecV.Y);

                maxX = Math.Max(maxX, (float)vecV.X);
                maxY = Math.Max(maxY, (float)vecV.Y);
            }

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Returns an equilateral triangle that sorounds the given points.
        /// Points are in counter-clock-wise order in a left-handed coordinate system.
        /// </summary>
        public static Vector[] GetSurroundingTriangle(Vector[] points)
        {
            if (points == null || points.Length == 0)
            {
                return null;
            }


            RectangleF rect = GetSurroundingRectangle(points);


            // --- Compute triangle sorounding the rectangle. ---

            /*
             * Let A, B, C, and D be the points of the rectangle placed as follows:
             * 
             *   D C
             *   A B
             * 
             * Let X, Y, and Z be the points of the triangle placed as follows:
             * 
             *    Z
             *   X Y
             * 
             * We then have XA = AD / tan 60.
             */

            double ad = rect.Height;
            double xa = ad / Math.Tan(Math.PI / 3);
            double xy = 2.0 * ad + rect.Width;

            // Height of triangle is 0.5 * sqrt(3) * len.
            double triH = 0.5 * Math.Sqrt(3) * xy;

            Vector X = new Vector(rect.X - xa, rect.Y);
            Vector Y = new Vector(X.X + xy, rect.Y);
            Vector Z = new Vector(X.X + 0.5 * xy, rect.Y + triH);

            return new Vector[] { X, Y, Z };
        }

        /// <summary>
        /// Allows to determine if a point is left or right a line xy.
        /// </summary>
        /// <returns>
        /// A positive number if p is right of y from perspective of x in a left-handed coordinate system.
        /// </returns>
        public static double GetScalar(Vector x, Vector y, Vector p)
        {
            return (x.X - y.X) * (p.Y - y.Y) - (x.Y - y.Y) * (p.X - y.X);
        }

        /// <summary>
        /// Determines if a given point is within the given triangle.
        /// </summary>
        public static bool InTriangle(Vector[] triangle, Vector p)
        {
            if (triangle == null || triangle.Length != 3)
            {
                return false;
            }

            double scalXY = GetScalar(triangle[0], triangle[1], p);
            double scalYZ = GetScalar(triangle[1], triangle[2], p);
            double scalZX = GetScalar(triangle[2], triangle[0], p);

            return
                (scalXY <= 0 && scalYZ <= 0 && scalZX <= 0) ||
                (scalXY >= 0 && scalYZ >= 0 && scalZX >= 0);
        }

        /// <summary>
        /// Computes the convex hull from a set of points going counter-clockwise starting at the left-most point.
        /// </summary>
        public static int[] GetConvexHull(Vector[] points)
        {
            // Hull computed via Graham Scan.


            // --- Find left-most point. ---

            int lmId = 0;

            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].X < points[lmId].X)
                {
                    lmId = i;
                }
            }


            // --- Sort points based on angle to left-most point. ---

            int[] ids = new int[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                ids[i] = i;
            }

            GrahamScanComp com = new GrahamScanComp(lmId, points);
            Array.Sort(ids, com);


            // --- Compute convex hull. ---

            List<int> chStack = new List<int>();
            chStack.Add(ids[0]);
            chStack.Add(ids[1]);

            for (int i = 2; i < ids.Length; i++)
            {
                int iId = ids[i];

                // Pop points from stack until P_2 -- P_1 -- P_i forms a left turn.
                for (Vector ptI = points[iId]; ;)
                {
                    int min1Id = chStack[chStack.Count - 1];
                    int min2Id = chStack[chStack.Count - 2];

                    Vector pt1 = points[min1Id];
                    Vector pt2 = points[min2Id];

                    if (GetScalar(pt2, pt1, ptI) <= 0) break;
                    if (chStack.Count <= 2) break;

                    chStack.RemoveAt(chStack.Count - 1);
                }

                chStack.Add(iId);
            }

            return chStack.ToArray();
        }

        /// <summary>
        /// Allows to compare points based on their angle around a given point.
        /// The smalles possible position for a point is at -180° (inclusive) and the largest at +180° (exclusive).
        /// If two points have the same angle, they are compared based on their distance.
        /// </summary>
        class GrahamScanComp : IComparer<int>
        {
            private int orId;
            private Vector[] points;

            public GrahamScanComp(int orId, Vector[] points)
            {
                this.orId = orId;
                this.points = points;
            }

            public int Compare(int x, int y)
            {
                // Returns:
                //   < 0 if x is smaller
                //   = 0 if x and y are equal
                //   > 0 if x is larger

                Vector origin = points[orId];
                Vector vX = points[x];
                Vector vY = points[y];

                if (vX == vY) return 0;
                if (vX == origin) return -1;
                if (vY == origin) return 1;


                // We define quadrants as follows:
                //   1: [ -180, -90 )
                //   2: [  -90,   0 )
                //   3: [    0,  90 )
                //   4: [   90, 180 )

                int xQuad = -1;
                int yQuad = -1;

                if (vX.X < origin.X && vX.Y <= origin.Y) xQuad = 1;
                else if (vX.X >= origin.X && vX.Y < origin.Y) xQuad = 2;
                else if (vX.X > origin.X && vX.Y >= origin.Y) xQuad = 3;
                else if (vX.X <= origin.X && vX.Y > origin.Y) xQuad = 4;

                if (vY.X < origin.X && vY.Y <= origin.Y) yQuad = 1;
                else if (vY.X >= origin.X && vY.Y < origin.Y) yQuad = 2;
                else if (vY.X > origin.X && vY.Y >= origin.Y) yQuad = 3;
                else if (vY.X <= origin.X && vY.Y > origin.Y) yQuad = 4;

                if (xQuad != yQuad)
                {
                    return xQuad.CompareTo(yQuad);
                }


                // X and y are in same quadrant.
                // Compare based on angle.

                double scalar = GetScalar(origin, vX, vY);

                if (scalar == 0.0)
                {
                    // All three points are on a straight line.
                    // Compare based on distance.
                    return (vX - origin).Length.CompareTo((vY - origin).Length);
                }

                return scalar.CompareTo(0.0);
            }
        }
    }
}

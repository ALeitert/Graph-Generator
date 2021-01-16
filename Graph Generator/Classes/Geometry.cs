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
                neiKys[2] = nexKey;  // next
            }


            // ToDo: Implement.

            return triData;
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

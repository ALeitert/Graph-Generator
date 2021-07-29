using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace GraphGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        EventHandler lastgenerate = null;

        private void btnGenerate_ButtonClick(object sender, EventArgs e)
        {
            lastgenerate?.Invoke(sender, e);
        }

        private void mnuGenerateRandom_Click(object sender, EventArgs e)
        {
            lastgenerate = mnuGenerateRandom_Click;

            int size = 0;
            int eCount = 0;


            // --- Check input. --

            if (!int.TryParse(txtV.Text, out size))
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            if (!int.TryParse(txtE.Text, out eCount))
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            if (size <= 0 || eCount <= 0)
            {
                SystemSounds.Exclamation.Play();
                return;
            }


            // --- Generate graph. --

            int seed = Environment.TickCount & 1023;
            lblSeed.Text = string.Format("Seed = {0}", seed);

            graphControl.Graph = Graph.NewRandom(size, eCount, seed);
            graphControl.StartDrawing();
        }

        private void mnuGenerateTriangulation_Click(object sender, EventArgs e)
        {
            lastgenerate = mnuGenerateTriangulation_Click;

            int size = 0;


            // --- Check input. --

            if (!int.TryParse(txtV.Text, out size))
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            if (size <= 0)
            {
                SystemSounds.Exclamation.Play();
                return;
            }


            // --- Generate graph. --

            Vector[] drawing = Geometry.GetRandomPoints(size, 3, 2);
            TriData triData = Geometry.Triangulate(drawing);

            Graph g = new Graph(size);

            foreach (int key in triData.Keys)
            {
                int[] tri = triData[key][0];

                // Add edges.
                g[tri[0]].Add(tri[1]);
                g[tri[0]].Add(tri[2]);
                g[tri[1]].Add(tri[0]);
                g[tri[1]].Add(tri[2]);
                g[tri[2]].Add(tri[0]);
                g[tri[2]].Add(tri[1]);
            }


            // --- Center vertices between their neighbours. ---
            // (Leads to more evenly sized triangles.)

            HashSet<int> convHullSet = new HashSet<int>(Geometry.GetConvexHull(drawing));

            for (int i = 0; i < 100; i++)
            {
                for (int vId = 0; vId < size; vId++)
                {
                    if (convHullSet.Contains(vId)) continue;

                    Vector sum = new Vector();

                    foreach (int uId in g[vId])
                    {
                        sum += drawing[uId];
                    }

                    drawing[vId] = sum / g[vId].Count;
                }
            }


            graphControl.Graph = g;
            graphControl.Drawing = drawing;
        }

        private void planarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lastgenerate = planarToolStripMenuItem_Click;
            int size = 0;


            // --- Check input. --

            if (!int.TryParse(txtV.Text, out size))
            {
                SystemSounds.Exclamation.Play();
                return;
            }

            if (size <= 0)
            {
                SystemSounds.Exclamation.Play();
                return;
            }


            Random rng = new Random();

            int seed = rng.Next(5000);
            rng = new Random(seed);
            lblSeed.Text = string.Format("Seed = {0}", seed);

            Vector[] pointList = Geometry.GetRandomPoints(size, 2, 2);
            Array.Sort(pointList);

            Graph g = new Graph(size);


            // --- Determine edges. ---

            for (int x = 0; x < pointList.Length; x++)
            {
                Vector xPt = pointList[x];

                for (int y = x + 1; y < pointList.Length; y++)
                {
                    Vector yPt = pointList[y];
                    Vector cPt = new Vector((xPt.X + yPt.X) / 2F, (xPt.Y + yPt.Y) / 2F);

                    double rad = (xPt.X - cPt.X) * (xPt.X - cPt.X) + (xPt.Y - cPt.Y) * (xPt.Y - cPt.Y);
                    bool addEdge = true;

                    for (int z = 0; z < pointList.Length; z++)
                    {
                        if (z == x || z == y) continue;

                        Vector zPt = pointList[z];

                        double dist = (zPt.X - cPt.X) * (zPt.X - cPt.X) + (zPt.Y - cPt.Y) * (zPt.Y - cPt.Y);

                        if (dist < rad)
                        {
                            addEdge = false;
                            break;
                        }
                    }

                    if (addEdge)
                    {
                        g[x].Add(y);
                        g[y].Add(x);
                    }
                }
            }


            graphControl.Graph = g;
            graphControl.Drawing = pointList;
        }

        private void btnTikzExport_Click(object sender, EventArgs e)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            CultureInfo engCI = new CultureInfo("en-US", false);

            Graph g = graphControl.Graph;
            Vector[] drawing = graphControl.Drawing;

            if (g == null || drawing == null)
            {
                SystemSounds.Exclamation.Play();
                return;
            }


            RectangleF rect = Geometry.GetSurroundingRectangle(drawing);

            float xScale = 10f / rect.Width;
            float yScale = 5f / rect.Height;

            float scale = Math.Min(xScale, yScale);

            if (scale < 1f)
            {
                Vector[] drawing2 = (Vector[])drawing.Clone();

                for (int vId = 0; vId < g.Size; vId++)
                {
                    drawing2[vId] *= scale;
                }

                drawing = drawing2;
            }


            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@"\begin{tikzpicture}");
            sb.AppendLine();

            for (int vId = 0; vId < g.Size; vId++)
            {
                Vector pt = drawing[vId];
                sb.AppendLine(string.Format(engCI, @"\coordinate(co{0}) at ({1:0.0000}, {2:0.0000});", letters[vId], pt.X, -pt.Y));
            }
            sb.AppendLine();

            for (int vId = 0; vId < g.Size; vId++)
            {
                List<int> neighs = new List<int>(g[vId]);
                neighs.Sort();

                for (int i = 0; i < neighs.Count; i++)
                {
                    int uId = neighs[i];
                    if (uId <= vId) continue;

                    sb.AppendLine(string.Format(@"\draw (co{0}) -- (co{1});", letters[vId], letters[uId]));
                }
            }
            sb.AppendLine();

            for (int vId = 0; vId < g.Size; vId++)
            {
                sb.AppendLine(string.Format(@"\node[gN] (n{0}) at (co{0}) {{}};", letters[vId]));
                sb.AppendLine(string.Format(@"\node at (co{0}) {{${1}$}};", letters[vId], letters[vId].ToString().ToLower()));
            }
            sb.AppendLine();

            sb.AppendLine(@"\end{tikzpicture}");

            Form f = new Form();
            TextBox txt = new TextBox();
            f.Controls.Add(txt);
            txt.Dock = DockStyle.Fill;
            txt.Multiline = true;
            txt.ScrollBars = ScrollBars.Both;
            txt.Font = new Font("Consolas", 10f);
            txt.Text = sb.ToString();
            f.Show();
        }

        private void mnuGenerateLayPart_Click(object sender, EventArgs e)
        {
            lastgenerate = mnuGenerateLayPart_Click;

            // --- Make tree --

            int tSize = 6;
            int gSize = 15;

            int seed = new Random().Next(5000);
            lblSeed.Text = string.Format("Seed = {0}", seed);
            Random rng = new Random(seed);

            int[] tree = new int[tSize];

            for (int i = 1; i < tSize; i++)
            {
                tree[i] = rng.Next(i);
            }


            // --- Generate clusters based on tree. ---

            Graph g = new Graph(gSize);

            List<int>[] culsters = new List<int>[tSize];

            for (int i = 0; i < tSize; i++)
            {
                culsters[i] = new List<int>();
                culsters[i].Add(i);

                if (i == 0) continue;

                int par = tree[i];
                g[i].Add(par);
                g[par].Add(i);
            }


            // --- Add remaining vertices to random clusters. --- 

            for (int i = tSize; i < gSize; i++)
            {
                int cl = rng.Next(tSize - 1) + 1;
                culsters[cl].Add(i);

                int pre = culsters[cl][culsters[cl].Count - 2];
                g[i].Add(pre);
                g[pre].Add(i);


                int pCl = tree[cl];

                foreach (int j in culsters[pCl])
                {
                    g[i].Add(j);
                    g[j].Add(i);
                }
            }


            graphControl.Graph = g;
            graphControl.StartDrawing();
        }
    }
}

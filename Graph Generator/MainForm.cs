﻿using System;
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
    }
}

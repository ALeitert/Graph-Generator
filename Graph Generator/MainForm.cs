using System;
using System.Collections.Generic;
using System.Media;
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
    }
}

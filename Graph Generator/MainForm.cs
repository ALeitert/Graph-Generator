using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;

namespace GraphGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            typeof(Control).GetProperty("DoubleBuffered", (System.Reflection.BindingFlags)(-1)).SetValue(pnlCanves, true, new object[] { });
        }


        private Graph currentGraph = null;
        private Vector[] drawing = null;

        private int drawingTime = -1;
        private const int MaxDrawTime = 3000; // in ms


        private void btnGenerate_Click(object sender, EventArgs e)
        {
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

            currentGraph = Graph.NewRandom(size, eCount, seed);
            drawing = null;

            // --- Draw graph. ---

            drawingTime = Environment.TickCount;
            drawingTimer.Start();

        }

        private void drawingTimer_Tick(object sender, EventArgs e)
        {
            DrawGraph();
            Refresh();

            if (Environment.TickCount - drawingTime > MaxDrawTime)
            {
                drawingTimer.Stop();
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            CenterGraph();
            Refresh();
        }


        private void DrawGraph()
        {
            if (currentGraph == null)
            {
                drawing = null;
                return;
            }

            drawing = currentGraph.Draw(drawing);
            CenterGraph();
        }

        private void CenterGraph()
        {
            // --- Center graph at (0, 0). ---

            double minX = double.MaxValue;
            double minY = double.MaxValue;

            double maxX = double.MinValue;
            double maxY = double.MinValue;

            for (int vId = 0; vId < currentGraph.Size; vId++)
            {
                Vector vecV = drawing[vId];

                minX = Math.Min(minX, vecV.X);
                minY = Math.Min(minY, vecV.Y);

                maxX = Math.Max(maxX, vecV.X);
                maxY = Math.Max(maxY, vecV.Y);
            }

            Vector shift = new Vector(minX + maxX, minY + maxY);
            shift *= -0.5;

            for (int vId = 0; vId < currentGraph.Size; vId++)
            {
                drawing[vId] += shift;
            }
        }

        private void pnlCanves_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(pnlCanves.BackColor);

            if (currentGraph == null || drawing == null)
            {
                return;
            }


            // --- Determine scale. ---

            double minX = double.MaxValue;
            double minY = double.MaxValue;

            double maxX = double.MinValue;
            double maxY = double.MinValue;

            for (int vId = 0; vId < currentGraph.Size; vId++)
            {
                Vector vecV = drawing[vId];

                minX = Math.Min(minX, vecV.X);
                minY = Math.Min(minY, vecV.Y);

                maxX = Math.Max(maxX, vecV.X);
                maxY = Math.Max(maxY, vecV.Y);
            }

            double scaleX = 0.8 * (double)pnlCanves.Width / (maxX - minX);
            double scaleY = 0.8 * (double)pnlCanves.Height / (maxY - minY);

            float scale = Convert.ToSingle(Math.Min(scaleX, scaleY));


            // --- Draw graph. ---

            g.TranslateTransform(pnlCanves.Width / 2, pnlCanves.Height / 2);
            g.ScaleTransform(1, -1);

            g.SmoothingMode = SmoothingMode.HighQuality;

            const float PenWidth = 1F;
            Pen blackPen = new Pen(Color.Black, PenWidth);

            // Draw edges.
            for (int vId = 0; vId < currentGraph.Size; vId++)
            {
                foreach (int uId in currentGraph[vId])
                {
                    if (uId < vId) continue;
                    g.DrawLine
                    (
                        blackPen,
                        (drawing[vId] * scale).ToPointF(),
                        (drawing[uId] * scale).ToPointF()
                    );
                }
            }

            for (int vId = 0; vId < currentGraph.Size; vId++)
            {
                PointF ptV = (drawing[vId] * scale).ToPointF();
                g.FillEllipse(Brushes.DarkGreen, ptV.X - 5, ptV.Y - 5, 11, 11);
            }
        }
    }
}

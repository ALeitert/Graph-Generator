using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GraphGenerator
{
    public partial class GraphControl : UserControl
    {
        public GraphControl()
        {
            InitializeComponent();
            typeof(Control).GetProperty("DoubleBuffered", (System.Reflection.BindingFlags)(-1)).SetValue(pnlCanves, true, new object[] { });
        }

        private void GraphControl_Resize(object sender, EventArgs e)
        {
            UpddateCanvasScale();
            Refresh();
        }

        private void pnlCanves_Paint(object sender, PaintEventArgs e)
        {
            PaintGraph(e.Graphics, pnlCanves.Width, pnlCanves.Height, canvasScale);
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


        private Graph graph = null;
        private Vector[] drawing = null;

        private int drawingTime = -1;
        private const int MaxDrawTime = 3000; // in ms
        private const float VertexRadius = 5F; // in pixel

        // Scaling from virtual points to coordinates of canvas.
        private float canvasScale = 1F;


        internal Graph Graph
        {
            get
            {
                return graph;
            }
            set
            {
                graph = value;
                drawing = null;
                StartDrawing();
            }
        }

        /// <summary>
        /// Starts the process of drawing the graph.
        /// The graph will be redrawn for a few seconds.
        /// </summary>
        private void StartDrawing()
        {
            drawingTime = Environment.TickCount;
            drawingTimer.Start();
        }

        /// <summary>
        /// Computes the x- and y-coordinates for each vertex of the given graph using a forced-based algorithm.
        /// </summary>
        private void DrawGraph()
        {
            if (graph == null)
            {
                drawing = null;
                return;
            }

            drawing = graph.Draw(drawing);

            CenterGraph();
            UpddateCanvasScale();
        }

        /// <summary>
        /// Centers tha graph at origin of coordinate system, i.e., at (0, 0).
        /// </summary>
        private void CenterGraph()
        {
            if (drawing == null) return;


            double minX = double.MaxValue;
            double minY = double.MaxValue;

            double maxX = double.MinValue;
            double maxY = double.MinValue;

            for (int vId = 0; vId < graph.Size; vId++)
            {
                Vector vecV = drawing[vId];

                minX = Math.Min(minX, vecV.X);
                minY = Math.Min(minY, vecV.Y);

                maxX = Math.Max(maxX, vecV.X);
                maxY = Math.Max(maxY, vecV.Y);
            }

            Vector shift = new Vector(minX + maxX, minY + maxY);
            shift *= -0.5;

            for (int vId = 0; vId < graph.Size; vId++)
            {
                drawing[vId] += shift;
            }
        }

        ///
        private void UpddateCanvasScale()
        {
            if (drawing == null) return;
            canvasScale = ComputeScale(pnlCanves.Width, pnlCanves.Height);
        }

        private float ComputeScale(double width, double height)
        {
            if (drawing == null) return 1F;

            double minX = double.MaxValue;
            double minY = double.MaxValue;

            double maxX = double.MinValue;
            double maxY = double.MinValue;

            for (int vId = 0; vId < graph.Size; vId++)
            {
                Vector vecV = drawing[vId];

                minX = Math.Min(minX, vecV.X);
                minY = Math.Min(minY, vecV.Y);

                maxX = Math.Max(maxX, vecV.X);
                maxY = Math.Max(maxY, vecV.Y);
            }

            double scaleX = 0.8 * width / (maxX - minX);
            double scaleY = 0.8 * height / (maxY - minY);

            return Convert.ToSingle(Math.Min(scaleX, scaleY));
        }

        /// <summary>
        /// Uses the given graphics object to paint the graph centered in the are of the given size.
        /// </summary>
        private void PaintGraph(Graphics g, int width, int height, float scale)
        {
            /*
             * ToDo: Potential improvements.
             *   - Give a rectangle instaed of just a size.
             *   - Undo configurations of g.
             */

            g.Clear(pnlCanves.BackColor);

            if (graph == null || drawing == null)
            {
                return;
            }


            // --- Draw graph. ---

            g.TranslateTransform(width / 2, height / 2);
            g.SmoothingMode = SmoothingMode.HighQuality;

            const float PenWidth = 1F;
            Pen blackPen = new Pen(Color.Black, PenWidth);

            // Draw edges.
            for (int vId = 0; vId < graph.Size; vId++)
            {
                foreach (int uId in graph[vId])
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

            float vRad = VertexRadius;
            float vDia = 2F * VertexRadius + 1F;

            for (int vId = 0; vId < graph.Size; vId++)
            {
                PointF ptV = (drawing[vId] * scale).ToPointF();
                g.FillEllipse(Brushes.DarkGreen, ptV.X - vRad, ptV.Y - vRad, vDia, vDia);
            }
        }
    }
}

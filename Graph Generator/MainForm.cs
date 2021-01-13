using System;
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

            Graph g = Graph.NewRandom(size, eCount, seed);


            // --- Draw graph.

        }
    }
}


namespace GraphGenerator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainToolBar = new System.Windows.Forms.ToolStrip();
            this.lblV = new System.Windows.Forms.ToolStripLabel();
            this.txtV = new System.Windows.Forms.ToolStripTextBox();
            this.lblE = new System.Windows.Forms.ToolStripLabel();
            this.txtE = new System.Windows.Forms.ToolStripTextBox();
            this.btnGenerate = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuGenerateRandom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGenerateTriangulation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblSeed = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.pnlToolBar = new System.Windows.Forms.Panel();
            this.graphControl = new GraphGenerator.GraphControl();
            this.mainToolBar.SuspendLayout();
            this.pnlToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainToolBar
            // 
            this.mainToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblV,
            this.txtV,
            this.lblE,
            this.txtE,
            this.btnGenerate,
            this.toolStripSeparator1,
            this.lblSeed,
            this.toolStripSeparator2});
            this.mainToolBar.Location = new System.Drawing.Point(0, 0);
            this.mainToolBar.Name = "mainToolBar";
            this.mainToolBar.Size = new System.Drawing.Size(776, 25);
            this.mainToolBar.TabIndex = 0;
            // 
            // lblV
            // 
            this.lblV.Name = "lblV";
            this.lblV.Size = new System.Drawing.Size(25, 22);
            this.lblV.Text = "n =";
            // 
            // txtV
            // 
            this.txtV.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtV.Name = "txtV";
            this.txtV.Size = new System.Drawing.Size(50, 25);
            this.txtV.Text = "10";
            // 
            // lblE
            // 
            this.lblE.Name = "lblE";
            this.lblE.Size = new System.Drawing.Size(29, 22);
            this.lblE.Text = "m =";
            // 
            // txtE
            // 
            this.txtE.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtE.Name = "txtE";
            this.txtE.Size = new System.Drawing.Size(50, 25);
            this.txtE.Text = "15";
            // 
            // btnGenerate
            // 
            this.btnGenerate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGenerate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGenerateRandom,
            this.mnuGenerateTriangulation});
            this.btnGenerate.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerate.Image")));
            this.btnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(67, 22);
            this.btnGenerate.Text = "Generate";
            // 
            // mnuGenerateRandom
            // 
            this.mnuGenerateRandom.Name = "mnuGenerateRandom";
            this.mnuGenerateRandom.Size = new System.Drawing.Size(180, 22);
            this.mnuGenerateRandom.Text = "Random";
            this.mnuGenerateRandom.Click += new System.EventHandler(this.mnuGenerateRandom_Click);
            // 
            // mnuGenerateTriangulation
            // 
            this.mnuGenerateTriangulation.Name = "mnuGenerateTriangulation";
            this.mnuGenerateTriangulation.Size = new System.Drawing.Size(180, 22);
            this.mnuGenerateTriangulation.Text = "Triangulation";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblSeed
            // 
            this.lblSeed.AutoSize = false;
            this.lblSeed.Name = "lblSeed";
            this.lblSeed.Size = new System.Drawing.Size(70, 22);
            this.lblSeed.Text = "Seed = 0";
            this.lblSeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // pnlToolBar
            // 
            this.pnlToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlToolBar.Controls.Add(this.mainToolBar);
            this.pnlToolBar.Location = new System.Drawing.Point(12, 12);
            this.pnlToolBar.Name = "pnlToolBar";
            this.pnlToolBar.Size = new System.Drawing.Size(776, 25);
            this.pnlToolBar.TabIndex = 3;
            // 
            // graphControl
            // 
            this.graphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphControl.Location = new System.Drawing.Point(12, 43);
            this.graphControl.Name = "graphControl";
            this.graphControl.Size = new System.Drawing.Size(776, 395);
            this.graphControl.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.graphControl);
            this.Controls.Add(this.pnlToolBar);
            this.Name = "MainForm";
            this.Text = "Graph Generator";
            this.mainToolBar.ResumeLayout(false);
            this.mainToolBar.PerformLayout();
            this.pnlToolBar.ResumeLayout(false);
            this.pnlToolBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainToolBar;
        private System.Windows.Forms.Panel pnlToolBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblV;
        private System.Windows.Forms.ToolStripTextBox txtV;
        private System.Windows.Forms.ToolStripLabel lblE;
        private System.Windows.Forms.ToolStripTextBox txtE;
        private System.Windows.Forms.ToolStripLabel lblSeed;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private GraphControl graphControl;
        private System.Windows.Forms.ToolStripDropDownButton btnGenerate;
        private System.Windows.Forms.ToolStripMenuItem mnuGenerateRandom;
        private System.Windows.Forms.ToolStripMenuItem mnuGenerateTriangulation;
    }
}
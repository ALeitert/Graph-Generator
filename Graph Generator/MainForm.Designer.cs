
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
            this.mainToolBar = new System.Windows.Forms.ToolStrip();
            this.pnlCanves = new System.Windows.Forms.Panel();
            this.pnlToolBar = new System.Windows.Forms.Panel();
            this.btnGenerate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainToolBar.SuspendLayout();
            this.pnlToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainToolBar
            // 
            this.mainToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.btnGenerate});
            this.mainToolBar.Location = new System.Drawing.Point(0, 0);
            this.mainToolBar.Name = "mainToolBar";
            this.mainToolBar.Size = new System.Drawing.Size(776, 25);
            this.mainToolBar.TabIndex = 0;
            // 
            // pnlCanves
            // 
            this.pnlCanves.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCanves.BackColor = System.Drawing.Color.White;
            this.pnlCanves.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCanves.Location = new System.Drawing.Point(12, 43);
            this.pnlCanves.Name = "pnlCanves";
            this.pnlCanves.Size = new System.Drawing.Size(776, 395);
            this.pnlCanves.TabIndex = 2;
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
            // btnGenerate
            // 
            this.btnGenerate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(58, 22);
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlToolBar);
            this.Controls.Add(this.pnlCanves);
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
        private System.Windows.Forms.ToolStripButton btnGenerate;
        private System.Windows.Forms.Panel pnlCanves;
        private System.Windows.Forms.Panel pnlToolBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
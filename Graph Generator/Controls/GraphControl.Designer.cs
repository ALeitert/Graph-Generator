
namespace GraphGenerator
{
    partial class GraphControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlCanves = new System.Windows.Forms.Panel();
            this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuVertex = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVertexAddEdge = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDraw = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingTimer = new System.Windows.Forms.Timer(this.components);
            this.mnuVertexRemoveEdge = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCanves
            // 
            this.pnlCanves.BackColor = System.Drawing.Color.White;
            this.pnlCanves.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCanves.ContextMenuStrip = this.mnuContext;
            this.pnlCanves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanves.Location = new System.Drawing.Point(0, 0);
            this.pnlCanves.Name = "pnlCanves";
            this.pnlCanves.Size = new System.Drawing.Size(150, 150);
            this.pnlCanves.TabIndex = 3;
            this.pnlCanves.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCanves_Paint);
            this.pnlCanves.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlCanves_MouseDown);
            this.pnlCanves.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlCanves_MouseMove);
            this.pnlCanves.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlCanves_MouseUp);
            // 
            // mnuContext
            // 
            this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuVertex,
            this.toolStripMenuItem1,
            this.mnuDraw});
            this.mnuContext.Name = "mnuContext";
            this.mnuContext.Size = new System.Drawing.Size(181, 76);
            this.mnuContext.Opening += new System.ComponentModel.CancelEventHandler(this.mnuContext_Opening);
            // 
            // mnuVertex
            // 
            this.mnuVertex.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuVertexAddEdge,
            this.mnuVertexRemoveEdge});
            this.mnuVertex.Name = "mnuVertex";
            this.mnuVertex.Size = new System.Drawing.Size(180, 22);
            this.mnuVertex.Text = "Vertex";
            // 
            // mnuVertexAddEdge
            // 
            this.mnuVertexAddEdge.Name = "mnuVertexAddEdge";
            this.mnuVertexAddEdge.Size = new System.Drawing.Size(180, 22);
            this.mnuVertexAddEdge.Text = "Add edge";
            this.mnuVertexAddEdge.Click += new System.EventHandler(this.mnuVertexAddEdge_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuDraw
            // 
            this.mnuDraw.Name = "mnuDraw";
            this.mnuDraw.Size = new System.Drawing.Size(180, 22);
            this.mnuDraw.Text = "Draw";
            this.mnuDraw.Click += new System.EventHandler(this.mnuDraw_Click);
            // 
            // drawingTimer
            // 
            this.drawingTimer.Tick += new System.EventHandler(this.drawingTimer_Tick);
            // 
            // mnuVertexRemoveEdge
            // 
            this.mnuVertexRemoveEdge.Name = "mnuVertexRemoveEdge";
            this.mnuVertexRemoveEdge.Size = new System.Drawing.Size(180, 22);
            this.mnuVertexRemoveEdge.Text = "Remove edge";
            this.mnuVertexRemoveEdge.Click += new System.EventHandler(this.mnuVertexRemoveEdge_Click);
            // 
            // GraphControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlCanves);
            this.Name = "GraphControl";
            this.Resize += new System.EventHandler(this.GraphControl_Resize);
            this.mnuContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCanves;
        private System.Windows.Forms.Timer drawingTimer;
        private System.Windows.Forms.ContextMenuStrip mnuContext;
        private System.Windows.Forms.ToolStripMenuItem mnuVertex;
        private System.Windows.Forms.ToolStripMenuItem mnuVertexAddEdge;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuDraw;
        private System.Windows.Forms.ToolStripMenuItem mnuVertexRemoveEdge;
    }
}


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
            this.drawingTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // pnlCanves
            // 
            this.pnlCanves.BackColor = System.Drawing.Color.White;
            this.pnlCanves.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCanves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanves.Location = new System.Drawing.Point(0, 0);
            this.pnlCanves.Name = "pnlCanves";
            this.pnlCanves.Size = new System.Drawing.Size(150, 150);
            this.pnlCanves.TabIndex = 3;
            this.pnlCanves.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCanves_Paint);
            this.pnlCanves.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlCanves_MouseMove);
            // 
            // drawingTimer
            // 
            this.drawingTimer.Tick += new System.EventHandler(this.drawingTimer_Tick);
            // 
            // GraphControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlCanves);
            this.Name = "GraphControl";
            this.Resize += new System.EventHandler(this.GraphControl_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCanves;
        private System.Windows.Forms.Timer drawingTimer;
    }
}

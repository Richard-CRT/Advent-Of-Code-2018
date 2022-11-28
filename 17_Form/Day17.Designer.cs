namespace _17_Form
{
    partial class Day17
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
            this.components = new System.ComponentModel.Container();
            this.TickTimer = new System.Windows.Forms.Timer(this.components);
            this.MapCanvas = new _17_Form.Canvas();
            this.SuspendLayout();
            // 
            // TickTimer
            // 
            this.TickTimer.Enabled = true;
            this.TickTimer.Interval = 10;
            this.TickTimer.Tick += new System.EventHandler(this.TickTimer_Tick);
            // 
            // MapCanvas
            // 
            this.MapCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MapCanvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(0)))));
            this.MapCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapCanvas.Location = new System.Drawing.Point(12, 12);
            this.MapCanvas.Name = "MapCanvas";
            this.MapCanvas.Size = new System.Drawing.Size(1879, 966);
            this.MapCanvas.TabIndex = 0;
            // 
            // Day17
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1903, 990);
            this.Controls.Add(this.MapCanvas);
            this.Name = "Day17";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Day17";
            this.ResumeLayout(false);

        }

        #endregion

        private Canvas MapCanvas;
        private System.Windows.Forms.Timer TickTimer;
    }
}
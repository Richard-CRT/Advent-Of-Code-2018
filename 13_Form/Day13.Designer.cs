namespace _13_Form
{
    partial class Day13
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
            this.TrackCanvas = new _13_Form.Canvas();
            this.SuspendLayout();
            // 
            // TickTimer
            // 
            this.TickTimer.Interval = 1;
            this.TickTimer.Tick += new System.EventHandler(this.TickTimer_Tick);
            // 
            // TrackCanvas
            // 
            this.TrackCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrackCanvas.BackColor = System.Drawing.Color.White;
            this.TrackCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TrackCanvas.Location = new System.Drawing.Point(12, 12);
            this.TrackCanvas.Name = "TrackCanvas";
            this.TrackCanvas.Size = new System.Drawing.Size(740, 356);
            this.TrackCanvas.TabIndex = 0;
            // 
            // Day13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 380);
            this.Controls.Add(this.TrackCanvas);
            this.Name = "Day13";
            this.Text = "Day13";
            this.Load += new System.EventHandler(this.Day13_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Canvas TrackCanvas;
        private System.Windows.Forms.Timer TickTimer;
    }
}
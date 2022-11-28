namespace _10
{
    partial class Day10
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
            this.ButtonAdvance = new System.Windows.Forms.Button();
            this.MapCanvas = new _10.Canvas();
            this.TimerTick = new System.Windows.Forms.Timer(this.components);
            this.ButtonStart = new System.Windows.Forms.Button();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonAdvance
            // 
            this.ButtonAdvance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonAdvance.Location = new System.Drawing.Point(12, 355);
            this.ButtonAdvance.Name = "ButtonAdvance";
            this.ButtonAdvance.Size = new System.Drawing.Size(82, 23);
            this.ButtonAdvance.TabIndex = 0;
            this.ButtonAdvance.Text = "Advance";
            this.ButtonAdvance.UseVisualStyleBackColor = true;
            this.ButtonAdvance.Click += new System.EventHandler(this.ButtonAdvance_Click);
            // 
            // MapCanvas
            // 
            this.MapCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MapCanvas.BackColor = System.Drawing.Color.White;
            this.MapCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapCanvas.Location = new System.Drawing.Point(12, 12);
            this.MapCanvas.Name = "MapCanvas";
            this.MapCanvas.Size = new System.Drawing.Size(1687, 337);
            this.MapCanvas.TabIndex = 1;
            // 
            // TimerTick
            // 
            this.TimerTick.Interval = 20;
            this.TimerTick.Tick += new System.EventHandler(this.TimerTick_Tick);
            // 
            // ButtonStart
            // 
            this.ButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonStart.Location = new System.Drawing.Point(99, 355);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(81, 23);
            this.ButtonStart.TabIndex = 2;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonStop.Location = new System.Drawing.Point(188, 355);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(78, 23);
            this.ButtonStop.TabIndex = 3;
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // Day10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1711, 390);
            this.Controls.Add(this.ButtonStop);
            this.Controls.Add(this.ButtonStart);
            this.Controls.Add(this.MapCanvas);
            this.Controls.Add(this.ButtonAdvance);
            this.Name = "Day10";
            this.Text = "Day 10";
            this.Load += new System.EventHandler(this.Day10_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonAdvance;
        private Canvas MapCanvas;
        private System.Windows.Forms.Timer TimerTick;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Button ButtonStop;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _10
{
    public partial class Day10 : Form
    {
        List<Point> Points;
        int Ticks = 0;

        public Day10(List<Point> points)
        {
            Points = points;

            InitializeComponent();
        }

        private void Advance()
        {
            foreach (Point point in Points)
            {
                point.X += point.VelocityX;
                point.Y += point.VelocityY;
            }
            Ticks++;
        }

        private void UpdateCanvas()
        {
            //Debug.WriteLine(Ticks);
            this.MapCanvas.UpdatePoints(Points);
        }

        private void TimerTick_Tick(object sender, EventArgs e)
        {
            if (Ticks < 10345)
            {
                int difference = (int)Math.Round((10345 - Ticks) / 20.0) + 1;
#if DEBUG
                if (difference == 1)
                {
                    Thread.Sleep(10);
                }
#endif
                for (int i = 0; i < Math.Min(10345- Ticks, difference); i++)
                {
                    Advance();
                }
            }
            else
            {
                TimerTick.Enabled = false;
            }
            UpdateCanvas();
        }

        private void ButtonAdvance_Click(object sender, EventArgs e)
        {
            Advance();
            UpdateCanvas();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            this.TimerTick.Enabled = true;
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            this.TimerTick.Enabled = false;
        }

        private void Day10_Load(object sender, EventArgs e)
        {
            /*
            // 10345
            for (int i = 0; i < 10345; i++)
            {
                Advance();
            }
            UpdateCanvas();
            */
        }
    }
}

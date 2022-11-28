using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdventOfCodeUtilities;

namespace _10
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInput();

            int coordinateWidth = 6;
            int velocityWidth = 2;

            List<Point> points = new List<Point>();

            foreach (string positionString in inputList)
            {
                int coordinateX = Int32.Parse(positionString.Substring(10, coordinateWidth));
                int coordinateY = Int32.Parse(positionString.Substring(12 + coordinateWidth, coordinateWidth));
                int velocityX = Int32.Parse(positionString.Substring(24 + 2 * coordinateWidth, velocityWidth));
                int velocityY = Int32.Parse(positionString.Substring(26 + 2 * coordinateWidth + velocityWidth, velocityWidth));

                points.Add(new Point(coordinateX, coordinateY, velocityX, velocityY));
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Day10(points));
        }
    }

    public class Point
    {
        public int X;
        public int Y;
        public int VelocityX;
        public int VelocityY;

        public Point(int x, int y, int velocityX, int velocityY)
        {
            X = x;
            Y = y;
            VelocityX = velocityX;
            VelocityY = velocityY;
        }
    }
}

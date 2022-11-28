using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _17_Form
{
    public partial class Day17 : Form
    {
        public List<string> inputList;

        public Day17()
        {
            InitializeComponent();
            inputList = AoCUtilities.GetInput();

            MapCanvas.parent = this;
            MapCanvas.Canvas_Setup();
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            MapCanvas.Tick();
        }
    }
}

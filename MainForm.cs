using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COSCSimulator
{
    public partial class MainForm : Form
    {
        private int x1 = 20, y1 = 20, x2 = 100, y2 = 100, z1 = 20, z2 = 100;
        private int zXConstant;
        private double velocity = 44;
        private Stopwatch stopwatch = new Stopwatch();
        private int size = 4;
        private Brush lightGreen = new SolidBrush(Color.LightGreen);
        private Brush black = new SolidBrush(Color.Black);
        private Brush red = new SolidBrush(Color.Red);
        private Brush lightBlue = new SolidBrush(Color.LightBlue);
        private Brush yellow = new SolidBrush(Color.Yellow);

        private bool simulationRunning = false;
        private double simulationDuration = 0;

        public const string directions = "Left Click Sets Current Position and Right Click Sets Target Position";

        public MainForm()
        {
            InitializeComponent();

            x2 = simulationBoard.Size.Width - 20;
            y2 = simulationBoard.Size.Height - 20;
            z2 = zAxisBoard.Size.Height - 20;

            zXConstant = zAxisBoard.Size.Width / 2;
            
            infoLabel.Text = directions;
        }        
        
        private void simulationBoard_Click(object sender, EventArgs e)
        {
            if (!simulationRunning)
            {
                Point point = simulationBoard.PointToClient(Cursor.Position);
                MouseEventArgs me = (MouseEventArgs)e;
                if (me.Button == MouseButtons.Left)
                {
                    x1 = point.X;
                    y1 = point.Y;
                }
                else
                {
                    x2 = point.X;
                    y2 = point.Y;
                }

                paintSimulationBoard();
            }
        }

        private void simulationBoard_Paint(object sender, PaintEventArgs e)
        {
            paintSimulationBoard();            
        }

        private void paintSimulationBoard()
        {
            Graphics gObj = simulationBoard.CreateGraphics();
            gObj.FillRectangle(lightGreen, 0, 0, simulationBoard.Size.Width, simulationBoard.Size.Height);
            gObj.FillRectangle(black, x1, y1, size, size);
            gObj.FillRectangle(red, x2, y2, size, size);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Update anything that needs to update less frequently such as labels and text and whatnot
        }

        private void zAxisBoard_Paint(object sender, PaintEventArgs e)
        {
            paintZAxisBoard();
        }

        private void paintZAxisBoard()
        {
            Graphics gObj = zAxisBoard.CreateGraphics();
            // Reinit the board
            gObj.FillRectangle(lightBlue, 0, 0, zAxisBoard.Size.Width, zAxisBoard.Size.Height);

            gObj.FillRectangle(black, zXConstant, z1, size, size);
            gObj.FillRectangle(red, zXConstant, z2, size, size);
        }

        private void zAxisBoard_Click(object sender, EventArgs e)
        {
            if (!simulationRunning)
            {
                Point point = simulationBoard.PointToClient(Cursor.Position);

                MouseEventArgs me = (MouseEventArgs)e;
                if (me.Button == MouseButtons.Left)
                {
                    z1 = point.Y;
                }
                else
                {
                    z2 = point.Y;
                }
                
                paintZAxisBoard();
            }
        }

        private async void goButton_Click(object sender, EventArgs e)
        {
            resultsListBox.Items.Clear();
            paintSimulationBoard();
            paintZAxisBoard();

            simulationRunning = true;
            double tX = x2 - x1;
            double tY = y2 - y1;
            double tZ = z2 - z1;
            double tLength = Math.Sqrt(tX * tX + tY * tY + tZ * tZ);
            simulationDuration = Math.Round(tLength / velocity, 4);
            infoLabel.Text = "Distance = " + Math.Round(tLength, 0).ToString() + " feet. Expected Flight Duration in Seconds = " + simulationDuration.ToString();

            stopwatch.Reset();
            stopwatch.Start();
            timer1.Enabled = true;

            int numberOfObjects = Convert.ToInt32(countDropDown.Text);
            SimulatorController controller = new SimulatorController(simulationBoard, zAxisBoard, numberOfObjects, simulationDuration, x1, y1, z1, x2, y2, z2, velocity);
            await Task.Run(() =>
            {                
                controller.Run();
            });
            stopwatch.Stop();
            timer1.Enabled = false;
            infoLabel.Text = directions;
            simulationRunning = false;

            foreach (var distance in controller.getDistances())
            {
                resultsListBox.Items.Add(Math.Round(distance,2).ToString());
            }

        }


    }
}

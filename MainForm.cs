using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COSCSimulator
{
    public partial class MainForm : Form
    {
        private int x1, y1, x2, y2, z1, z2;
        private int zXConstant;
        private double velocity = 44;
        private Stopwatch stopwatch = new Stopwatch();
        private Brush lightGreen = new SolidBrush(Color.LightGreen);
        private Brush black = new SolidBrush(Color.Black);
        private Brush red = new SolidBrush(Color.Red);
        private Brush lightBlue = new SolidBrush(Color.LightBlue);

        private bool simulationRunning = false;
        private double simulationDuration = 0;

        public const string directions = "Left Click Sets Current Position and Right Click Sets Target Position";
        
        public MainForm()
        {
            InitializeComponent();

            // Initial board setup
            //simulatorBoardBitmap = new Bitmap(simulationBoard.Size.Width, simulationBoard.Size.Height);           

            simulationPictureBox.Image = new Bitmap(5280, 5280);            

            x1 = simulationBoard.Size.Width / 4;
            y1 = simulationBoard.Size.Height / 10;
            z1 = zAxisBoard.Size.Height / 3;

            x2 = simulationBoard.Size.Width / 2;
            y2 = simulationBoard.Size.Height - simulationBoard.Size.Height/10;
            z2 = zAxisBoard.Size.Height / 2;

            zXConstant = zAxisBoard.Size.Width / 2;
            
            infoLabel.Text = directions;

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            simulationBoard, new object[] { true });

            typeof(PictureBox).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            simulationPictureBox, new object[] { true });

            paintSimulationBoard();
        }                

        private void simulationBoard_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawImageUnscaled(this.simulatorBoardBitmap, Point.Empty);    
            //simulationPictureBox.Update();
        }

        private void clearSimulationBoard()
        {
            simulationPictureBox.Image = new Bitmap(5280, 5280);
        }

        private void paintSimulationBoard()
        {
            clearSimulationBoard();
            Graphics gObj = Graphics.FromImage(simulationPictureBox.Image);

            //gObj.FillRectangle(lightGreen, 0, 0, simulationBoard.Size.Width, simulationBoard.Size.Height);
            //gObj.FillRectangle(black, x1, y1, SimulatedObject.objectSize * 10, SimulatedObject.objectSize * 10);
            //gObj.FillRectangle(red, x2, y2, SimulatedObject.objectSize * 10, SimulatedObject.objectSize * 10);

            gObj.FillEllipse(black, new Rectangle(x1, y1, 20, 20));
            gObj.FillEllipse(red, new Rectangle(x2, y2, 20, 20));
            simulationPictureBox.Invalidate();
        }

        private void simulationPictureBox_Click(object sender, EventArgs e)
        {
            if (!simulationRunning)
            {
                Point point = simulationPictureBox.PointToClient(Cursor.Position);
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

            gObj.FillRectangle(black, zXConstant, z1, SimulatedObject.objectSize, SimulatedObject.objectSize);
            gObj.FillRectangle(red, zXConstant, z2, SimulatedObject.objectSize, SimulatedObject.objectSize);
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
            clearSimulationBoard();
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
            SimulatorController controller = new SimulatorController(simulationPictureBox, zAxisBoard, numberOfObjects, simulationDuration, x1, y1, z1, x2, y2, z2, velocity);
            
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

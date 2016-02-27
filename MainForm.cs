using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
        private Brush black = new SolidBrush(Color.Black);
        private Brush red = new SolidBrush(Color.Red);
        private CancellationTokenSource tokenSource = null;

        private bool simulationRunning = false;
        private double simulationDuration = 0;
        private int speedTrackbarValue = 1;

        public const string directions = "Left Click Sets Current Position and Right Click Sets Target Position";

        private BetterTimer mainTimer = new BetterTimer();

        public MainForm()
        {
            InitializeComponent();
            
            mainTimer.Enabled = false;
            mainTimer.Interval = 1000;
            mainTimer.Tick += new EventHandler(mainTimer_Tick);            
    
            // Initial board setup
            //simulatorBoardBitmap = new Bitmap(simulationBoard.Size.Width, simulationBoard.Size.Height);           

            clearXY();
            clearZ();

            x1 = xyAxisPanel.Size.Width / 4;
            y1 = xyAxisPanel.Size.Height / 10;
            z1 = zAxisPictureBox.Size.Height / 3;

            x2 = xyAxisPanel.Size.Width / 2;
            y2 = xyAxisPanel.Size.Height - xyAxisPanel.Size.Height/10;
            z2 = zAxisPictureBox.Size.Height / 2;

            zXConstant = zAxisPictureBox.Size.Width / 2;
            
            infoLabel.Text = directions;

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            xyAxisPanel, new object[] { true });

            typeof(PictureBox).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            simulationPictureBox, new object[] { true });

            paintXY();
            paintZ();
        }                
        
        private void clearXY()
        {
            simulationPictureBox.Image = new Bitmap(5280, 5280);
        }

        private void paintXY()
        {
            clearXY();
            Graphics gObj = Graphics.FromImage(simulationPictureBox.Image);
            
            gObj.FillEllipse(black, new Rectangle(x1, y1, 20, 20));
            gObj.FillEllipse(red, new Rectangle(x2, y2, 20, 20));
            simulationPictureBox.Invalidate();
        }

        private void xy_Click(object sender, EventArgs e)
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

                paintXY();
            }
        }

        private void z_Click(object sender, EventArgs e)
        {
            if (!simulationRunning)
            {
                Point point = zAxisPictureBox.PointToClient(Cursor.Position);
                MouseEventArgs me = (MouseEventArgs)e;
                if (me.Button == MouseButtons.Left)
                {
                    z1 = point.Y;
                }
                else
                {
                    z2 = point.Y;
                }

                paintZ();
            }
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            // Update anything that needs to update less frequently such as labels and text and whatnot
            speedTrackbarValue = speedTrackBar.Value;
        }

        private void clearZ()
        {
            zAxisPictureBox.Image = new Bitmap(82, 537);
        }        

        private void paintZ()
        {
            clearZ();
            Graphics gObj = Graphics.FromImage(zAxisPictureBox.Image);

            gObj.FillRectangle(black, zXConstant, z1, SimulatedObject.objectSize, SimulatedObject.objectSize);
            gObj.FillRectangle(red, zXConstant, z2, SimulatedObject.objectSize, SimulatedObject.objectSize);

            zAxisPictureBox.Invalidate();
        }        

        private async void goButton_Click(object sender, EventArgs e)
        {
            if (!simulationRunning)
            {
                resultsListBox.Items.Clear();
                clearXY();
                paintZ();

                simulationRunning = true;
                double tX = x2 - x1;
                double tY = y2 - y1;
                double tZ = z2 - z1;
                double tLength = Math.Sqrt(tX * tX + tY * tY + tZ * tZ);
                simulationDuration = Math.Round(tLength / velocity, 4);
                infoLabel.Text = "Distance = " + Math.Round(tLength, 0).ToString() + " feet. Expected Flight Duration in Seconds = " + simulationDuration.ToString();

                stopwatch.Reset();
                stopwatch.Start();
                mainTimer.Interval = 100;
                mainTimer.Enabled = true;
                
                goButton.Text = "STOP!";

                int numberOfObjects = Convert.ToInt32(countDropDown.Text);

                speedTrackbarValue = speedTrackBar.Value;
                SimulatorController controller = new SimulatorController(simulationPictureBox, zAxisPictureBox, ref speedTrackbarValue,
                    numberOfObjects, simulationDuration, x1, y1, z1, x2, y2, z2, velocity);

                tokenSource = new CancellationTokenSource();

                Application.DoEvents();
                await Task.Run(() =>
                {
                    controller.Run(tokenSource);
                });

                foreach (var distance in controller.getDistances())
                {
                    resultsListBox.Items.Add(Math.Round(distance, 2).ToString());
                }
            }
            else
            {
                tokenSource.Cancel();
            }

            stopwatch.Stop();
            mainTimer.Enabled = false;
            infoLabel.Text = directions;            
            goButton.Text = "GO!";
            simulationRunning = false;
        }


    }
}

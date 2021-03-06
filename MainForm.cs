﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        public const string directions = "Left Click Sets Current Position and Right Click Sets Target Position";

        private System.Timers.Timer mainTimer = new System.Timers.Timer();

        private SpeedReference stv = new SpeedReference();
        private SimulatorController controller;
        private double totalSimulationDistance = 0;

        public MainForm()
        {
            InitializeComponent();

            mainTimer.Enabled = false;
            mainTimer.Interval = 1000;
            mainTimer.Elapsed += new System.Timers.ElapsedEventHandler(mainTimer_Tick);            
    
            // Initial board setup
            //simulatorBoardBitmap = new Bitmap(simulationBoard.Size.Width, simulationBoard.Size.Height);           

            clearXY();
            clearZ();

            totalSimulatedObjects_dd.SelectedIndex = 3;
            IMU_GyroAccuracy_dd.SelectedIndex = 2;
            IMU_AccelAccuracy_dd.SelectedIndex = 0;

            x1 = xyAxisPanel.Size.Width / 2;
            y1 = xyAxisPanel.Size.Height / 2;
            z1 = zAxisPictureBox.Size.Height / 2;

            x2 = xyAxisPanel.Size.Width-100;
            y2 = simulationPictureBox.Image.Height-100;
            z2 = zAxisPictureBox.Size.Height / 2;

            // Just to make it really about x and y to trouble shoot IMU
            z1 = z2;

            zXConstant = zAxisPictureBox.Size.Width / 2;
            
            infoLabel.Text = directions;

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            xyAxisPanel, new object[] { true });

            typeof(PictureBox).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            simulationPictureBox, new object[] { true });

            ((Bitmap)origPicBox.Image).MakeTransparent(((Bitmap)origPicBox.Image).GetPixel(1, 1));
            origPicBox.BackColor = System.Drawing.Color.Transparent;
            origPicBox.Parent = simulationPictureBox;

            ((Bitmap)destPicBox.Image).MakeTransparent(((Bitmap)destPicBox.Image).GetPixel(1, 1));
            destPicBox.BackColor = System.Drawing.Color.Transparent;
            destPicBox.Parent = simulationPictureBox;

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

            //gObj.ScaleTransform(zoomFactor, zoomFactor);

            //gObj.FillEllipse(black, new Rectangle(x1, y1, 20, 20));
            //gObj.FillEllipse(red, new Rectangle(x2, y2, 20, 20));
            origPicBox.Visible = true;
            destPicBox.Visible = true;
            origPicBox.Location = new Point(x1 - (origPicBox.Width / 2), y1 - origPicBox.Height);
            destPicBox.Location = new Point(x2 - (destPicBox.Width / 2), y2 - destPicBox.Height);

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

        private void xyAxisPanel_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.ScaleTransform(2, 2);
            //simulationPictureBox.Refresh();
        }

        private void speedTrackBar_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                var size = speedTrackBar.Size.Width;
                var total = speedTrackBar.Maximum - speedTrackBar.Minimum + 1;
                size -= (size / (total + 2));

                Point point = speedTrackBar.PointToClient(Cursor.Position);
                var clickSpot = (Convert.ToDouble(point.X) / size);
                var newValue = Convert.ToInt32(clickSpot * total);
                speedTrackBar.Value = newValue;
            }
            catch
            {

            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // Try to maximize the viewing window
            var bottomPanelLocation = bottomPanel.Location;
            var rightPanelLocation = rightPanel.Location;
            var xyAxisPanelLocation = xyAxisPanel.Location;
            var newSize = new Size(rightPanelLocation.X- xyAxisPanelLocation.X, bottomPanelLocation.Y - xyAxisPanelLocation.Y);
            xyAxisPanel.Size = newSize;
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

        private void Velocity_tb_Leave(object sender, EventArgs e)
        {
            try
            {
                velocity = Convert.ToInt32(Velocity_tb.Text);
                velocity *= 5280;
                velocity /= 3600; // ft per sec
            }
            catch
            {

            }
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            // Update anything that needs to update less frequently such as labels and text and whatnot
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action(timerAction));
            }
        }

        private void timerAction()
        {
            int simulatedSecondsElapsed = controller.currentSimulatedTickCount / Convert.ToInt32(SimulatorController.ticksPerSecond);
            infoLabel.Text = "Distance = " + Math.Round(totalSimulationDistance, 0).ToString() + " feet. Expected Flight Duration in Seconds = " + simulationDuration.ToString() + ", Simulated Seconds = "+ simulatedSecondsElapsed.ToString();
            stv.speedTrackbarValue = speedTrackBar.Value;
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
                enableORdisable(false);

                origPicBox.Visible = false;
                destPicBox.Visible = false;

                int repeatCount = 1;
                try
                {
                    repeatCount = Convert.ToInt32(Repeat_tb.Text);
                }
                catch
                {

                }

                tokenSource = new CancellationTokenSource();
                double tX = x2 - x1;
                double tY = y2 - y1;
                double tZ = z2 - z1;
                totalSimulationDistance = Math.Sqrt(tX * tX + tY * tY + tZ * tZ);
                if (Log_cb.Checked)
                {
                    // Cut it short by GPS length to avoid last minute bad decisions
                    totalSimulationDistance -= GPS_Module.maxRadius;
                }
                simulationDuration = Math.Round(totalSimulationDistance / velocity, 4);
                double imuGyroAccuracy = Convert.ToDouble(IMU_GyroAccuracy_dd.Text);
                double imuAccelAccuracy = Convert.ToDouble(IMU_AccelAccuracy_dd.Text);
                double gpsLoss = Convert.ToDouble(gpsLoss_tb.Text);
                int roundTripTime = Convert.ToInt32(RTT_tb.Text);

                StringBuilder sb = new StringBuilder();
                StreamWriter logFile = null;

                if (Log_cb.Checked)
                {
                    string logFileName = @"C:\temp\cosc636.csv";
                    try {                        
                        logFile = new StreamWriter(logFileName);
                        sb.Append("totalSimulationDistance " + totalSimulationDistance.ToString());
                        sb.Append(",");
                        sb.Append("simulationDuration " + simulationDuration.ToString());
                        sb.Append(",");
                        sb.Append("imuGyroAccuracy " + imuGyroAccuracy.ToString());
                        sb.Append(",");
                        sb.Append("imuAccelAccuracy " + imuAccelAccuracy.ToString());
                        sb.Append(",");
                        sb.Append("gpsLoss " + gpsLoss.ToString());
                        logFile.WriteLine(sb.ToString());
                    }
                    catch {
                        MessageBox.Show("Path to " + logFileName + " cannot be written to.");
                    }
                }                

                for (int simCount = 0; !tokenSource.Token.IsCancellationRequested && simCount < repeatCount; simCount++)
                {
                    resultsListBox.Items.Clear();

                    clearXY();
                    paintZ();

                    simulationRunning = true;
                    
                    infoLabel.Text = "Distance = " + Math.Round(totalSimulationDistance, 0).ToString() + " feet. Expected Flight Duration in Seconds = " + simulationDuration.ToString();

                    stopwatch.Reset();
                    stopwatch.Start();
                    mainTimer.Interval = 100;
                    mainTimer.Enabled = true;

                    goButton.Text = "STOP!";

                    int formationStyle = Convert.ToInt32(totalSimulatedObjects_dd.SelectedIndex);                    

                    Position origin = new Position(x1, y1, z1);
                    Position destination = new Position(x2, y2, z2);

                    stv.speedTrackbarValue = speedTrackBar.Value;
                    controller = new SimulatorController(xyAxisPanel, simulationPictureBox, zAxisPictureBox, stv,
                        formationStyle, simulationDuration, origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, roundTripTime, gpsLoss);
                    
                    Application.DoEvents();
                    await Task.Run(() =>
                    {
                        controller.Run(tokenSource);
                    });

                    sb.Clear();
                    foreach (var info in controller.getDistances())
                    {
                        double distance = info.Item1;
                        string descript = info.Item2;

                        if (Log_cb.Checked) {
                            // We cut it short by GPS length to avoid last minute bad decisions
                            distance -= GPS_Module.maxRadius;
                        }

                        sb.Append(distance.ToString());
                        sb.Append(",");
                        // Assume any return value in description is to indicate NOGPS
                        sb.Append(descript.Length>0 ? "0" : "1");
                        sb.Append(",");
                        resultsListBox.Items.Add(Math.Round(distance, 2).ToString() + " " + descript);
                    }
                    if (logFile != null)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        logFile.WriteLine(sb.ToString());
                    }
                }
                if (logFile != null)
                {
                    logFile.Flush();
                    logFile.Close();
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

            enableORdisable(true);
        }

        private void enableORdisable(bool which)
        {
            gpsLoss_tb.Enabled = which;
            Velocity_tb.Enabled = which;
            RTT_tb.Enabled = which;
            Repeat_tb.Enabled = which;
            totalSimulatedObjects_dd.Enabled = which;
            IMU_AccelAccuracy_dd.Enabled = which;
            IMU_GyroAccuracy_dd.Enabled = which;
            Log_cb.Enabled = which;
        }
    }
}

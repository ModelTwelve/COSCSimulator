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

        public class MovingObj
        {
            Random random = new Random();
            
            public double actualXPosition { get; private set; }
            public double actualYPosition { get; private set; }
            public double actualZPosition { get; private set; }

            public double expectedXPosition { get; private set; }
            public double expectedYPosition { get; private set; }
            public double expectedZPosition { get; private set; }

            public double targetXPosition { get; private set; }
            public double targetYPosition { get; private set; }
            public double targetZPosition { get; private set; }

            public double prevX { get; private set; }
            public double prevY { get; private set; }
            public double prevZ { get; private set; }

            public double speed { get; private set; }

            public int ticks = 0;

            private double ticksPerSecond = 1000, tickSpeed;

            public MovingObj(double startX, double startY, double startZ, double goalX, double goalY, double goalZ, double velocity)
            {
                actualXPosition = startX;
                actualYPosition = startY;
                actualZPosition = startZ;

                expectedXPosition = startX;
                expectedYPosition = startY;
                expectedZPosition = startZ;

                targetXPosition = goalX;
                targetYPosition = goalY;
                targetZPosition = goalZ;

                // I want to 
                speed = velocity;
                tickSpeed = speed / ticksPerSecond;

                prevX = -1;
                prevY = -1;                
            }

            public int GPS()
            {
                // 5 meters = 0 to 16 ft would be approx +/- 16 ft from your actual
                return random.Next(-16, 16);
            }

            public bool areWeThereYet()
            {
                return actualXPosition == targetXPosition && actualYPosition == targetYPosition && actualZPosition == targetZPosition;
            }

            public void stepTowards()
            {
                ++ticks;

                prevX = actualXPosition;
                prevY = actualYPosition;
                prevZ = actualZPosition;

                // We are actually here
                double tX = targetXPosition - actualXPosition;
                double tY = targetYPosition - actualYPosition;
                double tZ = targetZPosition - actualZPosition;

                // We think we're here
                double eX = targetXPosition - expectedXPosition;
                double eY = targetYPosition - expectedYPosition;
                double eZ = targetZPosition - expectedZPosition;

                // We actually need to move this far
                double tLength = Math.Sqrt(tX * tX + tY * tY + tZ * tZ);
                // We think we need to move this far
                double eLength = Math.Sqrt(eX * eX + eY * eY + eZ * eZ);

                if (tLength > speed)
                {
                    // move towards the goal
                    //actualXPosition = actualXPosition + (speed * tX / tLength);
                    //actualYPosition = actualYPosition + (speed * tY / tLength);
                    //actualZPosition = actualZPosition + (speed * tZ / tLength);
                    
                    // Move towards the goal using where we think we are
                    // and allowing that to affect the actual positioning
                    actualXPosition = actualXPosition + (tickSpeed * eX / eLength);
                    actualYPosition = actualYPosition + (tickSpeed * eY / eLength);
                    actualZPosition = actualZPosition + (tickSpeed * eZ / eLength);

                    // Pretend like the GPS updates every second
                    if (ticks % Convert.ToInt32(ticksPerSecond) == 0)
                    {
                        expectedXPosition = actualXPosition + GPS();
                        expectedYPosition = actualYPosition + GPS();
                        expectedZPosition = actualZPosition + GPS();
                    }
                    else
                    {
                        // Else continue on your wrong path
                        expectedXPosition = expectedXPosition + (tickSpeed * eX / eLength);
                        expectedYPosition = expectedYPosition + (tickSpeed * eY / eLength);
                        expectedZPosition = expectedZPosition + (tickSpeed * eZ / eLength);
                    }
                }
                else
                {
                    // Last step
                    if (tLength > tickSpeed)
                    {
                        actualXPosition = actualXPosition + (tickSpeed * eX / eLength);
                        actualYPosition = actualYPosition + (tickSpeed * eY / eLength);
                        actualZPosition = actualZPosition + (tickSpeed * eZ / eLength);

                        expectedXPosition = actualXPosition;
                        expectedYPosition = actualYPosition;
                        expectedZPosition = actualZPosition;
                    }
                    else
                    {
                        actualXPosition = targetXPosition;
                        actualYPosition = targetYPosition;
                        actualZPosition = targetZPosition;
                    }
                }                
            }
        }

        private int x1 = 20, y1 = 20, x2 = 100, y2 = 100, z1 = 20, z2 = 100;
        private int zXConstant;
        private double velocity = 44;
        private Stopwatch stopwatch = new Stopwatch();
        private int size = 10;
        private Brush lightGreen = new SolidBrush(Color.LightGreen);
        private Brush black = new SolidBrush(Color.Black);
        private Brush red = new SolidBrush(Color.Red);
        private Brush lightBlue = new SolidBrush(Color.LightBlue);

        private bool simulationRunning = false;

        public const string directions = "Left Click Sets Current Position and Right Click Sets Target Position";

        public MainForm()
        {
            InitializeComponent();

            x2 = simulationBoard.Size.Width - 20;
            y2 = simulationBoard.Size.Height - 20;
            z2 = zAxisBoard.Size.Height - 20;

            zXConstant = zAxisBoard.Size.Width / 2;

            setCurrentPositionLabel();
            setGoalPositionLabel();
            infoLabel.Text = directions;
        }

        private void setCurrentPositionLabel()
        {
            CurrentPositionLabel.Text = String.Format("({0},{1},{2})", x1.ToString(), y1.ToString(), z1.ToString());
        }

        private void setGoalPositionLabel()
        {
            GoalPositionLabel.Text = String.Format("({0},{1},{2})", x2.ToString(), y2.ToString(), z2.ToString());
        }

        private void doSimulation()
        {
            Graphics gObjSimulationBoard = simulationBoard.CreateGraphics();
            Graphics gObjZAxisBoard = zAxisBoard.CreateGraphics();    

            //int goalX = simulationBoard.Size.Width;
            //int goalY = simulationBoard.Size.Height;
                        
            int delay = 10;

            // Make velocity proportional to the delay
            MovingObj mObj = new MovingObj(x1, y1, z1, x2, y2, z2, velocity);
            int steps = 0;
            while (!mObj.areWeThereYet())
            {
                ++steps;
                mObj.stepTowards();

                gObjSimulationBoard.FillRectangle(lightGreen, Convert.ToInt32(mObj.prevX), Convert.ToInt32(mObj.prevY), size, size);
                gObjSimulationBoard.FillRectangle(black, Convert.ToInt32(mObj.actualXPosition), Convert.ToInt32(mObj.actualYPosition), size, size);

                gObjZAxisBoard.FillRectangle(lightBlue, zXConstant, Convert.ToInt32(mObj.prevZ), size, size);
                gObjZAxisBoard.FillRectangle(black, zXConstant, Convert.ToInt32(mObj.actualZPosition), size, size);

                x1 = Convert.ToInt32(mObj.actualXPosition);
                y1 = Convert.ToInt32(mObj.actualYPosition);
                z1 = Convert.ToInt32(mObj.actualZPosition);

                if (steps % 100 == 0)
                {
                    Task.Delay(delay).Wait();
                }
            }                        
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
                setCurrentPositionLabel();
                setGoalPositionLabel();
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
            //double milli = Convert.ToDouble(stopwatch.ElapsedMilliseconds);
            //this.Text = "Simulation - Seconds Elapsed " + Math.Round(milli / 1000.0, 2).ToString();
            setCurrentPositionLabel();
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

                setCurrentPositionLabel();
                setGoalPositionLabel();
                paintZAxisBoard();
            }
        }

        private async void goButton_Click(object sender, EventArgs e)
        {
            simulationRunning = true;
            double tX = x2 - x1;
            double tY = y2 - y1;
            double tZ = z2 - z1;
            double tLength = Math.Sqrt(tX * tX + tY * tY + tZ * tZ);
            infoLabel.Text = "Distance = " + Math.Round(tLength, 0).ToString() + " feet. Expected Flight Duration in Seconds = " + Math.Round(tLength / velocity, 0).ToString();

            stopwatch.Reset();
            stopwatch.Start();
            timer1.Enabled = true;
            await Task.Run(() =>
            {
                doSimulation();
            });
            stopwatch.Stop();
            timer1.Enabled = false;
            infoLabel.Text = directions;
            simulationRunning = false;
        }


    }
}

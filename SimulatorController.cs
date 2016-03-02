using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COSCSimulator
{
    public class SimulatorController
    {
        public const double ticksPerSecond = 1000;

        List<SimulatedObject> movingObjects = new List<SimulatedObject>();
        private double simulationDuration;
        private float zXConstant;

        PictureBox xyPictureBox, zPictureBox;
        Graphics xyGraphics, zGraphics;
        int speedTrackbarValue;

        private Brush black = new SolidBrush(Color.Black);
        private Brush red = new SolidBrush(Color.Red);
        private Brush yellow = new SolidBrush(Color.Yellow);

        public SimulatorController(PictureBox xyPictureBox, PictureBox zictureBox,
            ref int speedTrackbarValue,
            int formationNumber, double simulationDuration,
            double startX, double startY, double startZ, 
            double goalX, double goalY, double goalZ, 
            double velocity, double imu_Theta, double imu_Phi,
            double gpsLoss)
        {
            // Make the duration (in seconds) into ticks
            this.simulationDuration = Math.Floor(simulationDuration * SimulatorController.ticksPerSecond);

            this.xyPictureBox = xyPictureBox;
            this.zPictureBox = zictureBox;
            xyGraphics = Graphics.FromImage(this.xyPictureBox.Image);
            zGraphics = Graphics.FromImage(this.zPictureBox.Image);

            this.speedTrackbarValue = speedTrackbarValue;

            //gObjSimulationBoard.SmoothingMode = SmoothingMode.AntiAlias;
            //gObjZAxisBoard.SmoothingMode = SmoothingMode.AntiAlias;

            //xyGraphics.ScaleTransform(10, 10);

            zXConstant = zictureBox.Size.Width / 2;

            buildFormation(formationNumber,startX, startY, startZ,goalX, goalY, goalZ,velocity, imu_Theta, imu_Phi, gpsLoss);
        }

        public List<double> getDistances()
        {
            List<double> rv = new List<double>();
            foreach(var mObj in movingObjects)
            {
                rv.Add(mObj.distanceFromTarget());
            }
            return rv;
        }

        private void buildFormation(int formationNumber,
            double startX, double startY, double startZ,
            double goalX, double goalY, double goalZ,
            double velocity, double imu_Theta, double imu_Phi, double gpsLoss)
        {                      
            switch (formationNumber)
            {
                case 1:
                    movingObjects.Add(new SimulatedObject(startX, startY, startZ, goalX, goalY, goalZ, velocity, imu_Theta, imu_Phi, gpsLoss));
                    break;
                case 2:
                    movingObjects.Add(new SimulatedObject(startX, startY, startZ, goalX, goalY, goalZ, velocity, imu_Theta, imu_Phi, gpsLoss));
                    movingObjects.Add(new SimulatedObject(startX - 40, startY, startZ, goalX - 40, goalY, goalZ, velocity, imu_Theta, imu_Phi, 0));
                    break;
                case 3:
                    movingObjects.Add(new SimulatedObject(startX, startY, startZ, goalX, goalY, goalZ, velocity, imu_Theta, imu_Phi, gpsLoss));
                    movingObjects.Add(new SimulatedObject(startX - 40, startY, startZ, goalX - 40, goalY, goalZ, velocity, imu_Theta, imu_Phi, 0));
                    movingObjects.Add(new SimulatedObject(startX + 40, startY, startZ, goalX + 40, goalY, goalZ, velocity, imu_Theta, imu_Phi, 0));
                    break;
            }

            // Redraw goals
            Pen purplePen = new Pen(Color.Purple, 3);
            foreach (var mObj in movingObjects)
            {
                xyGraphics.FillRectangle(red, Convert.ToSingle(mObj.targetPosition.x), Convert.ToSingle(mObj.targetPosition.y), SimulatedObject.objectSize, SimulatedObject.objectSize);
                zGraphics.FillRectangle(red, zXConstant, Convert.ToSingle(mObj.targetPosition.z), SimulatedObject.objectSize, SimulatedObject.objectSize);
                xyGraphics.DrawEllipse(purplePen, 
                    Convert.ToSingle(mObj.targetPosition.x)-GPS_Module.maxRadius + (SimulatedObject.objectSize/2), 
                    Convert.ToSingle(mObj.targetPosition.y)-GPS_Module.maxRadius + (SimulatedObject.objectSize / 2), GPS_Module.maxRadius + GPS_Module.maxRadius, 
                    GPS_Module.maxRadius + GPS_Module.maxRadius);                
            }
        }

        public void Run(CancellationTokenSource token)
        {
            //int delay = 1;

            int steps = 0;

            bool allDone = false;

            while (!allDone)
            {
                int speedOffset = speedTrackbarValue * 10;

                ++steps;
                if (steps % speedOffset == 0)
                {
                    //Task.Delay(delay).Wait();
                    Thread.Sleep(1);
                }

                // Have we already taken enough steps to end the simulation?
                if (steps >= simulationDuration)
                {
                    // No need to set alldone let's just get outta here
                    break;
                }
                // Go ahead and set alldone to true and we'll try and disprove that
                allDone = true;
                foreach (var mObj in movingObjects)
                {
                    bool areWeThere = mObj.areWeThereYet();
                    allDone = allDone && areWeThere;

                    mObj.stepTowards();

                    showNewLocation(mObj);
                }
                xyPictureBox.Invalidate();
                zPictureBox.Invalidate();

                if (token.IsCancellationRequested)
                {
                    allDone = true;
                }
            }            
        }

        private void showNewLocation(SimulatedObject mObj)
        {
            xyGraphics.FillRectangle(yellow, Convert.ToInt32(mObj.prevActualPosition.x), Convert.ToInt32(mObj.prevActualPosition.y), SimulatedObject.objectSize, SimulatedObject.objectSize);            
            xyGraphics.FillRectangle(black, Convert.ToInt32(mObj.actualPosition.x), Convert.ToInt32(mObj.actualPosition.y), SimulatedObject.objectSize, SimulatedObject.objectSize);
            //gObjSimulationBoard.FillRectangle(yellow, Convert.ToInt32(mObj.prevActualXPosition) + (size / 2), Convert.ToInt32(mObj.prevActualYPosition) + (size / 2), 1, 1);

            zGraphics.FillRectangle(yellow, zXConstant, Convert.ToInt32(mObj.prevActualPosition.z), SimulatedObject.objectSize, SimulatedObject.objectSize);            
            zGraphics.FillRectangle(black, zXConstant, Convert.ToInt32(mObj.actualPosition.z), SimulatedObject.objectSize, SimulatedObject.objectSize);
            //gObjZAxisBoard.FillRectangle(yellow, zXConstant + (size / 2), Convert.ToInt32(mObj.prevActualZPosition), 1, 1);
        }
    }
}

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

        public SimulatorController(Panel xyPanel, PictureBox xyPictureBox, PictureBox zictureBox,
            ref int speedTrackbarValue,
            int formationStyle, double simulationDuration,
            Position origin, Position destination,
            double velocity, double imuGyroAccuracy, double imuAccelAccuracy,
            double gpsLoss)
        {
            // Make the duration (in seconds) into ticks
            this.simulationDuration = Math.Floor(simulationDuration * SimulatorController.ticksPerSecond);

            this.xyPictureBox = xyPictureBox;
            this.zPictureBox = zictureBox;            

            xyGraphics = Graphics.FromImage(this.xyPictureBox.Image);
            zGraphics = Graphics.FromImage(this.zPictureBox.Image);

            this.speedTrackbarValue = speedTrackbarValue;            

            zXConstant = zictureBox.Size.Width / 2;

            buildFormation(formationStyle, origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, gpsLoss);
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
            Position origin, Position destination,
            double velocity, double imuGyroAccuracy, double imuAccelAccuracy, double gpsLoss)
        {                      
            switch (formationNumber)
            {
                case 0:
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, gpsLoss));
                    break;
                case 1:
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, gpsLoss));
                    origin.x -= 40; destination.x -= 40;
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, 0));
                    break;
                case 2:
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, gpsLoss));
                    origin.x -= 40; destination.x -= 40;
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, 0));
                    origin.x += 80; destination.x += 80;
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, 0));
                    break;
                case 3:
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, gpsLoss));

                    origin.x -= 40; destination.x -= 40;
                    origin.y -= 40; destination.y -= 40;
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, 0));

                    origin.y += 80; destination.y += 80;
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, 0));
                    
                    origin.x += 80; destination.x += 80;
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, 0));

                    origin.y -= 80; destination.y -= 80;
                    movingObjects.Add(new SimulatedObject(origin, destination, velocity, imuGyroAccuracy, imuAccelAccuracy, 0));

                    movingObjects[0].assignNodes(new List<SimulatedObject> { movingObjects[1] , movingObjects[2] , movingObjects[3] });
                    break;
                case 4:
                    // going to goto layers now
                    buildExtendedFormation(10, origin, destination, velocity, imuGyroAccuracy,
                        imuAccelAccuracy, gpsLoss);
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

        private void buildExtendedFormation(int layers,
            Position origin, Position destination,
            double velocity, double imuGyroAccuracy, double imuAccelAccuracy, double gpsLoss)
        {

            //double dX = destination.x - origin.x;
            //double dY = destination.y - origin.y;
            //double theta = Math.Atan(dX / dY) * 180.0 / Math.PI;

            //double spacingDistance = 40;

            //dX = origin.x + spacingDistance * Math.Cos((Convert.ToDouble(System.Math.PI) / 180) * theta);
            //dY = origin.y + spacingDistance * Math.Sin((Convert.ToDouble(System.Math.PI) / 180) * theta);

            //dX -= origin.x;
            //dY -= origin.y;

            //double dLength = Math.Sqrt(dX * dX + dY * dY);

            double spacing = 40;
            int middleLayer = layers / 2;
            int leaveOff = 1;
            for (int layerNumber = 1; layerNumber < layers; layerNumber++)
            {
                double originXoffset = origin.x - (spacing * layerNumber);
                double originYoffset = origin.y - (spacing * layerNumber);

                double destinationXoffset = destination.x - (spacing * layerNumber);
                double destinationYoffset = destination.y - (spacing * layerNumber);
                int maxForRow = (layerNumber - 1) * 2 + 1;
                
                for (int oNumber = 1; oNumber <= maxForRow; oNumber++)
                {
                    originXoffset += spacing;
                    destinationXoffset += spacing;

                    Position newOrigin = new Position();
                    newOrigin.Clone(origin);
                    newOrigin.x = originXoffset;
                    newOrigin.y = originYoffset;
                    Position newDestination = new Position();
                    newDestination.Clone(destination);
                    newDestination.x = destinationXoffset;
                    newDestination.y = destinationYoffset;

                    if (layerNumber <= middleLayer)
                    {
                        movingObjects.Add(new SimulatedObject(newOrigin, newDestination, velocity, imuGyroAccuracy, imuAccelAccuracy, 0));
                    }
                    else if ( (oNumber <= (maxForRow+1) / 2 - leaveOff) || (oNumber >= (maxForRow+1)/2 + leaveOff) )
                    {                        
                        movingObjects.Add(new SimulatedObject(newOrigin, newDestination, velocity, imuGyroAccuracy, imuAccelAccuracy, 0));
                    }
                }
                if (layerNumber > middleLayer)
                {
                    leaveOff *= 2;
                }
            }
            for(int x=1;x<53;x++)
            {
                movingObjects[x].assignNodes(new List<SimulatedObject> { movingObjects[0], movingObjects[53], movingObjects[54] }, gpsLoss);
            }
        }

        public void Run(CancellationTokenSource token)
        {
            //int delay = 1;

            int steps = 0;

            bool allDone = false;

            int speedOffset = Convert.ToInt32(Math.Pow(2, speedTrackbarValue));
            while (!allDone)
            {               
                ++steps;
                if (steps % speedOffset == 0)
                {
                    //Task.Delay(1).Wait();
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

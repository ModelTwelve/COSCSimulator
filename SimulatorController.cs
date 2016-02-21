using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
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

        Graphics gObjSimulationBoard, gObjZAxisBoard;
        
        private Brush lightGreen = new SolidBrush(Color.LightGreen);
        private Brush black = new SolidBrush(Color.Black);
        private Brush red = new SolidBrush(Color.Red);
        private Brush lightBlue = new SolidBrush(Color.LightBlue);
        private Brush yellow = new SolidBrush(Color.Yellow);

        public SimulatorController(Panel simulationBoard, Panel zAxisBoard,
            int formationNumber, double simulationDuration,
            double startX, double startY, double startZ, 
            double goalX, double goalY, double goalZ, 
            double velocity)
        {
            // Make the duration (in seconds) into ticks
            this.simulationDuration = Math.Floor(simulationDuration * SimulatorController.ticksPerSecond);

            gObjSimulationBoard = simulationBoard.CreateGraphics();
            //gObjSimulationBoard.SmoothingMode = SmoothingMode.AntiAlias;
            //gObjSimulationBoard.ScaleTransform(2, 2);
            gObjZAxisBoard = zAxisBoard.CreateGraphics();

            zXConstant = zAxisBoard.Size.Width / 2;

            buildFormation(formationNumber,startX, startY, startZ,goalX, goalY, goalZ,velocity);
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
            double velocity)
        {                      
            switch (formationNumber)
            {
                case 1:
                    movingObjects.Add(new SimulatedObject(startX, startY, startZ, goalX, goalY, goalZ, velocity));
                    break;
                case 2:
                    movingObjects.Add(new SimulatedObject(startX, startY, startZ, goalX, goalY, goalZ, velocity));
                    movingObjects.Add(new SimulatedObject(startX - 40, startY, startZ, goalX - 40, goalY, goalZ, velocity));
                    break;
                case 3:
                    movingObjects.Add(new SimulatedObject(startX, startY, startZ, goalX, goalY, goalZ, velocity));
                    movingObjects.Add(new SimulatedObject(startX - 40, startY, startZ, goalX - 40, goalY, goalZ, velocity));
                    movingObjects.Add(new SimulatedObject(startX + 40, startY, startZ, goalX + 40, goalY, goalZ, velocity));
                    break;
            }

            // Redraw goals
            foreach (var mObj in movingObjects)
            {
                gObjSimulationBoard.FillRectangle(red, Convert.ToSingle(mObj.targetPosition.x), Convert.ToSingle(mObj.targetPosition.y), SimulatedObject.objectSize, SimulatedObject.objectSize);
                gObjZAxisBoard.FillRectangle(red, zXConstant, Convert.ToSingle(mObj.targetPosition.z), SimulatedObject.objectSize, SimulatedObject.objectSize);
            }
        }

        public void Run()
        {
            int delay = 10;

            int steps = 0;

            bool allDone = false;

            while (!allDone)
            {
                ++steps;
                if (steps % SimulatorController.ticksPerSecond == 0)
                {
                    //Task.Delay(delay).Wait();
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
            }            
        }

        private void showNewLocation(SimulatedObject mObj)
        {
            gObjSimulationBoard.FillRectangle(yellow, Convert.ToInt32(mObj.prevActualPosition.x), Convert.ToInt32(mObj.prevActualPosition.y), SimulatedObject.objectSize, SimulatedObject.objectSize);            
            gObjSimulationBoard.FillRectangle(black, Convert.ToInt32(mObj.actualPosition.x), Convert.ToInt32(mObj.actualPosition.y), SimulatedObject.objectSize, SimulatedObject.objectSize);
            //gObjSimulationBoard.FillRectangle(yellow, Convert.ToInt32(mObj.prevActualXPosition) + (size / 2), Convert.ToInt32(mObj.prevActualYPosition) + (size / 2), 1, 1);

            gObjZAxisBoard.FillRectangle(yellow, zXConstant, Convert.ToInt32(mObj.prevActualPosition.z), SimulatedObject.objectSize, SimulatedObject.objectSize);            
            gObjZAxisBoard.FillRectangle(black, zXConstant, Convert.ToInt32(mObj.actualPosition.z), SimulatedObject.objectSize, SimulatedObject.objectSize);
            //gObjZAxisBoard.FillRectangle(yellow, zXConstant + (size / 2), Convert.ToInt32(mObj.prevActualZPosition), 1, 1);
        }
    }
}

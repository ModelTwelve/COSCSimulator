using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COSCSimulator
{
    public class SimulatorController
    {
        List<SimulatedObject> movingObjects = new List<SimulatedObject>();
        private double simulationDuration;
        private float zXConstant;
        private int ticksPerSecond = 1000;

        Graphics gObjSimulationBoard, gObjZAxisBoard;

        private int size = 4;
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
            this.simulationDuration = Math.Floor(simulationDuration * ticksPerSecond);

            gObjSimulationBoard = simulationBoard.CreateGraphics();
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
            Random random = new Random();            
            switch (formationNumber)
            {
                case 1:
                    movingObjects.Add(new SimulatedObject(random.Next(Int32.MinValue,Int32.MaxValue),startX, startY, startZ, goalX, goalY, goalZ, velocity));
                    break;
                case 2:
                    movingObjects.Add(new SimulatedObject(random.Next(Int32.MinValue, Int32.MaxValue),startX, startY, startZ, goalX, goalY, goalZ, velocity));
                    movingObjects.Add(new SimulatedObject(random.Next(Int32.MinValue, Int32.MaxValue),startX - 40, startY, startZ, goalX - 40, goalY, goalZ, velocity));
                    break;
                case 3:
                    movingObjects.Add(new SimulatedObject(random.Next(Int32.MinValue, Int32.MaxValue),startX, startY, startZ, goalX, goalY, goalZ, velocity));
                    movingObjects.Add(new SimulatedObject(random.Next(Int32.MinValue, Int32.MaxValue),startX - 40, startY, startZ, goalX - 40, goalY, goalZ, velocity));
                    movingObjects.Add(new SimulatedObject(random.Next(Int32.MinValue, Int32.MaxValue),startX + 40, startY, startZ, goalX + 40, goalY, goalZ, velocity));
                    break;
            }

            // Redraw goals
            foreach (var mObj in movingObjects)
            {
                gObjSimulationBoard.FillRectangle(red, Convert.ToSingle(mObj.targetXPosition), Convert.ToSingle(mObj.targetYPosition), size, size);
                gObjZAxisBoard.FillRectangle(red, zXConstant, Convert.ToSingle(mObj.targetZPosition), size, size);
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
                if (steps % 100 == 0)
                {
                    Task.Delay(delay).Wait();
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
            gObjSimulationBoard.FillRectangle(yellow, Convert.ToInt32(mObj.prevActualXPosition), Convert.ToInt32(mObj.prevActualYPosition), size, size);            
            gObjSimulationBoard.FillRectangle(black, Convert.ToInt32(mObj.actualXPosition), Convert.ToInt32(mObj.actualYPosition), size, size);
            //gObjSimulationBoard.FillRectangle(yellow, Convert.ToInt32(mObj.prevActualXPosition) + (size / 2), Convert.ToInt32(mObj.prevActualYPosition) + (size / 2), 1, 1);

            gObjZAxisBoard.FillRectangle(yellow, zXConstant, Convert.ToInt32(mObj.prevActualZPosition), size, size);            
            gObjZAxisBoard.FillRectangle(black, zXConstant, Convert.ToInt32(mObj.actualZPosition), size, size);
            //gObjZAxisBoard.FillRectangle(yellow, zXConstant + (size / 2), Convert.ToInt32(mObj.prevActualZPosition), 1, 1);
        }
    }
}

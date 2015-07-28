using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ElevatorManagementSystem{
    class Elevator : IElevator{
        // Fields
        static int maxFloor = 10;

        private Timer timer = new Timer(3000);
        private int currentFloor = 1;
        private MoveType moveType;
        private bool doorsAreOpen = false;
        private bool orderedToStop = false;

        // Construtor
        public Elevator(){
            timer.Elapsed += new ElapsedEventHandler(updateFloor);
        }

        // Properties
        public int MaxFloors{
            get { return maxFloor; }
            set { maxFloor = value; }
        }

        public int CurrentFloor{
            get { return currentFloor; }
        }

        public bool Opened{
            get { if (doorsAreOpen) return true;
                  else return false; }
        }

        public int Goto(int Floor){
            if (Floor < 0 || Floor > maxFloor) return -1;
            else if (currentFloor != Floor) {
                if (currentFloor < Floor)
                    moveType = MoveType.Up;
                else
                    moveType = MoveType.Down;
                    
                timer.Start();
                while (currentFloor != Floor && !orderedToStop){}
                timer.Stop();
            }

            moveType = MoveType.None;
            orderedToStop = false;
            return currentFloor;
        }

        public void updateFloor(object sender, ElapsedEventArgs e){
            switch(moveType){
                case MoveType.Up:       currentFloor++; break;
                case MoveType.Down:     currentFloor--; break;
            }
        }

        public bool Open(){
            doorsAreOpen = true;
            return true;
        }

        public bool Close(){
            doorsAreOpen = false;
            return true;
        }

        public void Alarm() { }
        
        public int Stop(){
            orderedToStop = true;
            return currentFloor;
        }
    }
}
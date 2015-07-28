using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorManagementSystem{
    public class ElevatorLibrary{
        // Fields
        Dictionary<string, IElevator> elevatorList;

        public ElevatorLibrary() {
            elevatorList = new Dictionary<string, IElevator>();
        }

        // Create elevator
        public void CreateElevator(string name){
            elevatorList.Add(name, new Elevator());
        }

        public IElevator GetElevator(string name){
            return elevatorList[name];
        }
    }
}

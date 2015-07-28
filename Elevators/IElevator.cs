using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorManagementSystem{
        /// <summary>
        /// A Interface contract for representing elevator commands.
        /// </summary>
        public interface IElevator{
                /// <summary>
                /// Property that gets and sets the Maximum Floors in a building.
                /// </summary>
                int MaxFloors{ get; set; }

                int CurrentFloor { get; }
                bool Opened { get; }

                /// <summary>
                /// Move elevator to a floor in the building.
                /// </summary>
                /// <param name="Floor">The floor number too travel to.</param>
                /// <returns>The floor that the elevator has stopped at, else -1 for an error.</returns>
                int Goto(int Floor);

                /// <summary>
                /// Opens the elevator doors.
                /// Hint: can doors open when elevator is moving?
                /// </summary>
                /// <returns>True if doors have opened, else False</returns>
                bool Open();

                /// <summary>
                /// Closes the elevator doors.
                /// </summary>
                /// <returns>True if doors have closed, else False</returns>
                bool Close();

                /// <summary>
                /// Sound the ALARM!
                /// </summary>
                void Alarm();

                /// <summary>
                /// Stops the elevator.
                /// </summary>
                /// <returns>Returns the floor the levator has sotpped, else -1 for an error.</returns>
                int Stop();
        }
}

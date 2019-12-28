using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tms.Som;

namespace tms
{
    public class CVehicle
    {
        public string VehicleName;
        public string Direction;
        public StatusEnum Status;

        public CVehicle()
        {
            VehicleName = GetVehicleName();
            Direction = GetDirection();
            Status = StatusEnum.Start;

        }

        public string GetVehicleName()
        {
            Console.WriteLine("Enter Vehicle Name [Car, Bus etc.] : ");
            string name = Console.ReadLine();
            return name;
        }

        public string GetDirection()
        {
            Console.WriteLine("Direction it is coming from East, West, North, South : ");
            string d =  Console.ReadLine();
            if (d.ToLower() == "east") return "east";
            else if (d.ToLower() == "west") return "west";
            else if (d.ToLower() == "north") return "north";
            else if (d.ToLower() == "south") return "south";
            else Console.WriteLine("Wrong Input, Default Direction ot EAST"); return "south";
        }

        public void SetStatus(StatusEnum s)
        {
            if (s == StatusEnum.Start) Status = StatusEnum.Start;
            else if (s == StatusEnum.Start) Status = StatusEnum.Stop;
            else Console.WriteLine("Wrong Status, Default status to START"); Status = StatusEnum.Start;
        }

    }
}

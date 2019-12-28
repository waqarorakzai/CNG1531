using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tms.Som;

namespace tms
{
    public class CTrafficLight
    {

        public string Sign;
        public int Duration;
        public string Position;

        public CTrafficLight()
        {
            Sign = "Red";
            Duration = 1;
            Position = "tmp";

        }

        public void setPosition(string name)
        {
            if(name.ToLower() == "east")
            {
                Position = "east";
            }
            else if (name.ToLower() == "west")
            {
                Position = "west";
            }
            else if (name.ToLower() == "north")
            {
                Position = "north";
            }
            else
            {
                Position = "south";
            }
        }
    }
}

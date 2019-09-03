using System;
using System.Collections.Generic;

namespace BYOCCore
{
    public  class  Clock : IBusDevice
    {
        private bool halted = false;
        private string deviceID;
        private string name;
        public Clock(string DeviceID, string Name)
        {
            deviceID = DeviceID;
            name = Name;
        }
        public void Clk()
        {
           
        }
        public string DisplayName() { return name; }
       
        public bool IsHalted() { return halted; }

        public List<String> SignalLines()
        {
            var lines = new List<String>();

     
            lines.Add("disable");
           

            return lines;

        }

        public void Enable(string function)
        {
            if (function == "disable")
            {
                halted = true;
            }
            else
            {
                throw new Exception($"Clock does not have a control line function called {function}");
            }
        }

        public string ID()
        {
            return deviceID;
        }

        public bool IsOutputEnabled()
        {
            return false;
        }


     
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYOCCore
{
   public  class Register : IBusDevice
    {

        protected  bool outputEnabled = false;
        protected bool loadEnabled = false;
        protected bool reset = false;
        protected bool inc = false;
        protected bool dec = false;
        public byte Data = 0;
        protected Bus connectedBus;
        protected string deviceName = "";
        protected string deviceID = "";

        public string DisplayName() { return deviceName; }

        public Register(string DeviceName, string DeviceID, Bus ConnectedBus, byte InitialValue = 0)
        {
            deviceName = DeviceName;
            deviceID = DeviceID;
            connectedBus = ConnectedBus;
            Data = InitialValue;
        }

        public string ID() { return deviceID; }

        public void Clk()
        {
            if (loadEnabled)
            {
                Data = connectedBus.Data;
                loadEnabled = false;
            }
            if (outputEnabled)
            {
                connectedBus.Data = Data;
                outputEnabled = false;
            }
            if (reset)
            {
                Data = 0;
                reset = false;
            }
            if (inc)
            {
                Data++;
                inc = false;
            }
            if (dec)
            {
                Data--;
                dec = false;
            }
        }
        public List<String> SignalLines()
        {
            var lines = new List<String>();


            lines.Add("output");
            lines.Add("load");
            lines.Add("reset");
            lines.Add("inc");
            lines.Add("dec");

            return lines;

        }

        public void Enable(string function)
        {
            switch (function)
            {
                case "output":
                    outputEnabled = true;
                    break;
                case "load":
                    loadEnabled = true;
                    break;
                case "reset":
                    reset = true;
                    break;
                case "inc":
                    inc = true;
                    break;
                case "dec":
                    dec = true;
                    break;
                default:
                    throw new Exception("Unable to enable the unknown function: " + function);
            }
        }

      
        public string ToString(int firstColumnPaddedWidth)
        {
            return $"{deviceName}".PadRight(firstColumnPaddedWidth, ' ') + $"= {Data.ToString(connectedBus.NumberFormat)}";
        }
        public string OperationsOnNextClock()
        {
            string next = "";
            if (loadEnabled) next = $"{next}load";
            if (outputEnabled) next = $"{next}output"; ;
            if (reset) next = $"{next}reset"; ;
            if (inc) next = $"{next}inc"; ;
            if (dec) next = $"{next}dec"; ;
            return $"{next}";
            
        }
        public bool IsOutputEnabled()
        {
            return outputEnabled;
        }
    }
}

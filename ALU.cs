using System;
using System.Collections.Generic;
using System.Text;


namespace BYOCCore
{

    
    public class ALU : IBusDevice
    {
        private Register a;
        private Register b;
        private Register sta;
        private bool add;
        private bool sub;
        private bool cmp;
        private Bus bus;
        private string deviceName;
        private string deviceID;


        public ALU(string DeviceName, string DeviceID, Register rega, Register regb, Register regsta, Bus Bus)
        {
            deviceID = DeviceID;
            a = rega;
            b = regb;
            sta = regsta;
            bus = Bus;
            deviceName = DeviceName;
        }

        public string DisplayName() { return deviceName; }
        public string ID() { return deviceID; }

        private string newStatus = "00000000";
        char bit = '1';
        private void setZero()
        {
            StringBuilder sb = new StringBuilder(newStatus);
            sb[7] = bit;
            newStatus = sb.ToString();
        }
        private void setCarry()
        {
            StringBuilder sb = new StringBuilder(newStatus);
            sb[6] = bit;
            newStatus = sb.ToString();
        }
        private void setOverFlow()
        {
            StringBuilder sb = new StringBuilder(newStatus);
            sb[5] = bit;
            newStatus = sb.ToString();
        }
        private void setNegative()
        {
            StringBuilder sb = new StringBuilder(newStatus);
            sb[4] = bit;
            newStatus = sb.ToString();
        }
        private void clearNewStatus()
        {
            newStatus = "00000000";
        }
        private void pushNewStatusToRegister()
        {
            this.sta.Data = Convert.ToByte(newStatus, 2);
        }

        public void Clk()
        {
            if (add)
            {
                clearNewStatus();
                bus.Data = (byte)(a.Data + b.Data);
                if (a.Data + b.Data == 0)
                {
                    setZero();
                }
                if (a.Data + b.Data > 255)
                {
                    setCarry();
                    
                }
                add = false;

                pushNewStatusToRegister();
            }
            if (sub)
            {
                clearNewStatus();
                var res = a.Data - b.Data;
                if (res == 0)
                {
                    setZero();
                }
                
                bus.Data = (byte)res;
                sub = false;

                pushNewStatusToRegister();
            }
            if (cmp)
            {
                clearNewStatus();
                if (a.Data - b.Data == 0)
                {
                    setZero();
                }
                if (a.Data - b.Data < 0)
                {
                    setNegative();
                }
                cmp = false;
                pushNewStatusToRegister();
            }
       }

        

        public string OperationsOnNextClock()
        {
            string next = "";
            if (add) next = $"{next}add";
            if (sub) next = $"{next}sub";
            if (cmp) next = $"{next}cmp";
          
            return $"{next}";

        }

        public List<String> SignalLines()
        {
            var lines = new List<String>();

            lines.Add("add");
            lines.Add("sub");
            lines.Add("cmp");


            return lines;

        }


        public void Enable(string function)
        {
            switch (function)
            {
                case "add":
                    add = true;
                    break;
                case "sub":
                    sub = true;
                    break;
                case "cmp":
                    cmp = true;
                    break;
                default:
                    throw new Exception("Unable to enable the unknown function: " + function);
            }
        }
        public bool IsOutputEnabled()
        {
            return add || sub ;
        }

        public string ToString(int firstColumnPaddedWidth)
        {
            return deviceName.PadRight(firstColumnPaddedWidth,' ');
        }
    }
}

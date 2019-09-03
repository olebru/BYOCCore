using System;


namespace BYOCCore
{
    public class ProgramCounter : Register, IBusDevice
    {
        public ProgramCounter(string DeviceName, string DeviceID, Bus bus) : base(DeviceName,DeviceID,bus)
        {
            
        }

        private bool countEnabled = false;

        public new string OperationsOnNextClock()
        {
            string next = "";
            if (loadEnabled) next = $"{next}load";
            if (outputEnabled) next = $"{next}output";
            if (reset) next = $"{next}reset";
            if (inc) next = $"{next}inc";
            if (dec) next = $"{next}dec";
            if (countEnabled) next = $"{next}count";

            return $"{next}";

        }

        public new void Enable(string function)
        {
            switch (function)
            {
                case "count":
                    countEnabled = true;
                    break;
                default:
                    base.Enable(function);
                    break;
            }

        }

        public new void Clk()
        {
            if (countEnabled)
            {
                increment();
                countEnabled = false;
            }

            base.Clk();
        }

        private void increment()
        {
            if (Data == byte.MaxValue)
            {
                Data = 0;
            }
            else
            {
                Data++;
            }
        }

       
    }
}

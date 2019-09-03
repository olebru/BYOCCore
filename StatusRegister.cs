using System;


namespace BYOCCore
{
   public  class StatusRegister : Register, IBusDevice
    {
        public bool Zero1
        {
            get
            {
                string binaryString = Convert.ToString(base.Data, 2);

                binaryString = binaryString.PadLeft(8, '0');

                return (binaryString[7] == '1');
            }

        }
        public bool Carry2
        {
            get
            {
                var binaryString = Convert.ToString(base.Data, 2);

               binaryString = binaryString.PadLeft(8, '0');

                return (binaryString[6] == '1');
            }

        }
        public bool Overflow4
        {
            get
            {
                var binaryString = Convert.ToString(base.Data, 2);

                binaryString = binaryString.PadLeft(8, '0');

                return (binaryString[5] == '1');
            }

        }
        public bool Negative8
        {
            get
            {
                var binaryString = Convert.ToString(base.Data, 2);

                binaryString = binaryString.PadLeft(8, '0');

                return (binaryString[4] == '1');
            }

        }

        private bool enableDecoderOffset = false;
        public bool EnableDecoderOffset = false;
        public StatusRegister(string DeviceName, string DeviceID, Bus ConnectedBus) : base(DeviceName, DeviceID, ConnectedBus)
        {
        }

        public new void Clk()
        {
            EnableDecoderOffset = enableDecoderOffset;
            base.Clk();
        }

        public new void Enable(string function)
        {
            switch (function)
            {
                case "enabledecoderoffset":
                    enableDecoderOffset = true;
                    break;
                case "disabledecoderoffset":
                    enableDecoderOffset = false;
                    break;
                default:
                    base.Enable(function);
                    break;
            }
        }

       
        public override string ToString()
        {
            return deviceName + " Value = " + Data.ToString(connectedBus.NumberFormat) + Environment.NewLine;
        }

    }
}

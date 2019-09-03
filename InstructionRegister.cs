using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYOCCore
{
   public  class InstructionRegister : Register, IBusDevice
    {
        public string Instruction = "N/A";
       
        private Register regsta;
        
        public InstructionRegister(string DeviceName, string DeviceID, Bus bus,Register StatusRegister) : base(DeviceName, DeviceID, bus)
        {
         
            regsta = StatusRegister;
        }

        public new void Clk()
        {
            
            increment();
            
            base.Clk();
        }

        private void updateInstructionName()
        {
            var inst = base.connectedBus.DecoderROM.FetchInstruction(regsta.Data, base.Data).FirstOrDefault();
            if (inst != null)
            {
                Instruction = inst.Mnemonic;
            }
            else
            {
                Instruction = string.Empty;
            }
           
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

        public new string OperationsOnNextClock()
        {
            string next = "";
            if (loadEnabled) next = $"{next}load";
            if (outputEnabled) next = $"{next}output";
            if (reset) next = $"{next}reset";
            if (inc) next = $"{next}inc";
            if (dec) next = $"{next}dec";
            return $"{next}";

        }

       



        public new string ToString(int firstColumnPaddedWidth)
        {
            
            return $"{base.deviceName} Value".PadRight(firstColumnPaddedWidth, ' ') + $"= {Data.ToString(base.connectedBus.NumberFormat)}";
        }


    }
}

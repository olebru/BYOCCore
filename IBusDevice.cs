using System;
using System.Collections.Generic;


namespace BYOCCore
{
   public interface IBusDevice
    {
   
        void Enable(string function);

        void Clk();

        string ID();

        bool IsOutputEnabled();

        List<string> SignalLines();

        string DisplayName();
     
    }
}

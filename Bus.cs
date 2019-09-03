using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYOCCore
{
    public class Bus
    {
        public string NumberFormat = "X2";
        private double hz= 0;
        private System.Diagnostics.Stopwatch stopwatch;
        public List<IBusDevice> devices;
        public DecoderRom DecoderROM;
      
        public Bus()
        {
            devices = new List<IBusDevice>();
            Data = new byte();
          
            stopwatch = new System.Diagnostics.Stopwatch();
        }

        private byte data;
        private bool dataWrittenInThisClk = false;
        public int Cycles = 0;

        public byte Data
        {
            get
            {
                return data;
            }
            set
            {
                if (dataWrittenInThisClk && Cycles != 0)
                {
              
                    throw new Exception("Puff of blue smoke exception, multiple bus devices has output enabled at the same time.");

                }

                data = value;
                dataWrittenInThisClk = true;
            }
        }

        public void Clk()
        {
            stopwatch.Stop();
            double ms = stopwatch.ElapsedMilliseconds;
            if (ms != 0)
            {
                hz = 1000 / ms;
                Debug.WriteLine(hz);
            }
           
            stopwatch.Reset();
            stopwatch.Start();
            this.Data = 0;
            this.dataWrittenInThisClk = false;
           var outputtingDevice = devices.SingleOrDefault(d => d.IsOutputEnabled());
            if (outputtingDevice != null)
            {
                outputtingDevice.Clk();
            }


            foreach (var busItem in devices)
            {

                if (!busItem.IsOutputEnabled())
                {
                    busItem.Clk();
                }

            }

            dataWrittenInThisClk = false;
            Cycles++;
        }

    }
}

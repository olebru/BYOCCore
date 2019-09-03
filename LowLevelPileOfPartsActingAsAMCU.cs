using System;
using System.Collections.Generic;
using System.Text;

namespace BYOCCore
{
    class LowLevelPileOfPartsActingAsAMCU
    {
        private Bus _bus;
        private DecoderRom _decoderRom;
        private Assembler _assembler;
        private byte[] _programByteCode;

        public LowLevelPileOfPartsActingAsAMCU()
        {

            _decoderRom = new DecoderRom(); //Blank constructor loades examplerom... FIXME... Dirty
            _assembler = new Assembler(_decoderRom);
            _bus = new Bus();
            _bus.NumberFormat = "X2";
            _bus.DecoderROM = _decoderRom;
          
            var rega = new Register("REGA", "rega", _bus);
            var regb = new Register("REGB", "regb", _bus);
            var regc = new Register("REGC", "regc", _bus);
            var regs = new Register("REGSWAP", "regs", _bus);
            var regsp = new Register("REGSP", "regsp", _bus, 255);
            var regsta = new StatusRegister("STATUS", "regsta", _bus);
            var regi = new InstructionRegister("REGINSTR", "regi", _bus, regsta);
            var pc = new ProgramCounter("PCOUNT", "pc", _bus);
            var mem = new RamModule("BASEMEM", "mem", _bus, regsp, pc);
            var mmu = new MMU("MMU", "mmu", _bus);
            var clk = new Clock("clk", "Clock");
            var alu = new ALU("ALU ", "alu", rega, regb, regsta, _bus);


            _bus.devices.Add(regi);
            _bus.devices.Add(pc);
            _bus.devices.Add(regsp);
            _bus.devices.Add(rega);
            _bus.devices.Add(regb);
            _bus.devices.Add(regc);
            _bus.devices.Add(regs);
            _bus.devices.Add(alu);
            _bus.devices.Add(regsta);
            _bus.devices.Add(mem);
            _bus.devices.Add(mmu);
            _bus.devices.Add(clk);

          
            _programByteCode = _assembler.Assemble(); //Paramless uses example src... FIXME... Dirty
            mem.LoadBytes(_programByteCode);
        }


    }
}

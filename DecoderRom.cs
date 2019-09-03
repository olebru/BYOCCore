using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BYOCCore
{
    public class DecoderRom
    {
        private class fileLine
        {
            public int readOrder = 0;
            private int opCode { get { return instructionBaseAddress + mnemonicSeq; } }
            public int mnemonicSeq = 0;
            public string clkFlag = string.Empty;
            public string deviceID = string.Empty;
            public string function = string.Empty;
            public string mnemonic = string.Empty;
            public string negative = string.Empty;
            public string overflow = string.Empty;
            public string carry = string.Empty;
            public string zero = string.Empty;
            public int instructionBaseAddress = 0;
            public int completeOpCode
            {
                get
                {
                    int fioc = 0;

                    fioc = Convert.ToInt32($"{status}{opCodeAsString}", 2);

                    return fioc;
                }
            }

            public string status { get { return $"{negative}{overflow}{carry}{zero}"; } }
            public int statusAsInt { get { return Convert.ToInt32(status, 2); } }
            public string opCodeAsString { get { return Convert.ToString(opCode, 2).PadLeft(8, '0'); } }
            public override string ToString()
            {
                return $"{mnemonic},CO{completeOpCode},O{opCode},B{instructionBaseAddress},{clkFlag},{deviceID},{function},{status}";
            }
        }
        private List<MicroInstruction> completeROM;

        public DecoderRom() : this (Properties.Resources.currentrom){ }
        
        public DecoderRom(string rom)
        {
            var initialListLine = new List<fileLine>();
            var interMediateListLine = new List<fileLine>();
            var finalListLine = new List<fileLine>();

   

            using (var sr = new System.IO.StreamReader(rom))
            {
                int order = -1;
                while (!sr.EndOfStream)
                {
                    order++;

                    var csvLine = sr.ReadLine();
                    var line = new fileLine();
                    var tokens = csvLine.Split('\t');
                    line.readOrder = order;

                    line.clkFlag = tokens[0];
                    line.deviceID = tokens[1];
                    line.function = tokens[2];
                    line.mnemonic = tokens[3];
                    line.negative = tokens[4];
                    line.overflow = tokens[5];
                    line.carry = tokens[6];
                    line.zero = tokens[7];
                    initialListLine.Add(line);
                }
            }

            foreach (var line in initialListLine)
            {
                List<String> possiblities = new List<string>();
                List<String> validBitCombinations = new List<string>();


                for (int i = 0; i < 16; i++)
                {
                    possiblities.Add(Convert.ToString(i, 2).PadLeft(4, '0'));
                }

                foreach (var possibilty in possiblities)
                {
                    var cand = new StringBuilder(string.Empty.PadLeft(4, '0'));
                    for (int i = 0; i < 4; i++)
                    {
                        if (line.status[i] == 'x') cand[i] = possibilty[i];
                        if (line.status[i] != 'x') cand[i] = line.status[i];
                    }
                    if (validBitCombinations.Count(v => v == cand.ToString()) == 0)
                        validBitCombinations.Add(cand.ToString());
                }

                foreach (var validCombination in validBitCombinations)
                {
                    var newLine = new fileLine();
                    newLine.readOrder = line.readOrder;
                    newLine.clkFlag = line.clkFlag;
                    newLine.deviceID = line.deviceID;
                    newLine.function = line.function;
                    newLine.mnemonic = line.mnemonic;
                    newLine.mnemonicSeq = line.mnemonicSeq;
                    newLine.instructionBaseAddress = line.instructionBaseAddress;

                    newLine.negative = validCombination[0].ToString();
                    newLine.overflow = validCombination[1].ToString();
                    newLine.carry = validCombination[2].ToString();
                    newLine.zero = validCombination[3].ToString();
                    interMediateListLine.Add(newLine);

                }

            }


            var listMnemonics = new List<string>();

            foreach (var line in interMediateListLine)
            {
                if (listMnemonics.Count(l => l == line.mnemonic) == 0)
                {
                    listMnemonics.Add(line.mnemonic);
                }
            }
            int addr = 0;
            foreach (var mnemonic in listMnemonics)
            {

                var listOfList = new List<List<fileLine>>();
                for (int i = 0; i < 16; i++)
                {
                    listOfList.Add(new List<fileLine>());
                }

                foreach (var instr in interMediateListLine.Where(l => l.mnemonic == mnemonic).OrderBy(l => l.readOrder))
                {
                    listOfList[instr.statusAsInt].Add(instr);
                }
                int listWithHighestCount = 0;
                int countOfListWithHigestCount = 0;
                foreach (var list in listOfList)
                {
                    if (list.Count(l => l.clkFlag == "p") > countOfListWithHigestCount)
                    {
                        countOfListWithHigestCount = list.Count(l => l.clkFlag == "p");
                        listWithHighestCount = listOfList.IndexOf(list);
                    }
                }

                foreach (var lineItem in interMediateListLine.Where(l => l.mnemonic == mnemonic))
                {
                    lineItem.instructionBaseAddress = addr;
                }
                addr = addr + countOfListWithHigestCount;
            }

            foreach (var mnemonic in listMnemonics)
            {

                var listOfList = new List<List<fileLine>>();
                for (int i = 0; i < 16; i++)
                {
                    listOfList.Add(new List<fileLine>());
                }

                foreach (var instr in interMediateListLine.Where(l => l.mnemonic == mnemonic))
                {
                    listOfList[instr.statusAsInt].Add(instr);
                }


                foreach (var list in listOfList)
                {
                    bool firstFound = false;
                    int offset = 0;
                    foreach (var line in list)
                    {
                       
                        if (line.clkFlag == "p" && firstFound)
                        {
                            offset++;
                                                    }
                        if (line.clkFlag == "p")
                        {
                            firstFound = true;
                        }
                        line.mnemonicSeq += offset;
                        finalListLine.Add(line);
                    }
                }
            }
            completeROM = new List<MicroInstruction>();
            foreach (var line in finalListLine)
            {
                var mc = new MicroInstruction(line.completeOpCode, line.deviceID, line.function, line.mnemonic, false, (line.instructionBaseAddress == line.completeOpCode && line.status == "0000"));
                completeROM.Add(mc);
            }

            if (this.OpCodeAddressSpaceUsedInPercent() > 99 )
            {
                throw new Exception("OpCode AddressSpace is exhausted, optimize...");
            }

        }
      
        public byte FetchByteCodeFromMnemonic(string Mnemonic)
        {
            var b = completeROM.FirstOrDefault(m => m.Mnemonic == Mnemonic).OPCode;

            return (byte)b;

        }

        public double OpCodeAddressSpaceUsedInPercent()
        {
            var opCodesBelow256 = completeROM.Where(o => o.OPCode < 256);

            var lastOpcode = opCodesBelow256.OrderBy(o => o.OPCode).Select(o => o.OPCode).Max();

            return Math.Round(((double)lastOpcode / 256d * 100d), 1);

        }


        public List<MicroInstruction> FetchInstruction(Byte StatusRegisterValue, Byte InstructionRegisterValue)
        {
          
            string strBaseOpCode = Convert.ToString(InstructionRegisterValue, 2).PadLeft(8, '0');
            string status = Convert.ToString(StatusRegisterValue, 2);
            string strfullOpCode = status + strBaseOpCode;
            int fullOpCode = Convert.ToInt32(strfullOpCode, 2);
            return completeROM.Where(m => m.OPCode == fullOpCode).ToList<MicroInstruction>();
        }

        
    }
}

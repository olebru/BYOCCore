using System;
using System.Text;

namespace BYOCCore
{
   public  class MicroInstruction
    {
        public MicroInstruction(int OPCode, string DeviceID, String Function, string Mnemonic, bool IsMCEntryPoint, bool IsASMEntryPoint)
        {
            this.OPCode = OPCode;
            this.DeviceID = DeviceID;
            this.Function = Function;
            this.Mnemonic = Mnemonic;
            this.IsMCEntryPoint = IsMCEntryPoint;
            this.IsASMEntryPoint = IsASMEntryPoint;
        }
        public int OPCode { get; set; }
        public string DeviceID { get; set; }
        public string Function { get; set; }
        public string Mnemonic { get; set; }
        public bool IsASMEntryPoint { get; set; }
        public bool IsMCEntryPoint { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (IsASMEntryPoint)
            {
                sb.Append("---");
                sb.Append(Environment.NewLine);
                sb.Append(Mnemonic);
                sb.Append("\t");
                sb.Append("opcode ");
                sb.Append(OPCode);
                sb.Append(Environment.NewLine);
            }
            sb.Append("\t");
            sb.Append(DeviceID);
            sb.Append("\t");
            sb.Append(Function);
            sb.Append("\t");
            sb.Append(OPCode);
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
        public string ToSingleLineString()
        {

            return Mnemonic + "\t" + OPCode + "\t" + DeviceID + "\t" + Function;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalkieTalkie
{
    class Data
    {
        private Command cmd;

        public Data()
        {
            cmd = Command.Null;
        }
        public Data(byte[] data)
        {
            cmd = (Command)BitConverter.ToInt32(data, 0);
        }
        public byte[] ToByte()
        {
            List<byte> temp = new List<byte>();
            temp.AddRange(BitConverter.GetBytes((int)cmd));
            return temp.ToArray();
        }
        public Command Cmd { get { return cmd; } set { cmd = value; } }
    }
}

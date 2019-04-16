using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTBluetooth
{
    public class NXTPacket
    {
        // FIELDS
        List<NXTAction> actions;

        // CONSTRUCTOR
        public NXTPacket()
        {
            actions = new List<NXTAction>();
        }

        public NXTPacket(List<NXTAction> actions)
        {
            this.actions = actions;
        }

        public NXTPacket(NXTAction[] actions)
        {
            this.actions = new List<NXTAction>();
            foreach (NXTAction a in actions)
            {
                this.actions.Add(a);
            }
        }

        // METHODS
        public bool AddAction(NXTAction a)
        {
            actions.Add(a);

            return true;
        }

        public byte[] GetPacketData()
        {
            string message = "";
            foreach (NXTAction a in actions)
            {
                message += a.ToString();
            }

            return ToNXTPacket(Encoding.ASCII.GetBytes(message));
        }

        // STATIC METHODS
        public static bool IntroduceNXT(NetworkStream s)
        {
            int length = GetLengthFromBytes((byte)s.ReadByte(), (byte)s.ReadByte());
            byte[] buffer = new byte[length];

            s.Read(buffer, 0, length);
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
            s.Flush();

            buffer = ToNXTPacket(Encoding.ASCII.GetBytes("toast\n"));
            s.Write(buffer, 0, buffer.Length);

            return true;
        }

        public static int GetLengthFromBytes(byte b1, byte b0)
        {
            return (int)BitConverter.ToInt16(new byte[] { b1, b0 }, 0);
        }

        public static void GetNTXPacketPrefix(byte[] b, out byte b0, out byte b1)
        {
            b1 = (byte)((short)b.Length >> 8);
            b0 = (byte)((short)b.Length & 255);
        }

        public static byte[] ToNXTPacket(byte[] b)
        {
            byte[] output = new byte[b.Length + 2];
            byte b0, b1;
            GetNTXPacketPrefix(b, out b0, out b1);

            output[0] = b0;
            output[1] = b1;
            b.CopyTo(output, 2);

            return output;
        }
    }
}

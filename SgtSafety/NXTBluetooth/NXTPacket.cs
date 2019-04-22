using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTBluetooth
{
    /*
     * Cette classe n'a plus d'utilité dans l'état car la plupart de ses méthodes utiles
     * sont statiques, l'instanciation est donc inutile.
     * A transformer en classe statique.
     */
    public class NXTPacket
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private NXTAction action;

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public NXTPacket(NXTAction action)
        {
            this.action = action;
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        // Retourne un tableau d'octets sous forme de paquet adapté à la communication avec le NXT
        public byte[] GetPacketData()
        {
            string message = action.ToString();

            return ToNXTPacket(Encoding.ASCII.GetBytes(message));
        }

        // Retourne une autre instance de NXTPacket avec les mêmes valeurs
        public NXTPacket Duplicate()
        {
            NXTPacket n = new NXTPacket(this.action);
            return n;
        }

        // --------------------------------------------------------------------------
        // STATIC METHODS
        // --------------------------------------------------------------------------

        // A l'aide d'un flux, effectue le protocole de présentation pour communiquer
        public static bool IntroduceNXT(NetworkStream s)
        {
            int length = GetLengthFromBytes((byte)s.ReadByte(), (byte)s.ReadByte());
            byte[] buffer = new byte[length];

            s.Read(buffer, 0, length);
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
            s.Flush();

            buffer = ToNXTPacket(Encoding.ASCII.GetBytes("SgtSafety\n")); // Nom que le robot affichera sur l'écran (retour à la ligne obligatoire)
            s.Write(buffer, 0, buffer.Length);

            return true;
        }

        // Convertis 2 octets en int (en passant par un short)
        public static int GetLengthFromBytes(byte b1, byte b0)
        {
            return (int)BitConverter.ToInt16(new byte[] { b1, b0 }, 0);
        }

        // Convertis la longueur length en deux octets
        public static void GetNTXPacketPrefix(short length, out byte b0, out byte b1)
        {
            b1 = (byte)(length >> 8);
            b0 = (byte)(length & 255);
        }

        // Convertis les données d'un paquet classique b en paquet NXT (valeur retournée)
        public static byte[] ToNXTPacket(byte[] b)
        {
            byte[] output = new byte[b.Length + 2];
            byte b0, b1;
            GetNTXPacketPrefix((short)b.Length, out b0, out b1);

            output[0] = b0;
            output[1] = b1;
            b.CopyTo(output, 2);

            return output;
        }
    }
}

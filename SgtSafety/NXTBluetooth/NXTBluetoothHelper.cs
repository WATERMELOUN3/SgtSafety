using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.NXTBluetooth
{
    public class NXTBluetoothHelper
    {
        // FIELDS
        private BluetoothEndPoint localEndpoint;
        private BluetoothClient localClient;
        private BluetoothComponent localComponent;
        private List<BluetoothDeviceInfo> discoveredDevices;

        private byte[] rawBuffer;
        private const int RAWBUFFER_SIZE = 64;

        private EventHandler connectedEvent;
        private EventHandler<DiscoverDevicesEventArgs> deviceFound;
        private EventHandler searchCompleted;

        public EventHandler<NXTPacketReceivedEventArgs> dataReceived;

        // GETTERS & SETTERS
        public BluetoothClient Client { get { return localClient; } }

        // CONSTRUCTOR
        public NXTBluetoothHelper()
        {
            localEndpoint = new BluetoothEndPoint(GetBTMacAddress(), BluetoothService.SerialPort);
            localClient = new BluetoothClient(localEndpoint);
            localComponent = new BluetoothComponent(localClient);
            discoveredDevices = new List<BluetoothDeviceInfo>();
            rawBuffer = new byte[RAWBUFFER_SIZE];
        }

        // METHODS
        public void SearchDevicesAsync(EventHandler<DiscoverDevicesEventArgs> e1, EventHandler e2)
        {
            localComponent.DiscoverDevicesAsync(20, true, true, true, true, null);
            localComponent.DiscoverDevicesProgress += DiscoveryProgress;
            localComponent.DiscoverDevicesComplete += DiscoveryCompleted;

            deviceFound = e1;
            searchCompleted = e2;
        }

        public void StopSearchDevicesAsync()
        {
            localComponent.Dispose();
            searchCompleted.Invoke(this, new EventArgs());
        }

        public bool PairIfNotAlreadyPaired(BluetoothDeviceInfo device)
        {
            BluetoothDeviceInfo[] paired = localClient.DiscoverDevices(255, false, true, false, false);

            bool isPaired = false;
            for (int i = 0; i < paired.Length; i++)
            {
                if (device.Equals(paired[i]))
                {
                    isPaired = true;
                    break;
                }
            }

            if (!isPaired)
            {
                Console.WriteLine("Trying to pair...");
                isPaired = BluetoothSecurity.PairRequest(device.DeviceAddress, "0000");
                if (isPaired)
                {
                    Console.WriteLine("Paired !");
                    return true;
                }

                return false;
            }

            return true;
        }

        // Pour se connecter à un périphérique appairé, si non appairé alors return false
        public bool ConnectToPaired(BluetoothDeviceInfo device, EventHandler e)
        {
            //if (device.Authenticated)
            //{
            connectedEvent = e;
            localClient.SetPin("0000");
            localClient.BeginConnect(device.DeviceAddress, BluetoothService.SerialPort, new AsyncCallback(Connect), device);

            return true;
            //}

            //return false;
        }

        public void DisconnectFromPaired()
        {
            localClient.EndConnect(null);
        }

        public void SendNTXPacket(NXTPacket packet, bool disposePacket = false)
        {
            NetworkStream stream = localClient.GetStream();
            stream.Flush(); // On flush le stream pour que le NXT lise

            byte[] data = packet.GetPacketData();
            stream.Write(data, 0, data.Length);
            stream.Flush(); // On re flush pour pouvoir lire la réponse du NXT
        }

        public void WaitForData(EventHandler<NXTPacketReceivedEventArgs> e)
        {
            this.dataReceived = e;
            Task<int> t = this.localClient.GetStream().ReadAsync(rawBuffer, 0, rawBuffer.Length);
            t.ContinueWith((tt) => DataReceived(tt));
        }

        // EVENTS / ASYNC CALLS
        private void WaitEnded(IAsyncResult res)
        {

        }

        private void DataReceived(Task<int> t)
        {
            localClient.GetStream().Flush(); // Si problème commenter cette ligne (ça marchait sans)
            dataReceived.Invoke(this, new NXTPacketReceivedEventArgs(rawBuffer));
        }

        private void Connect(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                NXTPacket.IntroduceNXT(localClient.GetStream());

                connectedEvent.Invoke(this, new EventArgs());
            }
        }

        private void DiscoveryProgress(Object sender, DiscoverDevicesEventArgs e)
        {
            foreach (BluetoothDeviceInfo b in e.Devices)
            {
                discoveredDevices.Add(b);
            }

            deviceFound.Invoke(this, e);
        }
          
        private void DiscoveryCompleted(Object sender, DiscoverDevicesEventArgs e)
        {
            searchCompleted.Invoke(this, new EventArgs());
        }

        // STATIC METHODS
        public static BluetoothAddress GetBTMacAddress()
        {
            BluetoothRadio myRadio = BluetoothRadio.PrimaryRadio;
            if (myRadio == null)
            {
                Console.WriteLine("Pas de matériel Bluetooth, ou logiciel Bluetooth non supporté.");
                return null;
            }

            return myRadio.LocalAddress;
        }
    }
}

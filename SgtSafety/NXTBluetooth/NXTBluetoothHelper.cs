using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // CONSTRUCTOR
        public NXTBluetoothHelper()
        {
            localEndpoint = new BluetoothEndPoint(GetBTMacAddress(), BluetoothService.SerialPort);
            localClient = new BluetoothClient(localEndpoint);
            localComponent = new BluetoothComponent(localClient);
        }

        // METHODS
        public void SearchDevicesAsync(EventHandler<DiscoverDevicesEventArgs> e, EventHandler<DiscoverDevicesEventArgs> e2)
        {
            localComponent.DiscoverDevicesAsync(255, true, true, true, true, null);
            localComponent.DiscoverDevicesProgress += e;
            localComponent.DiscoverDevicesComplete += e2;
        }

        public async Task<bool> Send(BluetoothDeviceInfo device, string content)
        {
            // Pour éviter de bloquer l'interface, on exécute cette portion sur un autre thread
            var task = Task.Run(() =>
            {
                try
                {
                    var ep = new BluetoothEndPoint(device.DeviceAddress, BluetoothService.SerialPort);

                    // Connexion
                    localClient.Connect(ep);

                    // On obtient le flux bluetooth (pour écrire dedans)
                    var bluetoothStream = localClient.GetStream();

                    // Si ok on envoie
                    if (localClient.Connected && bluetoothStream != null)
                    {
                        // Ecriture des données dans le flux
                        var buffer = Encoding.UTF8.GetBytes(content);
                        bluetoothStream.Write(buffer, 0, buffer.Length);
                        bluetoothStream.Flush();
                        bluetoothStream.Close();
                        return true;
                    }

                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur: " + e.Message);
                }

                return false;
            });
            return await task;
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

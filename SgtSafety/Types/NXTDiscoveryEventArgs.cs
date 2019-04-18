using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgtSafety.Types
{
    public class NXTDiscoveryEventArgs : EventArgs
    {
        private readonly BluetoothDeviceInfo[] _devices;

        public NXTDiscoveryEventArgs(BluetoothDeviceInfo[] devices)
        {
            this._devices = devices;
        }

        public BluetoothDeviceInfo[] Devices
        {
            get { return _devices; }
        }
    }
}
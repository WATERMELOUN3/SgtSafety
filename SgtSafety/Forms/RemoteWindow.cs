using SgtSafety.NXTBluetooth;
using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SgtSafety.Forms
{
    public partial class RemoteWindow : Form
    {
        NXTBluetoothHelper nxtHelper;
        NXTPacket pStraight, pLeft, pRight, pUturn;

        private void Button3_Click(object sender, EventArgs e)
        {
            nxtHelper.SendNTXPacket(pLeft);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            nxtHelper.SendNTXPacket(pRight);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            nxtHelper.SendNTXPacket(pUturn);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            nxtHelper.SendNTXPacket(pStraight);
        }

        public RemoteWindow(NXTBluetoothHelper nxtHelper)
        {
            InitializeComponent();
            this.nxtHelper = nxtHelper;

            pStraight = new NXTPacket(new NXTAction[] { new NXTAction(NXTMovement.STRAIGHT) });
            pLeft = new NXTPacket(new NXTAction[] { new NXTAction(NXTMovement.INTER_LEFT) });
            pRight = new NXTPacket(new NXTAction[] { new NXTAction(NXTMovement.INTER_RIGHT) });
            pUturn = new NXTPacket(new NXTAction[] { new NXTAction(NXTMovement.UTURN) });
        }
    }
}

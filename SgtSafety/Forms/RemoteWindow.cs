using SgtSafety.NXTBluetooth;
using SgtSafety.NXTEnvironment;
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
        NXTAction pStraight, pLeft, pRight, pUturn;
        NXTVehicule vehicule;

        private void Button3_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pLeft);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pRight);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pUturn);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pStraight);
        }

        public RemoteWindow(NXTVehicule vehicule)
        {
            InitializeComponent();
            this.vehicule = vehicule;

            pStraight = new NXTAction(NXTMovement.STRAIGHT);
            pLeft = new NXTAction(NXTMovement.INTER_LEFT);
            pRight = new NXTAction(NXTMovement.INTER_RIGHT);
            pUturn = new NXTAction(NXTMovement.UTURN);
        }
    }
}

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
        // FIELDS
        private NXTAction pStraight, pLeft, pRight, pUturn;
        private NXTVehicule vehicule;
        private delegate void SafeCallDelegate(NXTBuffer buffer);
        private delegate void Button10DisableDelegate();
        private System.Timers.Timer aTimer;

        // CONSTRUCTOR
        public RemoteWindow(NXTVehicule vehicule, System.Timers.Timer aTimer)
        {
            InitializeComponent();
            this.vehicule = vehicule;
            this.aTimer = aTimer;

            pStraight = new NXTAction(NXTMovement.STRAIGHT);
            pLeft = new NXTAction(NXTMovement.INTER_LEFT);
            pRight = new NXTAction(NXTMovement.INTER_RIGHT);
            pUturn = new NXTAction(NXTMovement.UTURN);
        }

        // METHODS
        private void Button10Disable()
        {
            if (this.button10.InvokeRequired)
            {
                var d = new Button10DisableDelegate(Button10Disable);
                this.Invoke(d);
            }
            else
            {
                button10.Text = "Lancer";
            }
        }

        public void UpdateBuffer(NXTBuffer buffer)
        {
            if (this.listBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateBuffer);
                this.Invoke(d, new object[] { buffer });
            }
            else
            {
                listBox1.Items.Clear();
                foreach (NXTAction a in buffer)
                {
                    listBox1.Items.Add(a.ToFancyString());
                }
            }
        }

        // EVENTS / ASYNC CALLS
        private void Button10_Click(object sender, EventArgs e)
        {
            UpdateBuffer(vehicule.Buffer);
            if (button10.Text == "Pause")
            {
                aTimer.Enabled = false;
                button10.Text = "Lancer";
            }
            else
            {
                //aTimer.Enabled = true;

                vehicule.SendNextAction();
                vehicule.NxtHelper.WaitForData(new EventHandler<NXTPacketReceivedEventArgs>(PacketReceived));
                button10.Text = "Pause";
            }
            
        }

        private void PacketReceived(object sender, NXTPacketReceivedEventArgs e)
        {
            UpdateBuffer(vehicule.Buffer);
            Console.WriteLine("Réponse reçue !");
            if (button10.Text == "Pause")
            {
                if (!vehicule.SendNextAction())
                {
                    Button10Disable();
                }
                else
                {
                    vehicule.NxtHelper.WaitForData(new EventHandler<NXTPacketReceivedEventArgs>(PacketReceived));
                }
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                vehicule.Buffer.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            else
            {
                MessageBox.Show("Veuillez selectionner un élément pour le supprimer.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pLeft);
            listBox1.Items.Add(pLeft.ToFancyString());
        }

        // Bouton "take"
        private void Button5_Click(object sender, EventArgs e)
        {
            if (!vehicule.Buffer.isEmpty())
            {
                NXTAction a = vehicule.Buffer.Last().Duplicate();
                a.SetAction(NXTAction.TAKE);

                vehicule.Buffer.RemoveAt(vehicule.Buffer.Count - 1);
                vehicule.addToBuffer(a);
            }

            UpdateBuffer(vehicule.Buffer);
        }

        // Bouton "drop"
        private void Button6_Click(object sender, EventArgs e)
        {
            if (!vehicule.Buffer.isEmpty())
            {
                NXTAction a = vehicule.Buffer.Last().Duplicate();
                a.SetAction(NXTAction.TAKE);

                vehicule.Buffer.RemoveAt(vehicule.Buffer.Count - 1);
                vehicule.addToBuffer(a);
            }

            UpdateBuffer(vehicule.Buffer);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            vehicule.Buffer.Clear();
            UpdateBuffer(vehicule.Buffer);
        }

        private void RemoteWindow_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Enter))
                button10.PerformClick();
            else if (keyData == Keys.Delete)
                button7.PerformClick();
            else if (keyData == (Keys.Control | Keys.Delete))
                button8.PerformClick();
            else if (keyData == Keys.Up)
                button1.PerformClick();
            else if (keyData == Keys.Down)
                button4.PerformClick();
            else if (keyData == Keys.Left)
                button3.PerformClick();
            else if (keyData == Keys.Right)
                button2.PerformClick();
            else if (keyData == Keys.T)
                button5.PerformClick();
            else if (keyData == Keys.D)
                button6.PerformClick();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pRight);
            listBox1.Items.Add(pRight.ToFancyString());
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pUturn);
            listBox1.Items.Add(pUturn.ToFancyString());
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pStraight);
            listBox1.Items.Add(pStraight.ToFancyString());
        }
    }
}

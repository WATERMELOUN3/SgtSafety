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
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private NXTAction pStraight, pLeft, pRight, pUturn;
        private NXTVehicule vehicule;
        private delegate void SafeCallDelegate(NXTBuffer buffer);
        private delegate void Button10DisableDelegate();

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public RemoteWindow(NXTVehicule vehicule)
        {
            InitializeComponent();
            this.vehicule = vehicule;

            pStraight = new NXTAction(NXTMovement.STRAIGHT);
            pLeft = new NXTAction(NXTMovement.INTER_LEFT);
            pRight = new NXTAction(NXTMovement.INTER_RIGHT);
            pUturn = new NXTAction(NXTMovement.UTURN);
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------

        // Méthode pour désactiver le bouton 10 ("Lancer"/"Pause") sans problèmes d'inter threading
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

        // Met à jour la listBox1 pour afficher le buffer
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

        // --------------------------------------------------------------------------
        // EVENTS / ASYNC CALLS
        // --------------------------------------------------------------------------

        // Bouton "Lancer"/"Pause"
        private void Button10_Click(object sender, EventArgs e)
        {
            UpdateBuffer(vehicule.Buffer);
            if (button10.Text == "Pause")
            {
                button10.Text = "Lancer";
            }
            else
            {
                vehicule.SendNextAction();
                vehicule.NxtHelper.WaitForData(new EventHandler<NXTPacketReceivedEventArgs>(PacketReceived));
                button10.Text = "Pause";
            }
            
        }

        // Paquet reçu
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

        // Bouton "Supprimer action"
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

        // Bouton "Gauche"
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
                a.Action = NXTAction.TAKE;

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
                a.Action = NXTAction.DROP;

                vehicule.Buffer.RemoveAt(vehicule.Buffer.Count - 1);
                vehicule.addToBuffer(a);
            }

            UpdateBuffer(vehicule.Buffer);
        }

        // Bouton "supprimer tout"
        private void Button8_Click(object sender, EventArgs e)
        {
            vehicule.Buffer.Clear();
            UpdateBuffer(vehicule.Buffer);
        }

        // Chargement de la fenêtre
        private void RemoteWindow_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        // Appelé lorsqu'un touche du clavier est pressée, sert pour les raccourcis
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

        // Bouton "droite"
        private void Button2_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pRight);
            listBox1.Items.Add(pRight.ToFancyString());
        }

        // Bouton "demi-tour"
        private void Button4_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pUturn);
            listBox1.Items.Add(pUturn.ToFancyString());
        }

        // Bouton "avancer"
        private void Button1_Click(object sender, EventArgs e)
        {
            vehicule.addToBuffer(pStraight);
            listBox1.Items.Add(pStraight.ToFancyString());
        }
    }
}

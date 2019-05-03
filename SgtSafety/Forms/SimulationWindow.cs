using SgtSafety.Forms.Render;
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
    public partial class SimulationWindow : Form
    {
        private NXTVehicule vehicule;

        private delegate void SafeCallDelegate(NXTBuffer buffer);
        private delegate void ButtonDisableDelegate();

        public SimulationWindow(NXTVehicule vehicule)
        {
            InitializeComponent();

            this.vehicule = vehicule;
            this.simulation1 = new Simulation(vehicule);
            this.simulation1.Location = new System.Drawing.Point(12, 27);
            this.simulation1.Name = "drawEditor1";
            this.simulation1.Size = new System.Drawing.Size(731, 411);
            this.simulation1.TabIndex = 0;
            this.simulation1.Text = "drawEditor1";
            this.Controls.Add(this.simulation1);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.simulation1.CalculatePath();
            UpdateBuffer(this.vehicule.Buffer);
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

        // Paquet reçu
        private void PacketReceived(object sender, NXTPacketReceivedEventArgs e)
        {
            UpdateBuffer(vehicule.Buffer);
            Console.WriteLine("Réponse reçue ! (Autonome)");
            if (button1.Text == "Pause")
            {
                if (!vehicule.SendNextAction())
                {
                    ButtonDisable();
                }
                else
                {
                    vehicule.NxtHelper.WaitForData(new EventHandler<NXTPacketReceivedEventArgs>(PacketReceived));
                }
            }
        }

        // Méthode pour désactiver le bouton 10 ("Lancer"/"Pause") sans problèmes d'inter threading
        private void ButtonDisable()
        {
            if (this.button1.InvokeRequired)
            {
                var d = new ButtonDisableDelegate(ButtonDisable);
                this.Invoke(d);
            }
            else
            {
                button1.Text = "Lancer";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            UpdateBuffer(vehicule.Buffer);
            if (button1.Text == "Pause")
            {
                button1.Text = "Lancer";
            }
            else
            {
                vehicule.SendNextAction();
                vehicule.NxtHelper.WaitForData(new EventHandler<NXTPacketReceivedEventArgs>(PacketReceived));
                button1.Text = "Pause";
            }
        }

        private void SimulationWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.simulation1.Dispose();
        }
    }
}

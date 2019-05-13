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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SgtSafety.Forms
{
    public partial class SimulationWindow : Form
    {
        private NXTVehicule iaVehicule;
        private NXTVehicule remoteVehicule;

        private delegate void SafeCallDelegate(NXTBuffer buffer);
        private delegate void ButtonDisableDelegate();

        public bool RemoteEnabled = false;
        public bool CoopEnabled = false;

        public SimulationWindow(NXTVehicule iaVehicule, NXTVehicule remoteVehicule)
        {
            InitializeComponent();

            this.iaVehicule = iaVehicule;
            this.remoteVehicule = remoteVehicule;

            this.simulation1 = new Simulation(iaVehicule, remoteVehicule, this);
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
            UpdateBuffer(this.iaVehicule.Buffer);
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
        private async void PacketReceived(object sender, NXTPacketReceivedEventArgs e)
        {
            UpdateBuffer(iaVehicule.Buffer);
            Console.WriteLine("Réponse reçue ! (Autonome)");
            if (button1.Text == "Pause")
            {
                if (!iaVehicule.SendNextAction(radioButton1.Checked))
                {
                    await Task.Delay(TimeSpan.FromMilliseconds((int)numericUpDown1.Value));
                    if (simulation1.CalculatePath())
                    {
                        UpdateBuffer(iaVehicule.Buffer);
                        if (radioButton1.Checked)
                            await Task.Delay(TimeSpan.FromMilliseconds((int)numericUpDown1.Value));
                        PacketReceived(sender, e);
                    }
                    else
                    {
                        iaVehicule.Circuit.FillColor(Microsoft.Xna.Framework.Color.White);
                        ButtonDisable();
                    }
                }
                else
                {
                    if (radioButton1.Checked)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds((int)numericUpDown1.Value));
                        PacketReceived(sender, e);
                    }
                    else
                        iaVehicule.NxtHelper.WaitForData(new EventHandler<NXTPacketReceivedEventArgs>(PacketReceived));
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
            UpdateBuffer(iaVehicule.Buffer);
            if (button1.Text == "Pause")
            {
                button1.Text = "Lancer";
            }
            else
            {
                if (simulation1.CalculatePath())
                {
                    UpdateBuffer(iaVehicule.Buffer);
                    button1.Text = "Pause";
                    iaVehicule.SendNextAction(radioButton1.Checked);
                    if (radioButton1.Checked)
                        PacketReceived(sender, new NXTPacketReceivedEventArgs(new byte[] { }));
                    else
                        iaVehicule.NxtHelper.WaitForData(new EventHandler<NXTPacketReceivedEventArgs>(PacketReceived));
                }
            }
        }

        private void SimulationWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.simulation1.Dispose();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Enabled = true;
                RemoteEnabled = true;
            }
            else
            {
                checkBox2.Checked = false;
                checkBox2.Enabled = false;
                RemoteEnabled = false;
                CoopEnabled = false;
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                groupBox1.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
            }
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                CoopEnabled = true;
            }
            else
            {
                CoopEnabled = false;
            }
        }
    }
}

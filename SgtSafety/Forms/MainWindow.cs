using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using Microsoft.Xna.Framework;
using SgtSafety.NXTBluetooth;
using SgtSafety.NXTEnvironment;
using SgtSafety.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SgtSafety.Forms
{
    public partial class MainWindow : Form
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private NXTVehicule remoteVehicule;
        private NXTVehicule autoVehicule;
        private delegate void SafeCallDelegate(object sender, EventArgs e);
        private RemoteWindow remoteWindow;

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------------
        // METHODS
        // --------------------------------------------------------------------------


        // --------------------------------------------------------------------------
        // EVENTS / ASYNC CALLS
        // --------------------------------------------------------------------------

        // Chargement de la fenêtre
        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.remoteVehicule = new NXTVehicule(new Point(0, 2), NXTVehicule.TOP);
            this.autoVehicule = new NXTVehicule(new Point(0, 3), NXTVehicule.TOP);
        }

        // Bouton "Recherche"
        private void Button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Rechercher")
            {
                listBox1.Items.Clear();
                progressBar1.Style = ProgressBarStyle.Marquee;
                button1.Text = "Arreter";
                button2.Enabled = false;
                label1.Text = "Recherche Bluetooth en cours...";

                remoteVehicule.NxtHelper.SearchDevicesAsync(new EventHandler<DiscoverDevicesEventArgs>(DiscoverProgress), new EventHandler(DiscoverCompleted));
            }
            else
            {
                remoteVehicule.NxtHelper.StopSearchDevicesAsync();
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Value = 100;
                button2.Enabled = true;
                button1.Text = "Rechercher";
            }

        }

        // Bouton "Se connecter télécommandé"
        private void Button2_Click(object sender, EventArgs e)
        {
            if (button2.ForeColor == System.Drawing.Color.Black)
            {
                BluetoothDeviceInfo device = (listBox1.SelectedItem as NXTDevice).DeviceInfo;
                remoteVehicule.NxtHelper.PairIfNotAlreadyPaired(device);
                remoteVehicule.NxtHelper.ConnectToPaired(device, new EventHandler(ConnectedRemote));
            }
            else if (button2.ForeColor == System.Drawing.Color.Green)
            {
                remoteVehicule.NxtHelper.DisconnectFromPaired();
                button2.ForeColor = System.Drawing.Color.Black;
            }
        }

        // Evenement "connecté", s'execute lorsque la connection bluetooth est établie
        private void ConnectedRemote(object sender, EventArgs e)
        {
            if (this.button6.InvokeRequired)
            {
                var d = new SafeCallDelegate(ConnectedRemote);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                button2.ForeColor = System.Drawing.Color.Green;
                button6.Enabled = true;
            }
        }

        private void ConnectedAuto(object sender, EventArgs e)
        {
            if (this.button4.InvokeRequired)
            {
                var d = new SafeCallDelegate(ConnectedAuto);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                button3.ForeColor = System.Drawing.Color.Green;
                button4.Enabled = true;
            }
        }

        // Evenement "progression de la découverte", est appelé lorsque des périphériques bluetooth sont découverts
        private void DiscoverProgress(object sender, DiscoverDevicesEventArgs e)
        {
            foreach (BluetoothDeviceInfo b in e.Devices)
            {
                NXTDevice device = new NXTDevice(b);
                listBox1.Items.Add(device);
            }
        }

        // Evenement "découverte terminée", est appelé lorsque la découverte de périphériques bluetooth est terminée
        private void DiscoverCompleted(object sender, EventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = 100;
            button1.Enabled = true;
            label1.Text = "Recherche terminée.";

            // Si il y a un périphérique au moins, on active les boutons "Se connecter"
            if (listBox1.Items.Count > 0)
                button2.Enabled = true;
        }

        // Bouton "télécommande"
        private void Button6_Click(object sender, EventArgs e)
        {
            remoteWindow = new RemoteWindow(this.remoteVehicule);
            remoteWindow.Show();
        }

        // Bouton "éditeur de circuit"
        private void Button5_Click(object sender, EventArgs e)
        {
            EditorWindow ew = new EditorWindow();
            ew.ShowDialog();

            remoteVehicule.Circuit = ew.Circuit;
            autoVehicule.Circuit = ew.Circuit;
        }

        // Bouton "ouvrir le simulateur / affichage"
        private void Button4_Click(object sender, EventArgs e)
        {
            SimulationWindow sw = new SimulationWindow(autoVehicule);
            sw.Show();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (button3.ForeColor == System.Drawing.Color.Black)
            {
                BluetoothDeviceInfo device = (listBox1.SelectedItem as NXTDevice).DeviceInfo;
                autoVehicule.NxtHelper.PairIfNotAlreadyPaired(device);
                autoVehicule.NxtHelper.ConnectToPaired(device, new EventHandler(ConnectedAuto));
            }
            else if (button3.ForeColor == System.Drawing.Color.Green)
            {
                autoVehicule.NxtHelper.DisconnectFromPaired();
                button3.ForeColor = System.Drawing.Color.Black;
            }
        }
    }
}

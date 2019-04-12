using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using SgtSafety.NXTBluetooth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SgtSafety
{
    public partial class MainWindow : Form
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private NXTBluetoothHelper nxtHelper;

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------------
        // FORM METHODS
        // --------------------------------------------------------------------------
        private void MainWindow_Load(object sender, EventArgs e)
        {
            nxtHelper = new NXTBluetoothHelper();
        }

        // Bouton "Recherche"
        private void Button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            progressBar1.Style = ProgressBarStyle.Marquee;
            button1.Enabled = false;
            label1.Text = "Recherche Bluetooth en cours...";

            nxtHelper.SearchDevicesAsync(new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesProgress),
                new EventHandler<DiscoverDevicesEventArgs>(component_DiscoverDevicesComplete));
        }

        // Bouton "Se connecter télécommandé"
        private void Button2_Click(object sender, EventArgs e)
        {

        }

        // --------------------------------------------------------------------------
        // EVENTS / ASYNC CALLS
        // --------------------------------------------------------------------------
        private void component_DiscoverDevicesProgress(object sender, DiscoverDevicesEventArgs e)
        {
            listBox1.Items.Clear();
            foreach(BluetoothDeviceInfo b in e.Devices)
            {
                listBox1.Items.Add(b);
            }
        }

        private void component_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = 100;
            button1.Enabled = true;
            label1.Text = "Recherche terminée.";

            // Si il y a un périphérique au moins, on active les boutons "Se connecter"
            if (listBox1.Items.Count > 0)
                button2.Enabled = true;
        }
    }
}

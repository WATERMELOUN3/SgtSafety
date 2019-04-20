﻿using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
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
using System.Timers;
using System.Windows.Forms;

namespace SgtSafety.Forms
{
    public partial class MainWindow : Form
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private NXTVehicule vehicule;
        private delegate void SafeCallDelegate(object sender, EventArgs e);
        private System.Timers.Timer aTimer;
        private RemoteWindow remoteWindow;


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
            this.vehicule = new NXTVehicule();

            aTimer = new System.Timers.Timer(5000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
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

                vehicule.NxtHelper.SearchDevicesAsync(new EventHandler<DiscoverDevicesEventArgs>(DiscoverProgress), new EventHandler(DiscoverCompleted));
            }
            else
            {
                vehicule.NxtHelper.StopSearchDevicesAsync();
                progressBar1.Style = ProgressBarStyle.Blocks;
                progressBar1.Value = 100;
                button2.Enabled = true;
                button1.Text = "Rechercher";
            }

        }

        // Bouton "Se connecter télécommandé"
        private void Button2_Click(object sender, EventArgs e)
        {
            if (button2.ForeColor == Color.Black)
            {
                BluetoothDeviceInfo device = (listBox1.SelectedItem as NXTDevice).DeviceInfo;
                vehicule.NxtHelper.PairIfNotAlreadyPaired(device);
                vehicule.NxtHelper.ConnectToPaired(device, new EventHandler(Connected));
            }
            else if (button2.ForeColor == Color.Green)
            {
                vehicule.NxtHelper.DisconnectFromPaired();
                button2.ForeColor = Color.Black;
            }
        }

        private void Connected(object sender, EventArgs e)
        {
            if (this.button6.InvokeRequired)
            {
                var d = new SafeCallDelegate(Connected);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                button2.ForeColor = Color.Green;
                button6.Enabled = true;
                aTimer.Enabled = true;
            }
        }

        // --------------------------------------------------------------------------
        // EVENTS / ASYNC CALLS
        // --------------------------------------------------------------------------
        private void DiscoverProgress(object sender, DiscoverDevicesEventArgs e)
        {
            foreach (BluetoothDeviceInfo b in e.Devices)
            {
                NXTDevice device = new NXTDevice(b);
                listBox1.Items.Add(device);
            }
        }

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

        private void Button6_Click(object sender, EventArgs e)
        {
            remoteWindow = new RemoteWindow(this.vehicule, this.aTimer);
            remoteWindow.Show();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            EditorWindow ew = new EditorWindow();
            ew.Show();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //if (vehicule.IsBusy)
            //{
            //    Console.WriteLine("Robot occupé...");
            //    if (nxtHelper.Client.Available > 0)
            //    {
            //        vehicule.IsBusy = false;
            //        nxtHelper.Client.GetStream().Flush();
            //    }
            //    else
            //        return;
            //}

            //Console.WriteLine("Robot disponible !");
            //NXTAction action = vehicule.executeCommand();
            //if (action != null)
            //{
            //    Console.WriteLine("Ordre envoyé: " + action.ToString());
            //    NXTPacket packet = new NXTPacket(action);
            //    nxtHelper.SendNTXPacket(packet);
            //    vehicule.IsBusy = true;
            //    nxtHelper.Client.GetStream().Flush();
            //}

            //if (remoteWindow != null)
            //{
            //    remoteWindow.UpdateBuffer(vehicule.Buffer);
            //}


        }
    }
}

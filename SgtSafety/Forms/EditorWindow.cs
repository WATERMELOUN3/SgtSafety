using SgtSafety.Forms.Render;
using SgtSafety.NXTEnvironment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SgtSafety.Forms
{
    public partial class EditorWindow : Form
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private NXTCircuit circuit;

        // --------------------------------------------------------------------------
        // GETTERS & SETTERS
        // --------------------------------------------------------------------------
        public NXTCircuit Circuit
        {
            get { return circuit; }
        }

        // --------------------------------------------------------------------------
        // CONSTRUCTOR
        // --------------------------------------------------------------------------
        public EditorWindow()
        {
            InitializeComponent();
        }

        // --------------------------------------------------------------------------
        // EVENTS / ASYNC CALLS
        // --------------------------------------------------------------------------
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Éditeur fait par Alexis GÉLIN--ANDRIEU.\nLibrairies utilisées: Monogame, Monogame.Forms.", "A propos...", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void NouveauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewCircuitDialog d = new NewCircuitDialog();
            d.ShowDialog();

            circuit = new NXTCircuit(d.CWidth, d.CHeight);
            circuit.Nom = d.Nom;
            d.Dispose();
        }

        private void EditorWindow_Load(object sender, EventArgs e)
        {
            NewCircuitDialog d = new NewCircuitDialog();
            d.ShowDialog();

            circuit = new NXTCircuit(d.CWidth, d.CHeight);
            circuit.Nom = d.Nom;

            this.drawEditor1 = new DrawEditor(circuit);
            this.drawEditor1.Location = new System.Drawing.Point(12, 27);
            this.drawEditor1.Name = "drawEditor1";
            this.drawEditor1.Size = new System.Drawing.Size(731, 411);
            this.drawEditor1.TabIndex = 0;
            this.drawEditor1.Text = "drawEditor1";
            this.Controls.Add(this.drawEditor1);
        }

        private void EnregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(NXTCircuit));

            if (!Directory.Exists("Circuits"))
                Directory.CreateDirectory("Circuits");
            Stream stream = new FileStream("Circuits\\" + circuit.Nom, FileMode.Create, FileAccess.Write);

            serializer.WriteObject(stream, circuit);
            stream.Close();
        }

        private void OuvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Stream stream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(NXTCircuit));

            NXTCircuit c = (NXTCircuit)ser.ReadObject(reader, true);
            reader.Close();
            stream.Close();

            this.circuit = c;
            this.drawEditor1.InitializeCircuit(this.circuit);
        }

        private void EditorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.drawEditor1.Dispose();
        }
    }
}

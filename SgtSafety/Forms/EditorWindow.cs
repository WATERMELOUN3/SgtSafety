using SgtSafety.Forms.Render;
using SgtSafety.NXTEnvironment;
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
    public partial class EditorWindow : Form
    {
        // --------------------------------------------------------------------------
        // FIELDS
        // --------------------------------------------------------------------------
        private NXTCircuit circuit;

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
    }
}

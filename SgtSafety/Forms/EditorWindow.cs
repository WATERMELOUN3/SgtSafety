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
        private NXTCircuit circuit;

        public EditorWindow()
        {
            InitializeComponent();
        }

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
            d.Dispose();

            drawEditor1.Enabled = true;
        }
    }
}

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
    public partial class NewCircuitDialog : Form
    {
        public string Nom { get; private set; }
        public int CWidth { get; private set; }
        public int CHeight { get; private set; }

        public NewCircuitDialog()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(string.Empty))
                MessageBox.Show("Veuillez entrer un nom pour le circuit", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (numericUpDown1.Value <= 0 || numericUpDown2.Value <= 0)
                MessageBox.Show("Veuillez entrer une valeur supérieure à 0 pour les dimensions", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Nom = textBox1.Text;
                CWidth = (int)numericUpDown1.Value;
                CHeight = (int)numericUpDown1.Value;
                this.Close();
            }
        }
    }
}

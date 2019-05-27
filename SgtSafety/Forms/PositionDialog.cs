using SgtSafety.NXTEnvironment;
using SgtSafety.NXTIA;
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
    public partial class PositionDialog : Form
    {
        public int TelX { get; private set; }
        public int TelY { get; private set; }
        public int AutoX { get; private set; }
        public int AutoY { get; private set; }
        public Microsoft.Xna.Framework.Point TelDir { get; private set; }
        public Microsoft.Xna.Framework.Point AutoDir { get; private set; }

        public PositionDialog(NXTVehicule auto, NXTVehicule tel)
        {
            InitializeComponent();

            numericUpDown1.Value = tel.Position.X;
            numericUpDown2.Value = tel.Position.Y;
            numericUpDown3.Value = auto.Position.X;
            numericUpDown4.Value = auto.Position.Y;
            comboBox1.SelectedIndex = (int)tel.ToOrientation;
            comboBox2.SelectedIndex = (int)auto.ToOrientation;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            TelX = (int)numericUpDown1.Value;
            TelY = (int)numericUpDown2.Value;
            AutoX = (int)numericUpDown3.Value;
            AutoY = (int)numericUpDown4.Value;
            TelDir = IA.OrientationToDirection((NXTEnvironment.Orientation)comboBox1.SelectedIndex);
            AutoDir = IA.OrientationToDirection((NXTEnvironment.Orientation)comboBox2.SelectedIndex);

            this.Close();
        }
    }
}

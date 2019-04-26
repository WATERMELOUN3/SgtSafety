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
    public partial class SimulationWindow : Form
    {
        public SimulationWindow(NXTVehicule vehicule)
        {
            InitializeComponent();

            this.simulation1 = new Simulation(vehicule);
            this.simulation1.Location = new System.Drawing.Point(12, 27);
            this.simulation1.Name = "drawEditor1";
            this.simulation1.Size = new System.Drawing.Size(731, 411);
            this.simulation1.TabIndex = 0;
            this.simulation1.Text = "drawEditor1";
            this.Controls.Add(this.simulation1);
        }
    }
}

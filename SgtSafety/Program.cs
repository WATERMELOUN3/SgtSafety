using SgtSafety.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SgtSafety
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Initialisation et lancement de l'application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }
}

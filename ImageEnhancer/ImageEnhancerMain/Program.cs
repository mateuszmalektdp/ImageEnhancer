using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

// Author: Mateusz Malek
// Silesian University of Technology 2023/24
// Assembly Project v1.0

namespace ImageEnhancerMain
{
    internal static class Program
    {
       
        [STAThread]
        static void Main()
        { 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ImageEnhancerGUI());
        }
    }
}

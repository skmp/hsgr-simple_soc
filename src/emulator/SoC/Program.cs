using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SoC
{
    static class Program
    {
        public static SoC MainWindow = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainWindow = new SoC();
            Application.Run(MainWindow);
        }
    }
}

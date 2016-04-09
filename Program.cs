using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using resub;

namespace resubS
{
    static class Program
    {
        static void OnExit(object o, EventArgs e)
        {
            Config.save();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Load configuration
            Config.load();

            Application.ApplicationExit += OnExit;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

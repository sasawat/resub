using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using resub;

namespace resubS
{
    static class Program
    {
        static void OnExit(object o, EventArgs e)
        {
            Config.save();
            if (File.Exists("resubMaster.wav")) File.Delete("resubMaster.wav");
            if (File.Exists(AudioChunkCollection.prefix)) File.Delete(AudioChunkCollection.prefix);
            if (File.Exists("resub.ass")) File.Delete("resub.ass");
            if (File.Exists("resub.aud")) File.Delete("resub.aud");
            if (File.Exists("resubbed.ass")) File.Delete("resubbed.ass");
            if (File.Exists("temp.mkv")) File.Delete("temp.mkv");
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

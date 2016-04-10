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
        public static Core ResubCore;

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
        static void Main(string[] args)
        {
            ResubCore = new Core();

            //Load configuration
            Config.load();

            Application.ApplicationExit += OnExit;

            //Compile with Properties->Output Type->Console Application 
            //to enable optional Command Line interface
            if(args.Length != 0)
            {
                ResubCore.printlnfunc = Console.WriteLine;
                if(args.Length == 2) ResubCore.run(args[0], args[1], Config.Dictlist, false);
                if (args.Length == 1) ResubCore.run(args[0], "", Config.Dictlist, false);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}

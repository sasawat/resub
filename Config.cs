using System;
using System.IO;
using System.Text;

namespace resub
{
    internal static class Config
    {
        public static void load()
        {
            //Read the config file
            StreamReader sr = new StreamReader(File.OpenRead("resub.conf"));
            string config = sr.ReadToEnd();
            sr.Close();
            string[] lines = config.Split('\n');
            foreach (string line in lines)
            {
                //check for blank line or comment
                if (line.Length == 0 || line[0] == '#') continue;
                string[] parts = line.Split('=');
                if (parts.Length != 2)
                {
                    Console.WriteLine("Malformed configuration line: " + line);
                    continue;
                }
                if (parts[0] == "watson_user")
                {
                    Watson.User = parts[1];
                }
                else if (parts[0] == "watson_pass")
                {
                    Watson.Pass = parts[1];
                }
                else if (parts[0] == "path_to_mkvmerge_executable")
                {
                    MKVToolsharp.mkvmergepath = parts[1];
                }
                else if (parts[0] == "path_to_mkvextract_executable")
                {
                    MKVToolsharp.mkvextractpath = parts[1];
                }
                else
                {
                    Console.WriteLine("Malformed configuration line: " + line);
                }
            }
            //check for all parameters
            if (
                Watson.User == "" ||
                Watson.Pass == "" ||
                MKVToolsharp.mkvmergepath == "" ||
                MKVToolsharp.mkvextractpath == ""
                )
            {
                Console.WriteLine("Required Configuration Parameters Not Present");
                Console.ReadKey();
                System.Environment.Exit(1);
            }
            //Create the auth string
            Watson.AuthStr = Convert.ToBase64String(Encoding.ASCII.GetBytes(Watson.User + ":" + Watson.Pass));
        }
    }
}
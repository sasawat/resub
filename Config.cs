using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace resub
{
    class DictFile
    {
        public string FileName { get; private set; }
        public string Name { get; private set; }
        public DictFile(string filename, string name)
        {
            FileName = filename;
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }
    }

    internal static class Config
    {
        public static List<DictFile> Dictlist;

        public static void load()
        {
            //Read the config file
            Dictlist = new List<DictFile>();
            var dictionaries = new string[0];
            var dictionarynames = new string[0];
            StreamReader sr;
            try
            {
                sr = new StreamReader(File.OpenRead("resub.conf"));
            }
            catch
            {
                MessageBox.Show("resub.conf not found");
                System.Environment.Exit(1);
                return;
            }
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
                    if (Watson.User[Watson.User.Length - 1] == '\r')
                    {
                        Watson.User = Watson.User.Substring(0, Watson.User.Length - 1);
                    }
                }
                else if (parts[0] == "watson_pass")
                {
                    Watson.Pass = parts[1];
                    if (Watson.Pass[Watson.Pass.Length - 1] == '\r')
                    {
                        Watson.Pass = Watson.Pass.Substring(0, Watson.Pass.Length - 1);
                    }
                }
                else if (parts[0] == "path_to_mkvmerge_executable")
                {
                    MKVToolsharp.mkvmergepath = parts[1];
                    if (MKVToolsharp.mkvmergepath[MKVToolsharp.mkvmergepath.Length - 1] == '\r')
                    {
                        MKVToolsharp.mkvmergepath = MKVToolsharp.mkvmergepath.Substring(0, MKVToolsharp.mkvmergepath.Length - 1);
                    }
                }
                else if (parts[0] == "path_to_mkvextract_executable")
                {
                    MKVToolsharp.mkvextractpath = parts[1];
                    MKVToolsharp.mkvextractpath = parts[1];
                    if (MKVToolsharp.mkvextractpath[MKVToolsharp.mkvextractpath.Length - 1] == '\r')
                    {
                        MKVToolsharp.mkvextractpath = MKVToolsharp.mkvextractpath.Substring(0, MKVToolsharp.mkvextractpath.Length - 1);
                    }
                }
                else if (parts[0] == "dictionaries")
                {
                    dictionaries = parts[1].Split(',');
                    if (dictionaries[dictionaries.Length - 1][dictionaries[dictionaries.Length - 1].Length - 1] == '\r')
                    {
                        dictionaries[dictionaries.Length - 1] = dictionaries[dictionaries.Length - 1].Substring(0, dictionaries[dictionaries.Length - 1].Length - 1);
                    }
                }
                else if(parts[0] == "dictionarynames")
                {
                    dictionarynames = parts[1].Split(',');
                    if (dictionarynames[dictionarynames.Length - 1][dictionarynames[dictionarynames.Length - 1].Length - 1] == '\r')
                    {
                        dictionarynames[dictionarynames.Length - 1] = dictionarynames[dictionarynames.Length - 1].Substring(0, dictionarynames[dictionarynames.Length - 1].Length - 1);
                    }
                    
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
                MessageBox.Show("Required Configuration Parameters Not Present");
                System.Environment.Exit(1);
            }
            //check for correct dictionary specification
            if(dictionaries.Length != dictionarynames.Length)
            {
                MessageBox.Show("Number of dictionaries and dictionarynames do not correspond");
                System.Environment.Exit(1);
            }
            //Create the auth string
            Watson.AuthStr = Convert.ToBase64String(Encoding.ASCII.GetBytes(Watson.User + ":" + Watson.Pass));
            //Create DictList
            for(int i = 0; i < dictionaries.Length; ++i)
            {
                Dictlist.Add(new DictFile(dictionaries[i], dictionarynames[i]));
            }
        }
    }
}
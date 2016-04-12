using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resub
{
    static class MKVToolsharp
    {
        public static string mkvmergepath = "";
        public static string mkvextractpath = "";

        //Returns <audio track ID, subtitle track ID> of mkvFileName file
        private static Tuple<int, int> findTrackIDs(string mkvFileName)
        {
            int subID = -1;
            int audID = -1;
            //Create mkvmerge process
            Process mkvmerge = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "mkvmerge.exe",
                    //FileName = "ping.exe",
                    Arguments = "-i " + mkvFileName,
                    UseShellExecute = false, //we want to start the executable
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            //Start process
            mkvmerge.Start();
            mkvmerge.WaitForExit();

            //Read standard output
            String temp;
            while (!mkvmerge.StandardOutput.EndOfStream)
            {
                temp = mkvmerge.StandardOutput.ReadLine();
                String[] arr = temp.Split(' ');
                //Find "Track ..." Lines
                if (arr[0] == "Track")
                {
                    int id = int.Parse(arr[2].Substring(0, arr[2].Length - 1));
                    if (arr[3] == "audio")
                    {
                        audID = id;
                    }
                    else if (arr[3] == "subtitles")
                    {
                        subID = id;
                    }
                }
            }
            if (audID < 0 || subID < 0) throw new Exception("Finding track IDs failed (does there exist audio and subtitle tracks?)");

            return new Tuple<int, int>(audID, subID);
            
        }

        public static void extrackTracks(string mkvFileName, string audOutName, string subOutName)
        {
            //Get track IDS
            var trackIDs = findTrackIDs(mkvFileName);
            int audID = trackIDs.Item1;
            int subID = trackIDs.Item2;

            //Create mkvextract process
            Process mkvextract = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "mkvextract.exe",
                    //FileName = "ping.exe",
                    Arguments = "tracks \"" + mkvFileName + "\" " +
                                audID + ":" + audOutName + " " +
                                subID + ":" + subOutName,
                    UseShellExecute = false, //we want to start the executable
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = true
                }
            };
            //Run extraction
            mkvextract.Start();
            mkvextract.WaitForExit();
        }

        public static void stripSubtitles(string mkvFileName, string mkvOutName)
        {
            //Create mkvmerge process
            Process mkvmerge = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "mkvmerge.exe",
                    Arguments = "-o " + mkvOutName + " --no-subtitles " + mkvFileName,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardInput = false,
                    CreateNoWindow = true
                }
            };
            
            //Start process
            mkvmerge.Start();
            mkvmerge.WaitForExit();
 
        }

        public static void mergeSubtitles(string mkvFileName, string subFileName, string subTitle, string mkvOutName)
        {
            //Create mkvmerge process
            Process mkvmerge = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "mkvmerge.exe",
                    Arguments = "-o " + mkvOutName + " " + mkvFileName + " --track-name 0:" + subTitle + " " + subFileName,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardInput = false,
                    CreateNoWindow = true
                }
            };
            
            //Start process
            mkvmerge.Start();
            mkvmerge.WaitForExit();
        }
    }
}

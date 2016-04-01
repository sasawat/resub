using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace resub
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load configuration
            Config.load();

            //Get files
            string infile;
            string outfile;
            if(args.Length < 2)
            {
                Console.Write("Input FileName\n> ");
                infile = Console.ReadLine();
                Console.Write("Output FileName (Default: InputFileName.resub.mkv)\n> ");
                outfile = Console.ReadLine();
                if(outfile == "")
                {
                    outfile = infile + ".resub.mkv";
                }
            }
            else
            {
                infile = args[0];
                outfile = args[1];
            }

            TranscriptionCollection tc;
            SubtitleCollection sc;
            AudioChunkCollection ac;

            bool fromfile;

            //See if we've already done a transcription
            if (File.Exists(infile + ".ilog"))
            {
                fromfile = true;
                var ilog = Log.ilogRead(infile + ".ilog");
                tc = ilog.Item2;
                sc = ilog.Item1;
                ac = new AudioChunkCollection();
                //Extract tracks still needed
                MKVToolsharp.extrackTracks(infile, "resub.aud", "resub.ass");
                
            }
            else //we haven't so we have to start afresh
            {
                fromfile = false;
                //Extract tracks
                MKVToolsharp.extrackTracks(infile, "resub.aud", "resub.ass");
                
                //Parse and transcribe
                sc = new SubtitleCollection("resub.ass");
                ac = new AudioChunkCollection("resub.aud", sc);
                tc = new TranscriptionCollection(ac);
            }

            //Write ilog so we don't waste time/money with transcription if we run again
            if(!fromfile) Log.ilogWrite(infile + ".ilog", tc, sc, ac);


            //Create the resuber object
            AllKnownResuber sr = new AllKnownResuber("m1tdic.txt");

            for(int i = 0; i < sc.lines.Count; ++i)
            {
                sr.resub(tc[i], sc[i]);
            }

            //Output the subtitle file
            sc.Output();

            //Merge everything together
            MKVToolsharp.mergeSubtitles(infile, "resubbed.ass", "Resub", outfile);

            ac.CleanUp();
            File.Delete("resub.ass");
            File.Delete("resub.aud");
                       
            Console.WriteLine("DONE");
        }
    }
}

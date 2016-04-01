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

            Console.WriteLine("=========================  resub  ========================");
            Console.WriteLine("University of Michigan");
            Console.WriteLine("ASIANLAN 124 - First Year Japanese through Anime and Manga");
            Console.WriteLine("W16 Final Project");
            Console.WriteLine("Sasawat Prankprakma (psasawat@umich.edu)\n\n");

            //Get files
            string infile;
            string outfile;
            if(args.Length < 1)
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
                if (args.Length >= 2) outfile = args[1];
                else outfile = infile + ".resub.mkv";
                Console.WriteLine("Input File: " + infile);
            }

            TranscriptionCollection tc;
            SubtitleCollection sc;
            AudioChunkCollection ac;

            bool fromfile;

            //See if we've already done a transcription
            if (File.Exists(infile + ".ilog"))
            {
                Console.WriteLine("Found ilog from previous resub run");
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
                Console.WriteLine("Demultiplexing Tracks");
                fromfile = false;
                //Extract tracks
                MKVToolsharp.extrackTracks(infile, "resub.aud", "resub.ass");

                //Parse and transcribe
                Console.WriteLine("Parsing Subtitles");
                sc = new SubtitleCollection("resub.ass");
                Console.WriteLine("Processing Audio");
                ac = new AudioChunkCollection("resub.aud", sc);
                Console.WriteLine("Transcribing Lines");
                tc = new TranscriptionCollection(ac);
            }

            //Write ilog so we don't waste time/money with transcription if we run again
            if(!fromfile) Log.ilogWrite(infile + ".ilog", tc, sc, ac);


            //Create the resuber object
            Console.WriteLine("Loading Dictionary");
            AllKnownResuber sr = new AllKnownResuber(Config.dictionaries[0]);

            Console.WriteLine("resubing!");
            for(int i = 0; i < sc.lines.Count; ++i)
            {
                sr.resub(tc[i], sc[i]);
            }

            //Output the subtitle file
            Console.WriteLine("Writing New Subtitles");
            sc.Output();

            //Merge everything together
            Console.WriteLine("Creating Output File");
            MKVToolsharp.mergeSubtitles(infile, "resubbed.ass", Config.dictionarynames[0], outfile);

            Console.WriteLine("Cleaning Up");
            ac.CleanUp();
            File.Delete("resub.ass");
            File.Delete("resub.aud");
                       
            Console.WriteLine("DONE");
            Console.ReadKey();
        }
    }
}

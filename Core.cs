using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resub
{
    class Core
    {
        public delegate void outputStatusLine(string line);

        public static outputStatusLine println;

        public static void run(string infile, string outfile, List<DictFile> dictionaries, bool interactive)
        {
            println("=========================  resub  ========================");
            println("University of Michigan");
            println("ASIANLAN 124 - First Year Japanese through Anime and Manga");
            println("W16 Final Project");
            println("Sasawat Prankprakma (psasawat@umich.edu)\n\n");

            //Get files
            if (infile == "")
            {
                if(!interactive)
                {
                    println("Error no input file specified");
                    return;
                }
                println("Input FileName\n> ");
                infile = Console.ReadLine();
                Console.Write("Output FileName (Default: InputFileName.resub.mkv)\n> ");
                outfile = Console.ReadLine();
                if (outfile == "")
                {
                    outfile = infile + ".resub.mkv";
                    println("Defaulting to output file: " + outfile);
                }
            }
            else
            {
                if (outfile == "")
                {
                    outfile = infile + ".resub.mkv";
                    println("Defaulting to output file: " + outfile);
                }
                println("Input File: " + infile);
                println("Output File: " + outfile);
            }
            
            //Objects for resub
            TranscriptionCollection tc;
            SubtitleCollection sc;
            AudioChunkCollection ac;
            bool fromfile;

            //See if we've already done a transcription
            if (File.Exists(infile + ".ilog"))
            {
                println("Found ilog from previous resub run");
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
                println("Demultiplexing Tracks");
                fromfile = false;
                //Extract tracks
                MKVToolsharp.extrackTracks(infile, "resub.aud", "resub.ass");

                //Parse and transcribe
                println("Parsing Subtitles");
                sc = new SubtitleCollection("resub.ass");
                println("Processing Audio");
                ac = new AudioChunkCollection("resub.aud", sc);
                println("Transcribing Lines");
                tc = new TranscriptionCollection(ac);
            }

            //Write ilog so we don't waste time/money with transcription if we run again
            if (!fromfile) Log.ilogWrite(infile + ".ilog", tc, sc, ac);

            //Resub!!
            string mergeinto = infile;
            foreach(DictFile dict in dictionaries)
            {
                println("Loading Dictionary: " + dict.FileName);
                AllKnownResuber sr = new AllKnownResuber(dict.FileName);
                SubtitleCollection outsub = sc.Clone();

                println("resubing!");
                for (int j = 0; j < sc.lines.Count; ++j)
                {
                    sr.resub(tc[j], outsub[j]);
                }

                //Output the subtitle file
                println("Writing New Subtitles");
                outsub.Output();

                //Merge everything together
                println("Creating Output File");
                MKVToolsharp.mergeSubtitles(mergeinto, "resubbed.ass", dict.Name, outfile);
                if (File.Exists("temp.mkv")) File.Delete("temp.mkv");
                File.Copy(outfile, "temp.mkv");
                mergeinto = "temp.mkv";
            }
            
            //Cleanup
            println("Cleaning Up");
            ac.CleanUp();
            File.Delete("resub.ass");
            File.Delete("resub.aud");
            File.Delete("resubbed.ass");
            File.Delete("temp.mkv");

            println("DONE");
        }
    }
}

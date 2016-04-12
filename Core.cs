using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace resub
{
    class Core
    {
        public delegate void outputStatusLine(string line);

        public outputStatusLine printlnfunc;

        private bool IsAsync;
        private bool IsInteractive;
        private bool DoStrip;
        private string Infile;
        private string Outfile;
        private int Progress;
        private List<DictFile> Dictionaries;
        public bool IsBusy { get; private set; }
        public BackgroundWorker BW;

        public void runAsync(string infile, string outfile, 
            List<DictFile> dictionaries, bool stripOriginalSubs, ProgressChangedEventHandler pceh)
        {
            if (IsBusy) return;
            IsBusy = true;
            IsAsync = true;
            IsInteractive = false;
            DoStrip = stripOriginalSubs;
            Infile = infile;
            Outfile = outfile;
            Dictionaries = dictionaries;
            BW = new BackgroundWorker();
            BW.WorkerReportsProgress = true;
            BW.DoWork += new DoWorkEventHandler(doWorkHandler);
            BW.ProgressChanged += pceh;
            BW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runWorkerCompleted);
            BW.RunWorkerAsync();
        }

        private void println(string line)
        {
            if (IsAsync) BW.ReportProgress(Progress, line);
            else printlnfunc(line);
        }

        public void run(string infile, string outfile, List<DictFile> dictionaries, bool interactive)
        {
            if (IsBusy) return;
            IsBusy = true;
            IsAsync = false;
            DoStrip = false;
            IsInteractive = interactive;
            Infile = infile;
            Outfile = outfile;
            Dictionaries = dictionaries;
            run();
            IsBusy = false;
        }

        private void runWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            IsBusy = false;
        }

        private void doWorkHandler(object sender, DoWorkEventArgs x)
        {
            run();
        }

        public void run()
        {
            Progress = 0;
            println("=========================  resub  ========================");
            println("University of Michigan");
            println("ASIANLAN 124 - First Year Japanese through Anime and Manga");
            println("W16 Final Project");
            println("Sasawat Prankprakma (psasawat@umich.edu)\n\n");

            //Get files
            if (Infile == "")
            {
                if(!IsInteractive)
                {
                    println("Error no input file specified");
                    return;
                }
                println("Input FileName\n> ");
                Infile = Console.ReadLine();
                Console.Write("Output FileName (Default: InputFileName.resub.mkv)\n> ");
                Outfile = Console.ReadLine();
                if (Outfile == "")
                {
                    Outfile = Infile + ".resub.mkv";
                    println("Defaulting to output file: " + Outfile);
                }
            }
            else
            {
                if (Outfile == "")
                {
                    Outfile = Infile + ".resub.mkv";
                    println("Defaulting to output file: " + Outfile);
                }
                println("Input File: " + Infile);
                println("Output File: " + Outfile);
            }
            
            //Objects for resub
            TranscriptionCollection tc;
            SubtitleCollection sc;
            AudioChunkCollection ac;
            bool fromfile;

            //See if we've already done a transcription
            if (File.Exists(Infile + ".ilog"))
            {
                println("Found ilog from previous resub run");
                fromfile = true;
                var ilog = Log.ilogRead(Infile + ".ilog");
                tc = ilog.Item2;
                sc = ilog.Item1;
                ac = new AudioChunkCollection();
                //Extract tracks still needed
                println("Demultiplexing Tracks");
                MKVToolsharp.extrackTracks(Infile, "resub.aud", "resub.ass");
                Progress = 70;
            }
            else //we haven't so we have to start afresh
            {
                println("Demultiplexing Tracks");
                fromfile = false;
                //Extract tracks
                MKVToolsharp.extrackTracks(Infile, "resub.aud", "resub.ass");
                Progress = 10;

                //Parse and transcribe
                println("Parsing Subtitles");
                sc = new SubtitleCollection("resub.ass");
                Progress = 12;
                println("Processing Audio");
                ac = new AudioChunkCollection("resub.aud", sc);
                Progress = 30;
                println("Transcribing Lines");
                tc = new TranscriptionCollection(ac);
                Progress = 70;
            }

            //Write ilog so we don't waste time/money with transcription if we run again
            if (!fromfile) Log.ilogWrite(Infile + ".ilog", tc, sc, ac);

            //Resub!!
            string mergeinto = Infile;
            if(DoStrip)
            {
                MKVToolsharp.stripSubtitles(Infile, "temp.mkv");
                mergeinto = "temp.mkv";
            }
            foreach(DictFile dict in Dictionaries)
            {
                println("Loading Dictionary: " + dict.FileName);
                AllKnownResuber sr = new AllKnownResuber(dict.FileName);
                SubtitleCollection outsub = sc.Clone();
                Progress = 80;

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
                MKVToolsharp.mergeSubtitles(mergeinto, "resubbed.ass", dict.Name, Outfile);
                if (File.Exists("temp.mkv")) File.Delete("temp.mkv");
                File.Copy(Outfile, "temp.mkv");
                mergeinto = "temp.mkv";
                Progress = 90;
            }
            
            //Cleanup
            println("Cleaning Up");
            ac.CleanUp();
            File.Delete("resub.ass");
            File.Delete("resub.aud");
            File.Delete("resubbed.ass");
            File.Delete("temp.mkv");

            Progress = 100;
            println("DONE");
        }
    }
}

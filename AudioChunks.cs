using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resub
{
    class AudioChunkCollection : IEnumerable<String>
    {
        public SubtitleCollection subs;

        public const String prefix = "chunks";

        public String masterWav { get; private set; }

        public List<String> chunkPaths;

        public AudioChunkCollection(String masterfile, SubtitleCollection sub)
        {
            subs = sub;
            MakeMasterWav(masterfile);
            chunkPaths = new List<string>();
            Chunk();
        }

        //Don't use other than to make C# think instantiated
        public AudioChunkCollection()
        {

        }

        private void MakeMasterWav(String original)
        {
            Process ffmpeg = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg.exe",
                    Arguments = "-i " + original + " resubMaster.wav",
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true
                }
            };
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            masterWav = "resubMaster.wav";
        }

        private void Chunk(Line line)
        {
            String path = prefix + "/" + line.Lnno + ".wav";
            Process ffmpeg = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg.exe",
                    Arguments = "-ss " + ((double)(line.Start)).ToString("F2") +
                               " -t " + ((double)(line.End) - (double)(line.Start)).ToString("F2") +
                               " -i " + masterWav +
                               " -acodec pcm_s16le -ac 1 -ar 44100 " +
                               path,
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = false
                }
            };
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            chunkPaths.Add(path);
        }

        private void Chunk()
        {
            Directory.CreateDirectory(prefix);
            foreach (Line l in subs)
            {
                Chunk(l);
            }
        }

        public void CleanUp()
        {
            try
            {
                Directory.Delete(prefix, true); 
            }
            catch(Exception)
            {
                //Don't do anything
            }
            try
            {
                File.Delete(masterWav);
            }
            catch (Exception)
            {
                //Don't do anything
            }
        }

        public void TestAdd(String x)
        {
            chunkPaths.Add(x);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)chunkPaths).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<string>)chunkPaths).GetEnumerator();
        }

        public String this[int i]
        {
            get
            {
                return chunkPaths[i];
            }
        }
    }
}

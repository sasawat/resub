using System;
using System.IO;

namespace resub
{
    internal static class Log
    {
        public static void ilogWrite(string fileName, TranscriptionCollection tr, SubtitleCollection sc, AudioChunkCollection ac)
        {
            string output = "";
            for (int i = 0; i < tr.Transcriptions.Count; ++i)
            {
                output += ac[i] + "^" + sc[i].Text + "^" +
                    tr[i].Guess[0] + "^" +
                    tr[i].Guess[1] + "^" +
                    tr[i].Guess[2] + "^" +
                    tr[i].Guess[3] + "^" +
                    tr[i].Guess[4] + "^" +
                    tr[i].Confi + "\n";
            }
            StreamWriter sw = new StreamWriter(File.OpenWrite(fileName));
            sw.Write(output);
            sw.Close();
        }

        public static Tuple<SubtitleCollection, TranscriptionCollection> ilogRead(string fileName)
        {
            StreamReader sr = new StreamReader(File.OpenRead(fileName));
            string input = sr.ReadToEnd();
            SubtitleCollection sc = SubtitleCollection.FromFile(input);
            TranscriptionCollection tc = TranscriptionCollection.FromFile(input);
            return new Tuple<SubtitleCollection, TranscriptionCollection>(sc, tc);
        }
    }
}
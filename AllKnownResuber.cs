using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resub
{
    class AllKnownResuber : IResuber
    {
        List<string> words;
        public AllKnownResuber(string filename)
        {
            StreamReader read = File.OpenText(filename);

            words = new List<string>();

            string[] numeral = { "none" };
            ResubUtils.VerbConjugation[] vconj = { };
            ResubUtils.AdjConjugation[] aconj = { };


            while(!read.EndOfStream)
            {
                string ln = read.ReadLine();
                string[] parts = ln.Split(',');
                if (parts[0] == "Numeral")
                {
                    numeral = ResubUtils.generateNumbers(int.Parse(parts[1])).ToArray();
                    words.AddRange(numeral);
                }
                else if (parts[0] == "Counter")
                {
                    if (numeral[0] == "none") throw new Exception("No number set defined. Please use Numeral command");
                    foreach (string x in numeral)
                    {
                        words.Add(x + parts[1]);
                    }
                }
                else if (parts[0] == "VerbConjugate")
                {
                    vconj = new ResubUtils.VerbConjugation[parts.Length - 1];
                    for (int i = 1; i < parts.Length; ++i)
                    {
                        if (parts[i] == "MASU") vconj[i - 1] = ResubUtils.VerbConjugation.MASU;
                        if (parts[i] == "PLAIN") vconj[i - 1] = ResubUtils.VerbConjugation.PLAIN;
                        if (parts[i] == "TE") vconj[i - 1] = ResubUtils.VerbConjugation.TE;
                        if (parts[i] == "TARI") vconj[i - 1] = ResubUtils.VerbConjugation.TARI;
                        if (parts[i] == "TAI") vconj[i - 1] = ResubUtils.VerbConjugation.TAI;
                        if (parts[i] == "TEIRU") vconj[i - 1] = ResubUtils.VerbConjugation.TEIRU;
                    }
                }
                else if (parts[0] == "AdjConjugate")
                {
                    aconj = new ResubUtils.AdjConjugation[parts.Length - 1];
                    for (int i = 1; i < parts.Length; ++i)
                    {
                        if (parts[i] == "PLAIN") aconj[i - 1] = ResubUtils.AdjConjugation.PLAIN;
                        if (parts[i] == "TE") aconj[i - 1] = ResubUtils.AdjConjugation.TE;
                        if (parts[i] == "PAST") aconj[i - 1] = ResubUtils.AdjConjugation.PAST;
                        if (parts[i] == "PASTNEGATIVE") aconj[i - 1] = ResubUtils.AdjConjugation.PASTNEGATIVE;
                        if (parts[i] == "NEGATIVE") aconj[i - 1] = ResubUtils.AdjConjugation.NEGATIVE;
                    }
                }
                else if (parts[0] == "ru")
                {
                    words.AddRange(ResubUtils.conjugate(parts[1], ResubUtils.VerbType.RU, vconj));
                }
                else if (parts[0] == "u")
                {
                    words.AddRange(ResubUtils.conjugate(parts[1], ResubUtils.VerbType.U, vconj));
                }
                else if (parts[0] == "i")
                {
                    words.AddRange(ResubUtils.conjugate(parts[1], ResubUtils.AdjType.I, aconj));
                }
                else if (parts[0] == "na")
                    words.AddRange(ResubUtils.conjugate(parts[1], ResubUtils.AdjType.NA, aconj));
                else
                {
                    words.AddRange(parts);
                }
            }
        }

        public void resub(Transcription tr, Line ln)
        {
            if(ResubUtils.containsOnly(tr, words.ToArray()))
            {
                ln.Text = " ";
            }
        }
    }
}

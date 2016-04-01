using System;
using System.Collections.Generic;

namespace resub
{
    internal interface IResuber
    {
        void resub(Transcription tr, Line ln);
    }

    internal static class ResubUtils
    {
        public static bool hasWord(Transcription tr, String word)
        {
            foreach (string x in tr)
            {
                if (x.Contains(word)) return true;
            }
            return false;
        }

        public static bool hasAny(Transcription tr, String[] word)
        {
            foreach (string x in tr)
            {
                foreach (string y in word)
                {
                    if (x.Contains(y)) return true;
                }
            }
            return false;
        }

        public static bool hasWord(Line ln, String word)
        {
            if (ln.Text.Contains(word)) return true;
            return false;
        }

        public static bool hasAny(Line ln, String[] word)
        {
            foreach (string y in word)
            {
                if (ln.Text.Contains(y)) return true;
            }
            return false;
        }

        public static bool containsOnly(Transcription tr, String[] word)
        {
            //loop through each candidate
            foreach (string x in tr)
            {
                if (x == "") continue;
                string[] trw = x.Split(' ');
                bool ok = true;
                //loop through each word in the transcription
                foreach (string y in trw)
                {
                    //check if the word is in word
                    bool isvocab = false;
                    //special check for whitespace
                    if (y == "") continue;
                    //loop through words looking for match
                    foreach (string z in word)
                    {
                        //check for word
                        if (z == y) isvocab = true;
                    }
                    if (!isvocab)
                    {
                        ok = false;
                        break;
                    }
                }
                //return if the transcription candidate only contains words in word
                if (ok) return true;
            }
            //no transcription candidate only contains words in word
            return false;
        }

        public static string toNihonNumeral(int i)
        {
            if (i > 99999) throw new Exception("Numbers above 九万九千九百九十九 are not supported yet");
            string ret = "";
            int place = 1;
            while (i != 0)
            {
                //add digit
                if (i % 10 == 0)
                {
                    i /= 10;
                    place *= 10;
                    continue;
                }
                switch (place)
                {
                    case 10: ret = "十" + ret; break;
                    case 100: ret = "百" + ret; break;
                    case 1000: ret = "千" + ret; break;
                    case 10000: ret = "万" + ret; break;
                    default: ret = ret + ""; break;
                }
                switch (i % 10)
                {
                    case 1:
                        if (place != 10 && place != 100) ret = "一" + ret;
                        break;

                    case 2: ret = "二" + ret; break;
                    case 3: ret = "三" + ret; break;
                    case 4: ret = "四" + ret; break;
                    case 5: ret = "五" + ret; break;
                    case 6: ret = "六" + ret; break;
                    case 7: ret = "七" + ret; break;
                    case 8: ret = "八" + ret; break;
                    case 9: ret = "九" + ret; break;
                }
                //add place symbol
                i = i / 10;
                place *= 10;
            }
            if (ret.Substring(0, 1) == "一" && place < 100000 && place >= 10)
            {
                ret = ret.Substring(1);
            }
            return ret;
        }

        public static List<string> generateNumbers(int max)
        {
            var ret = new List<string>();
            for (int i = 1; i < max; ++i)
            {
                ret.Add(toNihonNumeral(i));
                ret.Add("" + i);
            }
            return ret;
        }

        public enum VerbConjugation { MASU, TE, TEIRU, TAI, PLAIN, TARI };

        public enum VerbType { RU, U };

        public static string stem(string word, VerbType type)
        {
            if (type == VerbType.RU) return word.Substring(0, word.Length - 1);
            //else type == VerbType.U
            string cend = word.Substring(word.Length - 1, 1);
            string nend = "";
            string front = word.Substring(0, word.Length - 1);
            if (cend == "う") nend = "い";
            if (cend == "く") nend = "き";
            if (cend == "ぐ") nend = "ぎ";
            if (cend == "す") nend = "し";
            if (cend == "ず") nend = "じ";
            if (cend == "つ") nend = "ち";
            if (cend == "づ") nend = "ぢ";
            if (cend == "む") nend = "み";
            if (cend == "ぬ") nend = "に";
            if (cend == "ふ") nend = "ひ";
            if (cend == "ぶ") nend = "び";
            if (cend == "ぷ") nend = "ぴ";
            if (cend == "る") nend = "り";
            if (nend == "") throw new Exception("Error creating word stem please check word is a regular verb");
            return front + nend;
        }

        public static string teform(string word, VerbType type)
        {
            if (type == VerbType.RU) return word.Substring(0, word.Length - 1) + "て";
            //else type == VerbType.U
            //else type == VerbType.U
            string cend = word.Substring(word.Length - 1, 1);
            string nend = "";
            string front = word.Substring(0, word.Length - 1);
            if (cend == "う") nend = "って";
            if (cend == "く") nend = "いて";
            if (cend == "ぐ") nend = "いで";
            if (cend == "す") nend = "して";
            if (cend == "つ") nend = "って";
            if (cend == "む") nend = "んで";
            if (cend == "ぬ") nend = "んで";
            if (cend == "ぶ") nend = "んで";
            if (cend == "る") nend = "って";
            if (nend == "") throw new Exception("Error creating word stem please check word is a regular verb");
            return front + nend;
            
        }

        public static string taform(string word, VerbType type)
        {
            if (type == VerbType.RU) return word.Substring(0, word.Length - 1) + "て";
            //else type == VerbType.U
            //else type == VerbType.U
            string cend = word.Substring(word.Length - 1, 1);
            string nend = "";
            string front = word.Substring(0, word.Length - 1);
            if (cend == "う") nend = "った";
            if (cend == "く") nend = "いた";
            if (cend == "ぐ") nend = "いだ";
            if (cend == "す") nend = "した";
            if (cend == "つ") nend = "った";
            if (cend == "む") nend = "んだ";
            if (cend == "ぬ") nend = "んだ";
            if (cend == "ぶ") nend = "んだ";
            if (cend == "る") nend = "った";
            if (nend == "") throw new Exception("Error creating word stem please check word is a regular verb");
            return front + nend;
            
        }

        public static string plainegbase(string word, VerbType type)
        {
            if (type == VerbType.RU) return word.Substring(0, word.Length - 1);
            //else type == VerbType.U
            string cend = word.Substring(word.Length - 1, 1);
            string nend = "";
            string front = word.Substring(0, word.Length - 1);
            if (cend == "う") nend = "わ";
            if (cend == "く") nend = "か";
            if (cend == "ぐ") nend = "が";
            if (cend == "す") nend = "さ";
            if (cend == "つ") nend = "た";
            if (cend == "む") nend = "ま";
            if (cend == "ぬ") nend = "な";
            if (cend == "ぶ") nend = "ば";
            if (cend == "る") nend = "ら";
            if (nend == "") throw new Exception("Error creating word stem please check word is a regular verb");
            return front + nend;
        }

        public static List<string> conjugate(string word, VerbType type, VerbConjugation[] ways)
        {
            var ret = new List<string>();
            foreach (VerbConjugation x in ways)
            {
                switch (x)
                {
                    case VerbConjugation.MASU:
                        string temp = stem(word, type);
                        ret.Add(temp + "ます");
                        ret.Add(temp + "ません");
                        ret.Add(temp + "ました");
                        ret.Add(temp + "ませんでした");
                        break;
                    case VerbConjugation.TE:
                        ret.Add(teform(word, type));
                        break;
                    case VerbConjugation.TEIRU:
                        ret.Add(teform(word, type) + "いる");
                        ret.Add(teform(word, type) + "います");
                        break;
                    case VerbConjugation.TAI:
                        ret.Add(stem(word, type) + "たい");
                        break;
                    case VerbConjugation.TARI:
                        ret.Add(taform(word, type) + "り");
                        break;
                    case VerbConjugation.PLAIN:
                        ret.Add(word);
                        ret.Add(taform(word, type));
                        ret.Add(plainegbase(word, type) + "ない");
                        ret.Add(plainegbase(word, type) + "なかった");
                        break;
                }
            }
            return ret;
        }

        public enum AdjConjugation { PAST, NEGATIVE, PASTNEGATIVE, PLAIN, TE };

        public enum AdjType { I, NA };

        public static string negative(string word, AdjType type)
        {
            if (type == AdjType.NA) return word + "じゃない";
            //else type == AdjType.I
            return word.Substring(0, word.Length - 1) + "くない";
        }

        public static string past(string word, AdjType type)
        {
            if (type == AdjType.NA) return word; //Watson does datta/deshita separate I think
            //else type == AdjType.I;
            return word.Substring(0, word.Length - 1) + "かった";
        }

        public static string teform(string word, AdjType type)
        {
            if (type == AdjType.NA) return word + "で";
            //else type == AdjType.I
            return word.Substring(0, word.Length - 1) + "くて";
        }

        public static List<string> conjugate(string word, AdjType type, AdjConjugation[] ways)
        {
            var ret = new List<string>();
            foreach(AdjConjugation x in ways)
            {
                switch(x)
                {
                    case AdjConjugation.PLAIN:
                        ret.Add(word);
                        if (type == AdjType.NA) ret.Add(word + "な");
                        break;
                    case AdjConjugation.PAST:
                        ret.Add(past(word, type));
                        break;
                    case AdjConjugation.PASTNEGATIVE:
                        ret.Add(past(negative(word, type), AdjType.I));
                        break;
                    case AdjConjugation.NEGATIVE:
                        ret.Add(negative(word, type));
                        break;
                    case AdjConjugation.TE:
                        ret.Add(teform(word, type));
                        break;
                }
            }
            return ret;
        }
    }

    internal class SampleResuber : IResuber
    {
        public void resub(Transcription tr, Line ln)
        {
            string[] samplevocab = { "怖い" };
            if (ResubUtils.containsOnly(tr, samplevocab))
            {
                ln.Text = " ";
            }
        }
    }
}
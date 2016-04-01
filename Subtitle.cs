using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace resub
{
    class Timestamp
    {
        public int Hr;
        public int Min;
        public int Sec;
        public int Frac;

        public Timestamp(String x)
        {
            Hr = int.Parse(x.Substring(0, 1));
            Min = int.Parse(x.Substring(2, 2));
            Sec = int.Parse(x.Substring(5, 2));
            Frac = int.Parse(x.Substring(8, 2));
        }
        
        public static explicit operator double(Timestamp t)
        {
            return 3600.0 * t.Hr + 60.0 * t.Min + t.Sec + t.Frac / 100.0;
        }

        public override string ToString()
        {
            return Hr + ":" + Min + ":" + Sec + ":" + Frac;
        }
    }

    class Line
    {
        public int Lnno;
        public Timestamp Start;
        public Timestamp End;
        public String Text;

        public Line(String start, String end, String text, int lnno)
        {
            Start = new Timestamp(start);
            End = new Timestamp(end);
            Text = text;
            Lnno = lnno;
        }

        public override string ToString()
        {
            return Start.ToString() + " " + End.ToString() + " " + Text;
        }
    }

    class SubtitleCollection : IEnumerable<Line>
    {
        public String file { get; private set; }

        private int posStart;
        private int posEnd;
        private int posText;

        public List<Line> lines { get; private set; }

        public SubtitleCollection(String filename)
        {
            file = filename;
            lines = new List<Line>();
            posStart = -1;
            posEnd = -1;
            posText = -1;
            //Open for reading
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            //Loop looking for [Events]
            String temp;
            int num = 0;
            while(!sr.EndOfStream)
            {
                temp = sr.ReadLine();
                ++num;
                if(temp == "[Events]")
                {
                    break;
                }
            }
            //Loop looking for Format Specifier
            while(!sr.EndOfStream)
            {
                temp = sr.ReadLine();
                ++num;
                if (temp.Length > "Format:".Length && temp.Substring(0, "Format:".Length) == "Format:")
                {
                    ParseFormat(temp);
                    break;
                }
            }
            //Check format specifier found
            if(posEnd == -1 || posStart == -1 || posEnd == -1)
            {
                Console.Error.WriteLine("Subtitle Lacks Format Specifier");
                Environment.Exit(1);
            }
            while(!sr.EndOfStream)
            {
                temp = sr.ReadLine();
                ParseLine(temp, num);
                ++num;
            }
            sr.Close();
        }

        private SubtitleCollection()
        {
            file = "";
            posStart = 1;
            posEnd = 2;
            posText = 9;
            lines = new List<Line>();
        }

        public void Output()
        {
            FileStream ofile = File.OpenWrite("resubbed.ass");
            FileStream orig = File.OpenRead(file);
            StreamReader read = new StreamReader(orig);
            StreamWriter write = new StreamWriter(ofile);
            int filelnno = 0;
            int sublndx = 0;
            while(!read.EndOfStream)
            {
                string subline = read.ReadLine();
                if(sublndx < lines.Count && filelnno == lines[sublndx].Lnno)
                {
                    int comma = 0;
                    for(int i = 0; i < posText; ++i)
                    {
                        comma = subline.IndexOf(',', comma + 1);
                    }
                    subline = subline.Substring(0, comma) + "," + lines[sublndx].Text;
                    sublndx++;
                }
                write.WriteLine(subline);
                filelnno++;
            }
            orig.Close();
            ofile.Close();
        }

        public static SubtitleCollection FromFile(String text)
        {
            String[] lns = text.Split('\n');
            SubtitleCollection ret = new SubtitleCollection();
            ret.file = "resub.ass";
            foreach(String ln in lns)
            {
                if (ln == "") break;
                String[] parts = ln.Split('^');
                int lnno = int.Parse(parts[0].Substring(7, parts[0].Length - 11));
                ret.lines.Add(new Line("0:00:00:00", "1:00:00:00", parts[1], lnno));
            }
            return ret;
        }
        
        private void ParseFormat(String line)
        {
            //Check if actually a format line
            if (line.Length < "Format:".Length || line.Substring(0, "Format:".Length) != "Format:")
            {
                return; //Not a format line
            }
            //Extract the format spec
            String[] fmtspec = line.Substring("Format:".Length).Split(',');
            //Loop through looking for what we need
            for(int i = 0; i < fmtspec.Length; ++i)
            {
                if(fmtspec[i].Contains("Start"))
                {
                    posStart = i;
                }
                else if(fmtspec[i].Contains("End"))
                {
                    posEnd = i;
                }
                else if(fmtspec[i].Contains("Text"))
                {
                    posText = i;
                }
            }
        }

        private void ParseLine(String line, int num)
        {
            //Check that it is a Dialogue line
            if(line.Length < "Dialogue:".Length || line.Substring(0, "Dialogue:".Length) != "Dialogue:")
            {
                return;
            }
            //Extract stuff after Dialogue:
            String data = line.Substring("Dialogue:".Length);
            String sstart = "INVALID", send = "INVALID", stext;
            int i = 0;
            int il = 0;
            int at = 0;
            while((i = data.IndexOf(',', il + 1)) != -1)
            {
                if(at == posStart)
                {
                    sstart = data.Substring(il + 1, data.IndexOf(',', il + 1) - il - 1);
                }
                else if(at == posEnd)
                {
                    send = data.Substring(il + 1, data.IndexOf(',', il + 1) - il - 1);
                }
                il = i;
                ++at;
                if (at == posText) break;
            }
            if(i == -1 || at != posText || sstart == "INVALID" || send == "INVALID")
            {
                Console.Error.WriteLine("Warning Invalid Dialogue Line at " + num);
                return;
            }
            //Extract the text (remaining stuffs)
            stext = data.Substring(il + 1);
            //Heuristic to avoid signage and karaoke
            if (stext.IndexOf("\\pos") != -1) return;
            if (stext.IndexOf("\\blur") != -1) return;
            //Clean up stuff
            while((i = sstart.IndexOf(' ')) != -1)
            {
                sstart.Remove(i, 1);
            }
            while ((i = send.IndexOf(' ')) != -1)
            {
                send.Remove(i, 1);
            }
            //Okay push back
            if((double)(new Timestamp(send)) - (double)(new Timestamp(sstart)) < 0.5)
            {
                //Timeless subtitle
                return;
            }
            lines.Add(new Line(sstart, send, stext, num));           
        }

        public override string ToString()
        {
            String ret = "";
            foreach(Line x in lines)
            {
                ret += x + "\n";
            }
            return ret;
        }

        public IEnumerator<Line> GetEnumerator()
        {
            return ((IEnumerable<Line>)lines).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Line>)lines).GetEnumerator();
        }

        public Line this[int i]
        {
            get
            {
                return lines[i];
            }
        }
    }
}

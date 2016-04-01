using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Helpers;

namespace resub
{
    internal static class Watson
    {
        public static String User = "";
        public static String Pass = "";
        public static String AuthStr = Convert.ToBase64String(Encoding.ASCII.GetBytes(User + ":" + Pass));

        public static String Test()
        {
            HttpWebRequest req = WebRequest.CreateHttp(@"https://stream.watsonplatform.net/speech-to-text/api/v1/models");
            req.Headers.Add("Authorization", "Basic " + AuthStr);
            req.Accept = "*/*";
            req.Method = "GET";
            req.KeepAlive = false;
            WebResponse res = req.GetResponse();
            String ret = new StreamReader(res.GetResponseStream()).ReadToEnd();
            res.Close();
            return ret;
        }

        public static String InvokeRecognize(String fileName)
        {
            //Set up the file read
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);

            //Set up the webrequest
            WebRequest req = WebRequest.CreateHttp(@"https://stream.watsonplatform.net/speech-to-text/api/v1/recognize?model=ja-JP_BroadbandModel&continuous=true&max_alternatives=5");
            req.ContentType = "audio/wav";
            req.Headers.Add("Authorization", "Basic " + AuthStr);
            req.Method = "POST";
            req.ContentLength = fs.Length;

            //Run the request
            Stream reqstream = req.GetRequestStream();
            reqstream.Write(data, 0, data.Length);
            reqstream.Close();
            WebResponse res = req.GetResponse();
            String ret = (new StreamReader(res.GetResponseStream())).ReadToEnd();

            //Finish up
            res.Close();
            return ret;
        }
    }

    internal class Transcription : IEnumerable
    {
        public String[] Guess { get; private set; }
        public double Confi { get; private set; }

        private Transcription(String[] guess, double confi)
        {
            Guess = new String[5];
            guess.CopyTo(Guess, 0);
            Confi = confi;
        }

        public static Transcription AskWatson(String audFile)
        {
            String response = (Watson.InvokeRecognize(audFile));
            return DeserializeWatsonResponse(response);
        }

        private static string extractFullAlternative(dynamic dec, int alt)
        {
            string ret = "";
            int i = 0;
            try
            {
                while(true)
                {
                    try
                    {
                        dynamic dtemp = dec["results"][i];
                    }
                    catch(Exception ex)
                    {
                        break;
                    }
 
                    ret += dec["results"][i]["alternatives"][alt]["transcript"];
                    ++i;
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Incomplete result from Watson. Audio may be noisy, mistimed, or missing");
            }
            return ret;
        }

        private static double extractLowestAlternativeConfidence(dynamic dec)
        {
            double ret = 100;
            try
            {
                int i = 0;
                while(true)
                {
                    try
                    {
                        dynamic dtemp = dec["results"][i];
                    }
                    catch(Exception ex)
                    {
                        break;
                    }
                    double temp = (float)dec["results"][i]["alternatives"][0]["confidence"];
                    ret = temp < ret ? temp : ret;
                    ++i;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Incomplete confidence from Watson. ");
            }
            return ret;
        }

        public static Transcription DeserializeWatsonResponse(String json)
        {
            dynamic dec = Json.Decode(json);

            String[] starr = new String[5];
            starr[0] = starr[1] = starr[2] = starr[3] = starr[4] = "";
            double confi = 0;
            confi = extractLowestAlternativeConfidence(dec);
            starr[0] = extractFullAlternative(dec, 0);
            starr[1] = extractFullAlternative(dec, 1);
            starr[2] = extractFullAlternative(dec, 2);
            starr[3] = extractFullAlternative(dec, 3);
            starr[4] = extractFullAlternative(dec, 4);
            return new Transcription(starr, confi);
        }

        public static Transcription FromFile(string line)
        {
            String[] starr = new String[5];
            String[] dcarr = line.Split('^');
            double confi = 0;
            try
            {
                starr[0] = dcarr[2];
                starr[1] = dcarr[3];
                starr[2] = dcarr[4];
                starr[3] = dcarr[5];
                starr[4] = dcarr[6];
                confi = double.Parse(dcarr[7]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error no audio?");
                starr[0] = "";
                starr[1] = "";
                starr[2] = "";
                starr[3] = "";
                starr[4] = "";
            }
            return new Transcription(starr, confi);
        }

        public IEnumerator GetEnumerator()
        {
            return Guess.GetEnumerator();
        }
    }

    internal class TranscriptionCollection : IEnumerable
    {
        public AudioChunkCollection Chunks { get; private set; }
        public List<Transcription> Transcriptions { get; private set; }

        public TranscriptionCollection(AudioChunkCollection chunks)
        {
            Chunks = chunks;
            Transcriptions = new List<Transcription>();
            foreach (String x in chunks)
            {
                Console.WriteLine("Transcribing " + x);
                Transcriptions.Add(Transcription.AskWatson(x));
            }
        }

        private TranscriptionCollection()
        {
            Transcriptions = new List<Transcription>();
        }

        public static TranscriptionCollection FromFile(string text)
        {
            TranscriptionCollection ret = new TranscriptionCollection();
            String[] lns = text.Split('\n');
            foreach (String ln in lns)
            {
                if (ln == "") break;
                ret.Transcriptions.Add(Transcription.FromFile(ln));
            }
            return ret;
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)Transcriptions).GetEnumerator();
        }

        public Transcription this[int i]
        {
            get
            {
                return Transcriptions[i];
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonarDatabase
{
    public class TranslateEntry
    {
        static private string[] endlines = { "\n", "\r\n", "\r" };

        public TranslateEntry(string text)
        {
            if (text.Length == 0)
            {
                Lines = new string[0];
                Paragraphs = new string[0];
            }
            Lines = text.Split(endlines, StringSplitOptions.None);
            int paragraphCount = 0;
            int lineCount = Lines.Count();
            for (int i = 0; i < lineCount; ++i)
            {
                if (Lines[i].Length > 0)
                {
                    string tr = Lines[i].Trim();
                    if (tr.Length == 0)
                    {
                        Lines[i] = tr;
                    }
                    else
                    {
                        paragraphCount++;
                    }
                }
            }
            Paragraphs = new string[paragraphCount];
            paragraphCount = 0;
            for (int i = 0; i < lineCount; ++i)
            {
                if (Lines[i].Length > 0)
                {
                    Paragraphs[paragraphCount++] = Lines[i];
                }
            }
        }

        public override string ToString()
        {
            return ToString("\r\n", Lines);
        }

        public string[] Lines { get; private set; }
        public string[] Paragraphs { get; private set; }

        public static string ToString(string newLine, string[] lineArray, int lineCount = -1)
        {
            string ret = "";
            int lc = 0;
            foreach (string line in lineArray)
            {
                if (lc < lineCount) break;
                if (lc > 0) ret += newLine;
                ret += line;
                ++lc;
            }
            return ret;
        }
    }
}

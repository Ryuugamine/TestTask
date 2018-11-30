using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string path = @"D://file.txt";
            List<String> lines;
            
            try
            {
                String mark;

                lines = Utils.ReadFile(path);

                for(int i = 0; i<lines.Count; i++)
                {

                    if (lines[i].Split(' ')[0].Equals("if"))
                    {
                        mark = Utils.ParseIf(lines[i]);
                    } else
                    {
                        mark = Utils.ParseLine(lines[i]);
                    }

                    if (mark != null)
                    {
                        for (int j = 0; j < lines.Count; j++)
                        {
                            if (lines[j].Contains(mark) && !lines[j].Contains("goto"))
                            {
                                mark = null;
                                i = j;
                                break;
                            }
                        }
                    }
                }

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Catch: "+e.ToString());
                Console.ReadKey();
            }
        }
    }
}

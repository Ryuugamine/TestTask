using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileParser
{
    static class Utils
    {
        static Dictionary<string, int> variables = new Dictionary<string, int>();
        enum operation { mult, rem, div, add, sutr };

        public static List<String> ReadFile(String path)
        {
            List<String> lines = new List<String>();
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }

        public static String ParseIf(String s)
        {
            s = s.Substring(3);

            if (!s.Equals(""))
            {
                String[] paths = s.Split(' ');

                List<int> num = new List<int>();
                num.Add(GetValues(paths[0]));
                num.Add(GetValues(paths[2]));

                if (Checker.CheckCondition(paths[1], num).Check())
                {
                    return ParseLine(s.Substring(s.IndexOf(paths[2]) + paths[2].Length + 1));
                }

                return null;
            }
            else
            {
                return null;
            }
        }

        public static String ParseLine(String s)
        {
            // тут вроде на null проверять не надо, итак одни условия
            if (s.Contains(Constants.GO_TO))
            {
                return s.Split(' ')[1];
            }

            if (s.Contains(Constants.READ))
            {
                ReadValue(s.Split(' ')[1]);
            }
            if (s.Contains(Constants.PRINT))
            {
                PrintValue(s.Split(' ')[1]);
            }
            if (s.Contains(" = "))
            {
                s = s.Replace(" ", "");
                string[] paths = s.Split('=');
                if (variables.ContainsKey(paths[0]))
                {
                    variables[paths[0]] = ParseExpression(paths[1]);
                }
                else
                {
                    variables.Add(paths[0], ParseExpression(paths[1]));
                }
            }
            return null;
        }
    
        private static int ParseExpression(String expr)
        {
            string[] vars = Regex.Split(expr, @"\W");
            for (int i = 0; i < vars.Length; i++)
            {
                if (!vars[i].Equals("") && !int.TryParse(vars[i], out int a))
                {
                   expr = expr.Replace(vars[i], GetValues(vars[i]).ToString());
                }
            }

            return RPN.Calculate(expr);
        }

        private static int GetValues(String s)
        {
            int value;

            if (!int.TryParse(s, out value))
            {
                value = variables[s];
            }
                
            return value;
        }

        private static void PrintValue(String s)
        {
            int value;
            if (int.TryParse(s, out value))
            {
                Console.WriteLine(value);
            }
            else
            {
                Console.WriteLine(variables[s]);
            }
        }

        private static void ReadValue(String s)
        {
            String a = Console.ReadLine();
            try
            {
                if (variables.ContainsKey(s))
                {
                    variables[s] = int.Parse(a);
                }
                else
                {
                    variables.Add(s, int.Parse(a));
                }
            }
            catch
            {
                Console.WriteLine("Ввод только интовыми числами!");
            }
        }
    }
}

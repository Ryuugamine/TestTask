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
                    //Console.WriteLine(line);
                }
            }
            return lines;
        }

        public static String ParseIf(String s)
        {
            s = s.Substring(3);

            String[] paths = s.Split(' ');

            switch (paths[1])
            {
                case "!=":
                    if (GetValues(paths[0]) != GetValues(paths[2]))
                        return ParseLine(s.Substring(s.IndexOf(paths[2]) + paths[2].Length + 1));
                    break;
                case "==":
                    if (GetValues(paths[0]) == GetValues(paths[2]))
                        return ParseLine(s.Substring(s.IndexOf(paths[2]) + paths[2].Length + 1));
                    break;
                case ">=":
                    if (GetValues(paths[0]) >= GetValues(paths[2]))
                        return ParseLine(s.Substring(s.IndexOf(paths[2]) + paths[2].Length + 1));
                    break;
                case "<=":
                    if (GetValues(paths[0]) <= GetValues(paths[2]))
                        return ParseLine(s.Substring(s.IndexOf(paths[2]) + paths[2].Length + 1));
                    break;
                case ">":
                    if (GetValues(paths[0]) > GetValues(paths[2]))
                        return ParseLine(s.Substring(s.IndexOf(paths[2]) + paths[2].Length + 1));
                    break;
                case "<":
                    if (GetValues(paths[0]) < GetValues(paths[2]))
                        return ParseLine(s.Substring(s.IndexOf(paths[2]) + paths[2].Length + 1));
                    break;
            }
            return null;
        }

        public static String ParseLine(String s)
        {
            if (s.Contains(Constants.GO_TO))
            {
                return s.Split(' ')[1];
            }

            if (s.Contains(Constants.READ))
                ReadValue(s.Split(' ')[1]);
            if (s.Contains(Constants.PRINT))
                PrintValue(s.Split(' ')[1]);
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


        private static string RemoveHooks(string expr)
        {
            int start = expr.IndexOf('(') + 1;
            int length;
            string buf = expr.Substring(start);
            string subexpr;

            int count = 1;
            while (count > 0)
            {
                if (buf.IndexOf('(') < buf.IndexOf(')') && buf.IndexOf('(') > 0)
                {
                    buf = buf.Substring(buf.IndexOf('(') + 1);
                    count++;
                }
                else
                {
                    buf = buf.Substring(buf.IndexOf(')') + 1);
                    count--;
                }
            }
            length = expr.Length - buf.Length - start - 1;
            subexpr = expr.Substring(start, length);
            string replaceValue = ParseExpression(subexpr).ToString();
            if (replaceValue.ToCharArray()[0] == Constants.MINUS)
                replaceValue = "0" + replaceValue;
            return expr = expr.Replace("(" + subexpr + ")", replaceValue);
        }
    

        private static int ParseExpression(String expr)
        {
            List<int> values = new List<int>();

            while (expr.Contains("("))
            {
                expr = RemoveHooks(expr);
            }

            int operationCount = GetOperationCount(expr);

            if (operationCount > 1)
            {
                if (expr.Contains(Constants.PLUS))
                {
                    string[] paths = expr.Split(Constants.PLUS);
                    for (int i = 0; i < paths.Length; i++)
                    {
                        values.Add(ParseExpression(paths[i]));
                    }
                    int res = 0;
                    res = values[0];
                    for (int i = 1; i < paths.Length; i++)
                    {
                        res += values[i];
                    }
                    return res;
                }
                else if (expr.Contains(Constants.MINUS))
                {
                    string[] paths = expr.Split(Constants.MINUS);
                    for (int i = 0; i < paths.Length; i++)
                    {
                        if(!paths[i].Equals(""))
                            values.Add(ParseExpression(paths[i]));
                    }
                    int res = 0;
                    if (paths[0].Equals(""))
                    {
                        res = -values[0];
                    }
                    else
                    {
                        res = values[0];
                    }

                    for (int i = 1; i < values.Count; i++)
                    {
                        res -= values[i];
                    }
                    return res;
                }
                else
                {
                    String[] vars = Regex.Split(expr, @"\W");
                    String[] oper = Regex.Split(expr, @"\w");
                    int res = GetValues(vars[0]);
                    for (int i = 0; i < vars.Length - 1; i++)
                    {
                        res = Checker.check(oper[i + 1]).performAction(res, GetValues(vars[i + 1]));
                    }
                    return res;
                }
            }
            else if (operationCount == 1)
            {
                String[] vars = Regex.Split(expr, @"\W");
                String[] oper = Regex.Split(expr, @"\w");

                if (expr.ToCharArray()[0] == Constants.MINUS)
                {
                    return -GetValues(expr.Substring(1));
                }     
                return Checker.check(oper[1]).performAction(GetValues(vars[0]), GetValues(vars[1]));

            }
            else return GetValues(expr);
        }

        private static int GetOperationCount(String s)
        {
            int operationCount = 0;
            operationCount += s.Split(Constants.PLUS).Length - 1;
            operationCount += s.Split(Constants.MINUS).Length - 1;
            operationCount += s.Split(Constants.MULTIPLY).Length - 1;
            operationCount += s.Split(Constants.DIVIDE).Length - 1;
            operationCount += s.Split(Constants.REMAINDER).Length - 1;

            return operationCount;
        }

        private static int GetValues(String s)
        {
            int value;

            if (!int.TryParse(s, out value))
                value = variables[s];

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

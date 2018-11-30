using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileParser
{
    class Program
    {
        static Dictionary<string, int> variables = new Dictionary<string, int>();
        enum operation {mult, rem, div};

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string path = @"D://file.txt";
            List<String> lines = new List<String>();
            
            try
            {
                String mark;
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                        //Console.WriteLine(line);
                    }
                }

                for(int i = 0; i<lines.Count; i++)
                {

                    if (lines[i].Split(' ')[0].Equals("if"))
                    {
                        mark = ParseIf(lines[i]);
                    } else
                    {
                        mark = ParseLine(lines[i]);
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

        private static String ParseIf(String s)
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

        private static String ParseLine(String s)
        {
            if (s.Contains("goto"))
            {
                return s.Split(' ')[1];
            }

            if (s.Contains("read"))
                ReadValue(s.Split(' ')[1]);
            if (s.Contains("print"))
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

        static int ParseExpression(String expr)
        {
            List<int> values = new List<int>();
           
            if (expr.Contains("("))
            {
                int start = expr.IndexOf('(')+1;
                int length;
                string buf = expr.Substring(start);
                string subexpr;

                int count = 1;
                while (count > 0)
                {
                    if (buf.IndexOf('(') < buf.IndexOf(')') && buf.IndexOf('(') > 0)
                    {
                        buf = buf.Substring(buf.IndexOf('(')+1);
                        count++;
                    }
                    else
                    {
                        buf = buf.Substring(buf.IndexOf(')')+1);
                        count--;
                    }
                }
                length = expr.Length - buf.Length - start -1;
                subexpr = expr.Substring(start, length);
                string replaceValue = ParseExpression(subexpr).ToString();
                if (replaceValue.Substring(0, 1).Equals("-"))
                    replaceValue = "0"+replaceValue;
                expr = expr.Replace("("+subexpr+")", replaceValue);
            }

            int operationCount = GetOperationCount(expr);

            if (operationCount > 1)
            {
                if (expr.Contains("+"))
                {
                    string[] paths = expr.Split('+');
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
                else if (expr.Contains("-"))
                {
                    string[] paths = expr.Split('-');
                    for (int i = 0; i < paths.Length; i++)
                    {
                        values.Add(ParseExpression(paths[i]));
                    }
                    int res = 0;
                    res = values[0];
                    for (int i = 1; i < paths.Length; i++)
                    {
                        res -= values[i];
                    }
                    return res;
                }
                else
                {
                    int mult, div, rem;
                    mult = expr.IndexOf('*');
                    div = expr.IndexOf('/');
                    rem = expr.IndexOf('%');
                    String buf = expr;
                    List<operation> order = new List<operation>();

                    while (GetOperationCount(buf) > 0)
                    {
                        if ((mult < div || div < 0) && (mult < rem || rem < 0) && mult > 0)
                        {
                            order.Add(operation.mult);
                            buf = buf.Substring(mult + 1);
                        }
                        if ((div < mult || mult < 0) && (div < rem || rem < 0) && div > 0)
                        {
                            order.Add(operation.div);
                            buf = buf.Substring(div + 1);
                        }
                        if ((rem < div || div < 0) && (rem < mult || mult < 0) && rem > 0)
                        {
                            order.Add(operation.rem);
                            buf = buf.Substring(rem + 1);
                        }

                        mult = buf.IndexOf('*');
                        div = buf.IndexOf('/');
                        rem = buf.IndexOf('%');
                    }

                    String[] vars = Regex.Split(expr, @"\W");
                    int res = GetValues(vars[0]);
                    for (int i = 0; i < vars.Length - 1; i++)
                    {
                        switch (order[i])
                        {
                            case operation.mult:
                                res *= GetValues(vars[i + 1]);
                                break;
                            case operation.div:
                                res /= GetValues(vars[i + 1]);
                                break;
                            case operation.rem:
                                res %= GetValues(vars[i + 1]);
                                break;
                        }
                    }
                    return res;
                }
            }
            else if (operationCount == 1)
            {
                if (expr.Contains("-"))
                {
                    if (expr.Substring(0, 1).Equals("-"))
                        return -GetValues(expr.Substring(1));

                    int a, b;
                    string[] paths = expr.Split('-');
                    a = GetValues(paths[0]);
                    b = GetValues(paths[1]);

                    return a - b;
                }
                else if (expr.Contains("+"))
                {
                    int a, b;
                    string[] paths = expr.Split('+');
                    a = GetValues(paths[0]);
                    b = GetValues(paths[1]);

                    return a + b;
                }
                else if (expr.Contains("*"))
                {
                    int a, b;
                    string[] paths = expr.Split('*');
                    a = GetValues(paths[0]);
                    b = GetValues(paths[1]);

                    return a * b;
                }
                else
                {
                    int a, b;
                    string[] paths = expr.Split('/');
                    a = GetValues(paths[0]);
                    b = GetValues(paths[1]);

                    return a / b;
                }
            }
            else return GetValues(expr);
        }

        private static int GetOperationCount(String s)
        {
            int operationCount = 0;
            operationCount += s.Split('+').Length - 1;
            operationCount += s.Split('-').Length - 1;
            operationCount += s.Split('*').Length - 1;
            operationCount += s.Split('/').Length - 1;
            operationCount += s.Split('%').Length - 1;

            return operationCount;
        }

        private static int GetValues(String s)
        {
            int value;

            try
            {
                value = int.Parse(s);
            }
            catch (Exception e)
            {
                value = variables[s];
            }

            return value;
        }

        static void PrintValue(String s)
        {
            try
            {
                Console.WriteLine(int.Parse(s));
            } catch (Exception e)
            {
                Console.WriteLine(variables[s]);
            }
        }

        static void ReadValue(String s)
        {
            String a = Console.ReadLine();
            try { 
                if (variables.ContainsKey(s))
                {
                    variables[s] = int.Parse(a);
                } else
                {
                    variables.Add(s, int.Parse(a));
                }
            } catch
            {
                Console.WriteLine("Ввод только интовыми числами!");
            }
        }

    }
}

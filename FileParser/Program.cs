using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileParser
{
    class Program
    {
        static int[][] points, tmp;
        static int[] offsets;
        static void Main(string[] args)
        {
            bool done = false;
            int[] vars = { 3, 4, 5, 7 };
            int repeatInterval = GetInterval(vars);     //вычисляем временной интервал запуска всех процессов одновременно
            points = new int[vars.Length][];
            for (int i = 0; i < vars.Length; i++)       //цикл вычисляющий все моменты времени запуска для каждого процесса
            {
                points[i] = new int[(repeatInterval * 2 / vars[i]) + 1];
                for (int j = 0; j < points[i].Length; j++)
                {
                    points[i][j] = vars[i] * (j);
                }
            }

            offsets = new int[vars.Length];

            tmp = points;
            while (offsets[0] < vars[0])
            {
                done = CheckIntersection(1, tmp[0]);
                if (done)
                {
                    break;
                }
                tmp = ResetValues(vars);
            }

            if (done)
            {
                for (int i = 0; i < offsets.Length; i++)
                {
                    Console.WriteLine($"Offset for process {i + 1}: {offsets[i]}s");
                }
            }
            else
            {
                Console.WriteLine("Offset can not be calculated");
            }
            Console.ReadLine();
        }

        private static bool CheckIntersection(int i, int[] mass)
        {
            int[] intersection = mass.Intersect(tmp[i]).ToArray();
            if (intersection.Length > 0)
            {
                if (i < points.Length - 1)
                {
                    return CheckIntersection(i + 1, intersection);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private static int[][] ResetValues(int[] vars)
        {
            int[][] tmp = points;

            offsets[offsets.Length - 1] += 1;
            for (int i = offsets.Length - 1; i > -1; i--)
            {
                if (offsets[i] == vars[i])
                {
                    offsets[i] = 0;
                    offsets[i - 1]++;
                }
            }

            for (int i = 0; i < offsets.Length; i++)
            {
                for (int j = 0; j < tmp[i].Length; j++)
                {
                    tmp[i][j] += offsets[i];
                }
            }
            return tmp;
        }

        private static int NOD(int m, int n)
        {
            int nod = 0;
            for (int i = 1; i < (n * m + 1); i++)
            {
                if (m % i == 0 && n % i == 0)
                {
                    nod = i;
                }
            }
            return nod;
        }

        private static int NOK(int n, int m)
        {
            return (n * m) / NOD(n, m);
        }

        private static int GetInterval(int[] vars)
        {
            int result = vars[0];
            for (int i = 1; i < vars.Length; i++)
            {
                result = NOK(result, vars[i]);
            }
            return result;
        }

        //static void Main(string[] args)
        //{
        //    Console.OutputEncoding = Encoding.UTF8;
        //    string path = @"D://file.txt";
        //    List<String> lines;

        //    try
        //    {
        //        String mark;

        //        lines = Utils.ReadFile(path);

        //        for(int i = 0; i<lines.Count; i++)
        //        {
        //            if (lines[i] == null || lines[i].Equals(""))
        //            {
        //                continue;
        //            }

        //            if (lines[i].Equals(Constants.END))
        //            {
        //                break;
        //            }

        //            if (lines[i].Split(' ')[0].Equals(Constants.IF))
        //            {
        //                mark = Utils.ParseIf(lines[i]);
        //            } else
        //            {
        //                mark = Utils.ParseLine(lines[i]);
        //            }

        //            if (mark != null)
        //            {
        //                for (int j = 0; j < lines.Count; j++)
        //                {
        //                    if (lines[j].Contains(mark) && !lines[j].Contains(Constants.GO_TO))
        //                    {
        //                        mark = null;
        //                        i = j;
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        Console.ReadKey();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Catch: "+e.ToString());
        //        Console.ReadKey();
        //    }
        //}
    }
}

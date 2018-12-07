using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class RPN
    {
        static private bool IsDelimeter(char c)
        {
            return (" ".IndexOf(c) != -1);
        }

        static private bool IsOperator(char с)
        {
            return ("+-/*%()".IndexOf(с) != -1);
        }

        static private byte GetPriority(char s)
        {
            switch (s)
            {
                case Constants.LEFT_HOOK: return 0;
                case Constants.RIGHT_HOOK: return 1;
                case Constants.PLUS: return 2;
                case Constants.MINUS: return 3;
                case Constants.REMAINDER:
                case Constants.DIVIDE:
                case Constants.MULTIPLY:
                    return 4;
                default: return 5;
            }
        }

        static public int Calculate(string input)
        {
            string output = GetExpression(input);
            return Counting(output);
        }

        static private string GetExpression(string input)
        {
            if (!input.Equals("") && input != null)
            {
                string output = string.Empty;
                Stack<char> operStack = new Stack<char>();

                for (int i = 0; i < input.Length; i++)
                {

                    if (IsDelimeter(input[i]))
                    {
                        continue;
                    }


                    if (Char.IsDigit(input[i]))
                    {

                        while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                        {
                            output += input[i];
                            i++;

                            if (i == input.Length) break;
                        }

                        output += " ";
                        i--;
                    }


                    if (IsOperator(input[i]))
                    {
                        if (input[i] == Constants.LEFT_HOOK)
                        {
                            operStack.Push(input[i]);
                        }
                        else if (input[i] == Constants.RIGHT_HOOK)
                        {

                            char s = operStack.Pop();

                            while (s != Constants.LEFT_HOOK)
                            {
                                output += s.ToString() + ' ';
                                s = operStack.Pop();
                            }
                        }
                        else
                        {
                            if (operStack.Count > 0)
                            {
                                if (GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                                {
                                    output += operStack.Pop().ToString() + " ";
                                }
                            }
                            operStack.Push(char.Parse(input[i].ToString()));

                        }
                    }
                }

                while (operStack.Count > 0)
                {
                    output += operStack.Pop() + " ";
                }

                return output;
            }
            else
            {
                return null;
            }
        }

        static private int Counting(string input)
        {
            if (!input.Equals("") && input != null)
            {
                int result = 0;
                Stack<int> temp = new Stack<int>();

                for (int i = 0; i < input.Length; i++)
                {

                    if (Char.IsDigit(input[i]))
                    {
                        string a = string.Empty;

                        while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                        {
                            a += input[i];
                            i++;
                            if (i == input.Length) break;
                        }
                        temp.Push(int.Parse(a));
                        i--;
                    }
                    else if (IsOperator(input[i]))
                    {

                        List<int> num = new List<int>();
                        num.Add(temp.Pop());
                        num.Add(temp.Pop());

                        result = Checker.CheckOperation(input[i], num).performAction();
                        temp.Push(result);
                    }
                }
                return temp.Peek();
            }
            else
            {
                return 0;
            }
        }
    }
}

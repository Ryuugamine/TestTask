using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class Checker
    {
        public static Operation check(string operation)
        {
            switch (operation)
            {
                case "+":
                    return new Add();
                case "-":
                    return new Subtract();
                case "/":
                    return new Divide();
                case "*":
                    return new Multiply();
                case "%":
                    return new Remainder();
            }
            return null;
        }
    }
}

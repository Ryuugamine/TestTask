using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class Checker
    {
        public static Operation check(char operation)
        {
            switch (operation)
            {
                case Constants.PLUS:
                    return new Add();
                case Constants.MINUS:
                    return new Subtract();
                case Constants.DIVIDE:
                    return new Divide();
                case Constants.MULTIPLY:
                    return new Multiply();
                case Constants.REMAINDER:
                    return new Remainder();
            }
            return null;
        }
    }
}

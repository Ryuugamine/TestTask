using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class Checker
    {
        public static Operation check(char operation, List<int> num)
        {
            switch (operation)
            {
                case Constants.PLUS:
                    return new Add(num[1], num[0]);
                case Constants.MINUS:
                    return new Subtract(num[1], num[0]);
                case Constants.DIVIDE:
                    return new Divide(num[1], num[0]);
                case Constants.MULTIPLY:
                    return new Multiply(num[1], num[0]);
                case Constants.REMAINDER:
                    return new Remainder(num[1], num[0]);
            }
            return null;
        }
    }
}

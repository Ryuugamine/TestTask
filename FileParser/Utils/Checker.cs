using FileParser.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class Checker
    {
        public static Operation CheckOperation(char operation, List<int> num)
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

        public static BoolValue CheckCondition(string operation, List<int> num)
        {
            switch (operation)
            {
                case Constants.EQUALLY:
                    return new Equally(num[0], num[1]);
                case Constants.NOT_EQUALLY:
                    return new NotEqually(num[0], num[1]);
                case Constants.MORE:
                    return new More(num[0], num[1]);
                case Constants.LESS:
                    return new Less(num[0], num[1]);
                case Constants.MORE_EQUALLY:
                    return new MoreEqually(num[0], num[1]);
                case Constants.LESS_EQUALLY:
                    return new LessEqually(num[0], num[1]);
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class Add : Operation
    {
        public override int performAction(int a, int b)
        {
            return a+b;
        }
    }
}

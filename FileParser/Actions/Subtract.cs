using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class Subtract : Operation
    {
        private int a, b;

        public Subtract(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public override int performAction()
        {
            return a-b;
        }
    }
}

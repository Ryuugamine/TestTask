using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Logic
{
    public class Equally : BoolValue
    {
        int a, b;
        public Equally (int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Check()
        {
            return a == b;
        }
    }
}

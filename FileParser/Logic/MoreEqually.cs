using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Logic
{
    public class MoreEqually : BoolValue
    {
        int a, b;
        public MoreEqually(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Check()
        {
            return a >= b;
        }
    }
}

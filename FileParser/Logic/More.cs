﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Logic
{
    public class More : BoolValue
    {
        int a, b;
        public More(int a, int b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Check()
        {
            return a > b;
        }
    }
}

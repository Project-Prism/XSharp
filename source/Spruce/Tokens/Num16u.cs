﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Spruce.Tokens {
    public class Num16u : Num {
        protected override object Check(string aText) {
            return UInt16.Parse(aText);
        }
    }
}

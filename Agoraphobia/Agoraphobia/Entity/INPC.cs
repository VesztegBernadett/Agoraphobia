﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Entity
{
    interface INPC
    {
        int Type { get; } // 0 - quest, 1 - trade, 2 - merchant, 3 - neutral, 4 - buff / debuff

    }
}
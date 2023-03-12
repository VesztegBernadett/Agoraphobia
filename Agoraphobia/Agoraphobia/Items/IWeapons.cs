using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Items
{
    internal interface IWeapons : IItemUsable
    {
        float Multiplier { get; set; }
        int Energy { get; set; }
    }
}

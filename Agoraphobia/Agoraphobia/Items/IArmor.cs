using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Items
{
    internal interface IArmor : IItemUsable
    {
        int Defense { get; }
        int Attack { get; }
    }
}

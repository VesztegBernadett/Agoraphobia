using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Items
{
    internal interface IConsumables : IItemUsable
    {
        int Energy { get; }
        int HP { get; }
        int Armor { get; }
        int Attack { get; }
        int Duration { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Items
{
    internal interface IConsumables : IItemUsable
    {
        int Energy { get; set; }
        int HP { get; set; }
        int Armor { get; set; }
        int Attack { get; set; }
    }
}

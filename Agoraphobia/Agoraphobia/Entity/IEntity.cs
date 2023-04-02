using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Character
{
    internal interface IEntity : IElement
    {
        int[] Coordinates { get; }
        List<IItem> Inventory { get; } // Inventory size is capped.
        int DreamCoins { get; }
    }
}

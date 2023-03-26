using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Rooms
{
    internal interface IRoom : IElement
    {
        List<IElement> Elements { get; }
        int Items { get; }
        List<IRoom> Exits { get; }
        byte Orientation { get; }  // 0 - positive / 1 - negative / 2 - neutral
        byte Type { get; } // 0 - basic, 1 - quest
        string View();
    }
}

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
        enum Orientation
        {
            Positive,
            Negative,
            Neutral
        };
        bool IsQuest { get; }
        string View();
    }
}

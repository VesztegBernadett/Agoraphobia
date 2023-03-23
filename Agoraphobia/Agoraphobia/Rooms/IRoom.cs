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
        int Orientation { get; }  // positive / negative / neutral
        string View();
    }
}

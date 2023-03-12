using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Rooms
{
    internal interface IRoom
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        int Orientation { get; }  // positive / negative / neutral

        string View();
    }
}

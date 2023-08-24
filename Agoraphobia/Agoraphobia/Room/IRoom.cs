using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Rooms
{
    internal interface IRoom : IElement
    {
        static List<IRoom> Rooms = new List<IRoom>();
        int NPC { get; } // Ids of NPCs in the room
        int Enemy { get; } // Ids of Enemies in the room
        List<int> Items { get; } // Ids of Items in the room
        List<int> Exits { get; }
    }
}

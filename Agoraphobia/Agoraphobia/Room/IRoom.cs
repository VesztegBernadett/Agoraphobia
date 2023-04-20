using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Rooms
{
    internal interface IRoom : IElement
    {
        List<int> NPCs { get; } // Ids of NPCs in the room
        List<int> Enemies { get; } // Ids of Enemies in the room
        List<int> Items { get; } // Ids of Items in the room
        List<int> Exits { get; }
        int ItemsNum { get; }
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

using Agoraphobia.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Entity
{
    interface INPC : IEntity, IArtist
    {
        enum Type
        {
            Quest,
            Trade,
            Merchant,
            Neutral,
            Buff,
            Debuff
        };
        static int[] Coordinates = { 5, 1 };
        static List<INPC> NPCs = new List<INPC>(); //Add the instance to this list in the constructor
        bool Interact();
    }
}

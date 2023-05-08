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
        static int[] Coordinates = { 10, 4 };
        static List<INPC> NPCs = new List<INPC>(); //Add the instance to this list in the constructor
        void Interact();
        public string Intro { get; }
    }
}

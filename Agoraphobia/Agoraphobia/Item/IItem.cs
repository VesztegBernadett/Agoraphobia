using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia
{
    internal interface IItem : IElement, IArtist
    {
        static int[] Coordinates = {80, 8};
        static List<IItem> Items = new List<IItem>();//Add the instance to this list in the constructor
        void PickUp();
        string Inspect();
        void Drop();
        void Delete();
        enum Rarity
        {
            Common,
            Uncommon,
            Rare,
            Epic,
            Legendary,
            Fabled
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia
{
    internal interface IItem : IElement
    {
        //static int[] Coordinates { get; }
        void PickUp();
        string Inspect();
        void Drop();
        void Delete();
        //enum Rarity
        //{
        //    Common,
        //    Uncommon,
        //    Rare,
        //    Epic,
        //    Legendary,
        //    Fabled
        //};
    }
}

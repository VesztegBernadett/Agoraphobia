using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia
{
    internal interface IItem : IElement
    {
        int[] Coordinates { get; }
        string Type { get; set; }
        void PickUp();
        string Inspect();
        void Drop();
        void Delete();
    }
}

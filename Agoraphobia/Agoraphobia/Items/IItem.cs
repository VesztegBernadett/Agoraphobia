using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia
{
    internal interface IItem
    {
        string Name { get; }
        string Description { get; }
        string Inspect();
        void Drop();
        void Delete();
    }
}

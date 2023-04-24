using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia
{
    internal interface IElement
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        static string PATH = "../../../Files/";
    }
}

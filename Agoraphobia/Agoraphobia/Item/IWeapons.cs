using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Items
{
    internal interface IWeapons : IItem
    {
        double MinMultiplier { get; }
        double MaxMultiplier { get; }
        int Energy { get; }
    }
}

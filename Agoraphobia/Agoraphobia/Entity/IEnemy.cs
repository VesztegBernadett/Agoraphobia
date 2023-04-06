using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Entity
{
    internal interface IEnemy : IAttackable
    {
        string Type { get; }
        //static List<IEnemy> Enemies { get; } //Add the instance to this list in the constructor
        Dictionary<int, float> DropRate { get; }
    }
}

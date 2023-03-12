using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Character
{
    internal interface IEntity
    {
        string Name { get; }
        string Description { get; }
        List<IItem> Inventory { get; set; }
        int Armor { get; set; }
        int HP { get; set; }
        int Energy { get; set; }
        int AttackDamage { get; set; }
    }
}

using Agoraphobia.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Entity
{
    internal interface IAttackable : IEntity
    {
        int Armor { get; }
        int HP { get; }
        int Energy { get; }
        int AttackDamage { get; }
        int Sanity { get; }
        void Attack(IAttackable target);
    }
}

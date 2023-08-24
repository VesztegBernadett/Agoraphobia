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
        int Defense { get; }
        int HP { get; set; }
        int MaxHP { get; set; }
        int AttackDamage { get; }
        int Sanity { get; }
        int DreamCoins { get; }
        void Attack();//We only use IAttackable for Enemy so we know that we attack the player
    }
}

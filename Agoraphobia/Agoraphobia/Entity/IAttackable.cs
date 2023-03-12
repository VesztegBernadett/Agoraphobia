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
        bool Attackable { get; set;  }
        void Attack();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Entity
{
    internal interface IPlayer : IAttackable
    {
        void Death();
        void Respawn();
        void ChangeSanity(int amount);
        int DreamCoins { get; }

        // possible improvements - hardcore mode
    }
}

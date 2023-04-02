using Agoraphobia.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Entity
{
    interface INPC : IEntity
    {
        //enum Type
        //{
        //    Quest,
        //    Trade,
        //    Merchant,
        //    Neutral,
        //    Buff,
        //    Debuff
        //};
        bool Interact();
    }
}

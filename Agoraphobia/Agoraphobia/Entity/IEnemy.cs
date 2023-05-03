using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Entity
{
    internal interface IEnemy : IAttackable, IArtist
    {
        static int[] Coordinates = { 45, 4}; 
        static List<IEnemy> Enemies = new List<IEnemy>(); //Add the instance to this list in the constructor
        Dictionary<int, double> DropRate { get; }
    }
}

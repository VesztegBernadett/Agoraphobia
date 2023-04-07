using Agoraphobia.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Consumable consumable = new Consumable("Consumable0.txt");
            Console.WriteLine(consumable.Name);
            Console.WriteLine(consumable.Description);
        }
    }
}

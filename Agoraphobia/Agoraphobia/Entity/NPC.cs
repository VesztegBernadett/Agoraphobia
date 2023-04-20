using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Agoraphobia.Items;
using static Agoraphobia.IItem;
using static Agoraphobia.Items.IArmor;
using System.Collections;
using System.Security.AccessControl;

namespace Agoraphobia.Entity
{
    internal class NPC : INPC
    {
        private static Random r = new Random();
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;
        public string Description { get => description; }
        public List<int> Inventory { get; set; }
        private readonly int dreamCoins;
        public int DreamCoins { get => dreamCoins; }
        public bool Interact()
        {
            return true;
        }

        public NPC(int id, string name, string desc, int coins, List<int> items)
        {
            this.id = id;
            this.name = name;
            description = desc;
            dreamCoins = coins;
            INPC.NPCs.Add(this);
            Inventory = items;
           
        }
    }
}

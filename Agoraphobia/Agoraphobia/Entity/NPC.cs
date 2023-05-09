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
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;

        public string Art { get; private set; }
        public string Description { get => description; }
        public List<int> Inventory { get; set; }
        private readonly int dreamCoins;
        public int DreamCoins { get => dreamCoins; }
        public string Intro { get; private set; }
        public void Interact()
        {
            Viewport.Shop(id);
        }
        public NPC(int id, string name, string desc, int coins, List<int> items, string intro)
        {
            this.id = id;
            this.name = name;
            description = desc;
            dreamCoins = coins;
            INPC.NPCs.Add(this);
            Inventory = items;
            Intro = intro;
            Art = File.ReadAllText($"{IElement.PATH}/Arts/NArt{id}.txt");
        }
    }
}

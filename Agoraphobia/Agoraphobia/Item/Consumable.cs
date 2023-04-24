using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using static Agoraphobia.IItem;
using System.Collections;

namespace Agoraphobia.Items
{
    internal class Consumable : IConsumables
    {
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;

        public string art { get; private set; }
        public string Description { get => description; }
        public int Energy { get; private set; }
        public int HP { get; private set; }
        public int Armor { get; private set; }
        public int Attack { get; private set; }
        public int Duration { get; private set; }
        public IItem.Rarity Rarity { get; private set; }
        public Consumable(int id, string name, string desc, int energy, int hp, int attack, int armor, int duration, int rarity)
        {
            this.id = id;
            this.name = name;
            description = desc;
            Armor = armor;
            Attack = attack;
            Energy = energy;
            HP = hp;
            Duration = duration;
            Rarity = (IItem.Rarity)rarity;
            IItem.Items.Add(this);
            art = File.ReadAllText($"{IElement.PATH}/Arts/IArt{id}.txt");
        }

        public void Use()
        {

        }

        public void PickUp()
        {

        }

        public string Inspect()
        {
            return $"";
        }

        public void Drop()
        {

        }

        public void Delete()
        {

        }
    }
}

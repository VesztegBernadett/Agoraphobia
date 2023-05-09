using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using static Agoraphobia.IItem;
using Agoraphobia.Entity;

namespace Agoraphobia.Items
{
    internal class Weapon : IWeapons
    {
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;

        public string Art { get; private set; }
        public string Description { get => description; }
        public double MinMultiplier { get; private set; }
        public double MaxMultiplier { get; private set; }

        public int Energy { get; private set; }
        public ItemRarity Rarity { get; set; }
        public int Price { get; private set; }
        public void Use()
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
        public Weapon(int id, string name, string desc, double minMultiplier, double maxMultiplier, int energy, int rarity, int price)
        {
            this.id = id;
            this.name = name;
            description = desc;
            MinMultiplier = minMultiplier;
            MaxMultiplier = maxMultiplier;
            Energy = energy;
            Rarity = (ItemRarity)rarity;
            Price = price;
            IItem.Items.Add(this);
            Art = File.ReadAllText($"{IElement.PATH}/Arts/IArt{id}.txt");
        }
    }
}

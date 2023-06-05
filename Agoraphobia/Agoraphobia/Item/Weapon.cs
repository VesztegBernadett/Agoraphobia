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
        private static readonly Random random = new Random();
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
            int r = random.Next(0, 10);
            if (r == 1 || r == 2)
            {
                this.id = id;
                this.name = "Rare " + name;
                description = "It's a rare item. " + desc;
                MinMultiplier = minMultiplier + 1;
                MaxMultiplier = maxMultiplier + 1;
                if (energy > 1)
                {
                    Energy = energy - 1;
                }
                else { Energy = energy; }
                if (rarity > 0 && rarity < 6)
                {
                    Rarity = (ItemRarity)rarity + 1;
                }
                else
                {
                    Rarity = (ItemRarity)rarity;
                }
                Price = price + 100;
                IItem.Items.Add(this);
                Art = File.ReadAllText($"{IElement.PATH}/Arts/IArt{id}.txt");
            }
            else if (r == 3)
            {
                this.id = id;
                this.name = "Clumsy " + name;
                description = "It's a dull item. " + desc;
                MinMultiplier = minMultiplier - 1;
                MaxMultiplier = maxMultiplier;
                Energy = energy;
                if (rarity > 0)
                {
                    Rarity = (ItemRarity)rarity - 1;
                }
                else
                {
                    Rarity = (ItemRarity)rarity;
                }
                Price = price - 50;
                IItem.Items.Add(this);
                Art = File.ReadAllText($"{IElement.PATH}/Arts/IArt{id}.txt");
            }
            else if (r == 4)
            {
                this.id = id;
                this.name = "Cheap " + name;
                description = "This item is on sale! " + desc;
                MinMultiplier = minMultiplier;
                MaxMultiplier = maxMultiplier;
                Energy = energy;
                Rarity = (ItemRarity)rarity;
                if (price > 200)
                {
                    Price = price - 200;
                }
                else
                {
                    Price = price - 50;
                }
                IItem.Items.Add(this);
                Art = File.ReadAllText($"{IElement.PATH}/Arts/IArt{id}.txt");
            }
            else
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
}

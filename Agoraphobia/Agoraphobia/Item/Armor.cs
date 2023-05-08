using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Agoraphobia.IItem;

namespace Agoraphobia.Items
{
    internal class Armor : IArmor
    {
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;

        public string Art { get; private set; }
        public string Description { get => description; }
        public int Defense { get; private set; }
        public int Attack { get; private set; }
        public ItemRarity Rarity { get; set; }
        public IArmor.Armorpiece Armorpiece { get; private set; }
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
        public Armor(int id, string name, string desc, int def, int attack, int piece, int rarity)
        {
            this.id = id;
            this.name = name;
            description = desc;
            Defense = def;
            Attack = attack;
            Rarity = (ItemRarity)rarity;
            Armorpiece = (IArmor.Armorpiece)piece;
            IItem.Items.Add(this);
            Art = File.ReadAllText($"{IElement.PATH}/Arts/IArt{id}.txt");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Agoraphobia.Items
{
    internal class Armor : IArmor
    {
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;
        public string Description { get => description; }
        public int Defense { get; private set; }
        public int Attack { get; private set; }
        public IItem.Rarity Rarity { get; private set; }
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

        public Armor(int id, string name, string desc, int def, int attack, int rarity)
        {
            this.id = id;
            this.name = name;
            this.description = desc;
            this.Defense = def;
            this.Attack = attack;
            this.Rarity = (IItem.Rarity)rarity;
            IItem.Items.Add(this);
        }
    }
}

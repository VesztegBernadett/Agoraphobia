using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Agoraphobia.Items
{
    internal class Weapon : IWeapons
    {
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;

        public string art { get; private set; }
        public string Description { get => description; }
        public double Multiplier { get; private set; }
        public int Energy { get; private set; }
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
        public void Show()
        {
            List<string> rows = art.Split('\n').ToList();
            for(int i = 0; i<rows.Count(); i++)
            {
                Console.SetCursorPosition(IItem.Coordinates[0], IItem.Coordinates[1]+i);
                Console.Write(rows[i]);
            }
        }
        public Weapon(int id, string name, string desc, double multiplier, int energy, int rarity)
        {
            this.id = id;
            this.name = name;
            description = desc;
            Multiplier = multiplier;
            Energy = energy;
            Rarity = (IItem.Rarity)rarity;
            IItem.Items.Add(this);
            art = File.ReadAllText($"{IElement.PATH}/Arts/IArt{id}.txt");
        }
    }
}

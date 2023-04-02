using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace Agoraphobia.Items
{
    internal class Consumable
    {
        static readonly int[] Coordinates = { 10, 25 };
        private int id;
        public int Id { get => id; }
        private string name;
        public string Name { get => name; }
        private string description;
        public string Description { get => description; }
        public int Energy { get; private set; }
        private int hp;
        public int HP { get => hp; }
        private int armor;
        public int Armor { get => armor; }
        private int attack;
        public int Attack { get => attack; }
        private int duration;
        public int Duration { get => duration; }
        public Consumable(string filename)
        {
            foreach (var line in File.ReadAllLines(filename, Encoding.UTF8))
            {
                string[] data = line.Split('#');
                switch (data[1])
                {
                    case "Id":
                        id = int.Parse(data[0]);
                        break;
                    case "Name":
                        name = data[0];
                        break;
                    case "Description":
                        description = data[0];
                        break;
                    case "Energy":
                        Energy = int.Parse(data[0]);
                        break;
                    case "HP":
                        hp = int.Parse(data[0]);
                        break;
                    case "Armor":
                        armor = int.Parse(data[0]);
                        break;
                    case "Attack":
                        attack = int.Parse(data[0]);
                        break;
                    case "Duration":
                        duration = int.Parse(data[0]);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

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
        private int id;
        public int Id { get => id; }
        private string name;
        public string Name { get => name; }
        private string description;
        public string Description { get => description; }
        public int Energy { get; private set; }
        public int HP { get; private set; }
        public int Armor { get; private set; }
        public int Attack { get; private set; }
        public int Duration { get; private set; }
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
                        HP = int.Parse(data[0]);
                        break;
                    case "Armor":
                        Armor = int.Parse(data[0]);
                        break;
                    case "Attack":
                        Attack = int.Parse(data[0]);
                        break;
                    case "Duration":
                        Duration = int.Parse(data[0]);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

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
        private int id;
        public int Id { get => id; }
        private string name;
        public string Name { get => name; }
        private string description;
        public string Description { get => description; }
        public int Defense { get; private set; }
        public int Attack { get; private set; }
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

        public Armor(string filename)
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
                    case "Defense":
                        Defense = int.Parse(data[0]);
                        break;
                    case "Attack":
                        Attack = int.Parse(data[0]);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

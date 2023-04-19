using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Agoraphobia.Entity
{
    internal class NPC : INPC
    {
        private static Random r = new Random();
        private int id;
        public int Id { get => id; }
        private string name;
        public string Name { get => name; }
        private string description;
        public string Description { get => description; }
        public List<int> Inventory { get; set; }
        private int dreamCoins;
        public int DreamCoins { get => dreamCoins; }
        public bool Interact()
        {
            return true;
        }

        public NPC(string filename)
        {
            Inventory = new List<int>();
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
                    case "Inventory":
                        foreach (var item in data[0].Split(';'))
                        {
                            int _;
                            string[] curr = item.Split('(');
                            if (int.TryParse(curr[0], out _))
                                Inventory.Add(int.Parse(curr[0]));
                            else
                            {
                                int[] interval = Array.ConvertAll(curr[1].Split('-'), int.Parse);
                                dreamCoins = r.Next(interval[0], interval[1] + 1);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

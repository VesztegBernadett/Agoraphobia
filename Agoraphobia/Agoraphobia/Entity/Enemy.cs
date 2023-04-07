using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Agoraphobia.Entity
{
    internal class Enemy : IEnemy
    {
        private static Random r = new Random(); 
        private int id;
        public int Id { get => id; }
        private string name;
        public string Name { get => name; }
        private string description;
        public string Description { get => description; }
        private int dreamCoins;
        public int DreamCoins { get => dreamCoins; }
        public Dictionary<int, float> DropRate { get; private set; }
        private int sanity;
        public int Sanity { get => sanity; }
        public int Defense { get; private set; }
        public int HP { get; private set; }
        public int Energy { get; private set; }
        public int AttackDamage { get; private set; }
        public List<int> Inventory { get; set; }
        public void Attack(IAttackable target)
        {

        }
        public Enemy(string filename)
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
                    case "Inventory":
                        foreach (var item in data[0].Split(';'))
                        {
                            int _;
                            string[] curr = item.Split('(');
                            if (int.TryParse(curr[0], out _))
                            {
                                DropRate.Add(int.Parse(curr[0]), float.Parse(curr[1]));
                                Inventory.Add(int.Parse(curr[0]));
                            }
                            else
                            {
                                int[] interval = Array.ConvertAll(curr[1].Split('-'), int.Parse);
                                dreamCoins = r.Next(interval[0], interval[1] + 1);
                            }
                        }
                        break;
                    case "Defense":
                        Defense = int.Parse(data[0]);
                        break;
                    case "HP":
                        HP = int.Parse(data[0]);
                        break;
                    case "Energy":
                        Energy = int.Parse(data[0]);
                        break;
                    case "AttackDamage":
                        AttackDamage = int.Parse(data[0]);
                        break;
                    case "Sanity":
                        sanity = int.Parse(data[0]);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

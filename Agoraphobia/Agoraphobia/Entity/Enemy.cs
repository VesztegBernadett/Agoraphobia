using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Agoraphobia.IItem;
using System.Collections;


namespace Agoraphobia.Entity
{
    internal class Enemy : IEnemy
    {
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;
        public string Description { get => description; }
        private readonly int dreamCoins;
        public int DreamCoins { get => dreamCoins; }
        public Dictionary<int, float> DropRate { get; private set; }
        private readonly int sanity;
        public int Sanity { get => sanity; }
        public int Defense { get; private set; }
        public int HP { get; private set; }
        public int Energy { get; private set; }
        public int AttackDamage { get; private set; }
        public List<int> Inventory { get; set; }
        public void Attack(IAttackable target)
        {

        }
        public Enemy(int id, string name, string desc, int def, int attack, int sanity, int hp, int energy, int coins, List<int> items, List<float> rates)
        {
            DropRate = new Dictionary<int, float>();
            Inventory = items;
            this.id = id;
            this.name = name;
            description = desc;
            Defense = def;
            AttackDamage = attack;
            this.sanity = sanity;
            HP = hp;
            Energy = energy;
            dreamCoins = coins;
            for (int i = 0; i < items.Count; i++)
                DropRate.Add(items[i], rates[i]);
            IEnemy.Enemies.Add(this);
        }
    }
}

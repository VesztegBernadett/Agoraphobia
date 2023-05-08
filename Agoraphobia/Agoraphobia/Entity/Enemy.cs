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

        public string Art { get; private set; }
        public string Description { get => description; }
        private readonly int dreamCoins;
        public int DreamCoins { get => dreamCoins; }
        public Dictionary<int, double> DropRate { get; private set; }
        private readonly int sanity;
        public int Sanity { get => sanity; }
        public int Defense { get; private set; }

        private int hp;
        public int HP 
        {
            get => hp;
            set
            {
                if (value <= 0)
                    Death();
                else hp = value;
            }
        }
        public int Energy { get; private set; }
        public int AttackDamage { get; private set; }
        public List<int> Inventory { get; set; }
        public void Attack()
        {
            Player.ChangeHP(-(AttackDamage-Player.Defense));
            Player.ChangeEnergy(int.Parse(Math.Ceiling(Player.MAXENERGY * 0.1).ToString()));
            if (Player.HP > 0)
            {
                Player.Attack(this);
            }
            else
            {
                Player.ChangeSanity(-Sanity);
            }
        }
        public Enemy(int id, string name, string desc, int def, int attack, int sanity, int hp, int energy, int coins, List<int> items, List<double> rates)
        {
            DropRate = new Dictionary<int, double>();
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
            Art = File.ReadAllText($"{IElement.PATH}/Arts/EArt{id}.txt");
        }

        public void Death()
        {
            Random r = new Random();
            Player.ChangeSanity(Sanity);
            Player.ChangeCoins(DreamCoins);
            foreach (int item in Inventory)
            {
                if (r.Next()<=DropRate[item])
                {
                    Player.Inventory.Add(item);
                }
            }
            Viewport.Message($"{Name} is dead.");//Add what loot you get from the enemy
            //Need to somehow get back to the Main scene
        }
    }
}

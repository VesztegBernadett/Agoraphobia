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
        private static readonly Random random = new Random();
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;
        public int MaxHP { get; set; }
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
            Player.ChangeEnergy(Player.MaxEnergy);
            if (Player.EffectDuration > 1)
            {
                Player.EffectDuration--;
            }
            else if (Player.EffectDuration == 1)
            {
                Player.ChangeAttack(-Player.ChangedAttack);
                Player.ChangeDefense(-Player.ChangedDefense);
                Player.EffectDuration--;
            }
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
            int r = random.Next(5);
            if (r == 1)
            {
                DropRate = new Dictionary<int, double>();
                Inventory = items;
                this.id = id;
                this.name = "Tough "+name;
                description = "This one is tougher! "+desc;
                Defense = def+2;
                AttackDamage = attack+2;
                this.sanity = sanity+10;
                MaxHP = hp+5;
                HP = hp+5;
                Energy = energy;
                dreamCoins = coins+100;
                for (int i = 0; i < items.Count; i++)
                    DropRate.Add(items[i], rates[i]);
                IEnemy.Enemies.Add(this);
                Art = File.ReadAllText($"{IElement.PATH}/Arts/EArt{id}.txt");
            }
            else if (r == 2)
            {
                DropRate = new Dictionary<int, double>();
                Inventory = items;
                this.id = id;
                this.name = "Weak "+name;
                description = "This one is weaker! "+desc;
                if (def != 0)
                {
                    Defense = def - 1;
                }
                else
                {
                    Defense = def;
                }
                if (attack > 5)
                {
                    AttackDamage = attack-2;
                }
                else
                {
                    AttackDamage = attack;
                }
                if (sanity > 10)
                {
                    this.sanity = sanity - 10;
                }
                else
                {
                    this.sanity = sanity;
                }
                MaxHP = hp-5;
                HP = hp-5;
                Energy = energy;
                if (coins > 50)
                {
                    dreamCoins = coins - 50;
                }
                else
                {
                    dreamCoins = coins;
                }
                for (int i = 0; i < items.Count; i++)
                    DropRate.Add(items[i], rates[i]);
                IEnemy.Enemies.Add(this);
                Art = File.ReadAllText($"{IElement.PATH}/Arts/EArt{id}.txt");
            }
            else
            {
                DropRate = new Dictionary<int, double>();
                Inventory = items;
                this.id = id;
                this.name = name;
                description = desc;
                Defense = def;
                AttackDamage = attack;
                this.sanity = sanity;
                MaxHP = hp;
                HP = hp;
                Energy = energy;
                dreamCoins = coins;
                for (int i = 0; i < items.Count; i++)
                    DropRate.Add(items[i], rates[i]);
                IEnemy.Enemies.Add(this);
                Art = File.ReadAllText($"{IElement.PATH}/Arts/EArt{id}.txt");
            }
        }

        public void Death()
        {
            Random r = new Random();
            Player.ChangeSanity(Sanity);
            Player.ChangeCoins(DreamCoins);
            foreach (int item in Inventory)
            {
                if (r.NextDouble()<=DropRate[item] && Player.InventoryLength < 18)
                {
                    Player.Inventory.Add(item);
                }
            }
            Viewport.Message($"{Name} is dead.");//Add what loot you get from the enemy
            Program.room.RemoveEnemy();
            hp = MaxHP;
            Player.ChangeEnergy(1);
            Program.MainScene(); //Need to somehow get back to the Main scene
        }
    }
}

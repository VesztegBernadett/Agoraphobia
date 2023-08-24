using Agoraphobia.Items;
using Agoraphobia.Rooms;

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
        private readonly int minDreamCoins;
        public int MinDreamCoins { get => minDreamCoins; }
        private readonly int maxDreamCoins;
        public int MaxDreamCoins { get => maxDreamCoins; }
        public int DreamCoins { get => random.Next(minDreamCoins, maxDreamCoins + 1); }
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
        public int AttackDamage { get; private set; }
        public List<int> Inventory { get; set; }
        public void Attack()
        {
            Player.ChangeHP(-(AttackDamage-Player.Defense));
            Player.ChangeEnergy(Player.MaxEnergy);
            if (Player.EffectDuration > 1)
                Player.EffectDuration--;
            else if (Player.EffectDuration == 1)
            {
                Player.ChangeAttack(-Player.ChangedAttack);
                Player.ChangeDefense(-Player.ChangedDefense);
                Player.EffectDuration--;
            }
            if (Player.HP > 0)
                Player.Attack(this, ref Program.inventory);
            else Player.ChangeSanity(-2 * Sanity);
        }
        public Enemy(int id, string name, string desc, int def, int attack, int sanity, int hp, int mincoins, int maxcoins, List<int> items, List<double> rates)
        {
            int r = random.Next(5);
            DropRate = new Dictionary<int, double>();
            Inventory = items;
            this.id = id;
            if (Program.newGame)
            {
                if (r == 1)
                {
                    this.name = "Tough " + name;
                    description = "This one is tougher! " + desc;
                    Defense = def + 2;
                    AttackDamage = attack + 2;
                    this.sanity = sanity + 10;
                    MaxHP = hp + 5;
                    HP = hp + 5;
                    minDreamCoins = mincoins + 100;
                    maxDreamCoins = maxcoins + 100;
                }
                else if (r == 2)
                {
                    this.name = "Weak " + name;
                    description = "This one is weaker! " + desc;
                    if (def != 0)
                        Defense = def - 1;
                    else Defense = def;
                    if (attack > 5)
                        AttackDamage = attack - 2;
                    else AttackDamage = attack;
                    if (sanity > 10)
                        this.sanity = sanity - 10;
                    else this.sanity = sanity;
                    MaxHP = hp - 5;
                    HP = hp - 5;
                    if (mincoins > 50)
                    {
                        minDreamCoins = mincoins - 50;
                        maxDreamCoins = maxcoins - 50;
                    }
                    else
                    {
                        minDreamCoins = mincoins;
                        maxDreamCoins = maxcoins;
                    }
                }
                else
                {
                    this.name = name;
                    description = desc;
                    Defense = def;
                    AttackDamage = attack;
                    this.sanity = sanity;
                    MaxHP = hp;
                    HP = hp;
                    minDreamCoins = mincoins;
                    maxDreamCoins = maxcoins;
                }
            }
            else
            {
                this.name = name;
                description = desc;
                Defense = def;
                AttackDamage = attack;
                this.sanity = sanity;
                MaxHP = hp;
                HP = hp;
                minDreamCoins = mincoins;
                maxDreamCoins = maxcoins;
            }
            for (int i = 0; i < items.Count; i++)
                DropRate.Add(items[i], rates[i]);
            IEnemy.Enemies.Add(this);
            Art = File.ReadAllText($"{Program.PATH}/Arts/EArt{id}.txt");
        }

        public void Death()
        {
            Random r = new Random();
            Player.ChangeSanity(Sanity);
            Player.ChangeCoins(DreamCoins);
            foreach (int item in Inventory)
            {
                if (r.NextDouble() <= DropRate[item] && Player.InventoryLength < 18)
                {
                    IItem current = IItem.Items.Find(x => x.Id == item);
                    if (current.GetType().ToString() == "Agoraphobia.Items.Weapon")
                    {
                        Weapon weapon = (Weapon)current;
                        weapon.Obtain();
                    }
                    else Player.Inventory.Add(item);
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

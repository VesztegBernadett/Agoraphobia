using Agoraphobia.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static Agoraphobia.IItem;

namespace Agoraphobia.Entity
{
    internal class Player
    {
        public static int Defense { get; private set; } = 3;
        public static int MaxHP { get; private set; } = 15;
        private static int hp = 15;
        public static int HP
        {
            get => hp;
            private set
            {
                if (value > MaxHP)
                    hp = MaxHP;
                else if (value < 0)
                    Death();
                else hp = value;
            }
        }
        public const int MAXENERGY = 3;
        private static int energy = 3;
        public static int Energy
        {
            get => energy;
            private set
            {
                if (value < 0)
                    energy = 0;
                else if (value > MAXENERGY)
                    energy = MAXENERGY;
                else energy = value;
            }
        }
        private static int attack = 2;
        public static int AttackDamage
        {
            get => attack;
            private set
            {
                if (value < 0)
                    attack = 0;
                else attack = value;
            }
        }
        private static int sanity = 50;
        public static int Sanity
        {
            get => sanity;
            private set
            {
                if (value <= 0)
                    WakeUp();
                else if (value >= 100)
                    GoInsane();
                else sanity = value;
            }
        }
        public static List<int> Inventory { get; private set; } = new List<int>();
        public static int DreamCoins { get; private set; } = 0;
        public static string Name { get; private set; } = "asdasd";

        public static DateTime playTimeStart;
        private static int score = 0;
        private static int roomcount = 0;

        public static void Attack(IEnemy target)
        {
            Random r = new Random();
            Console.Clear();
            Console.SetCursorPosition(50, 1);
            Console.WriteLine(target.Name);
            Console.Write(target.Art);
            Viewport.ShowGrid();
            int inventory=0;
            while (Energy>0&&target.HP>0)
            {
                Viewport.ShowStats();
                Viewport.ShowInventory(inventory);
                ConsoleKey input = Console.ReadKey(true).Key;

                switch (input)
                {
                    case ConsoleKey.LeftArrow:
                        if (inventory == 0)
                            inventory = Player.Inventory.Count - 1;
                        else inventory--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (inventory == Player.Inventory.Count - 1)
                            inventory = 0;
                        else inventory++;
                        break;
                    case ConsoleKey.Enter:
                        IItem selectedItem = IItem.Items.Find(x => x.Id == Inventory[inventory]);
                        if (selectedItem.GetType().ToString() =="Agoraphobia.Items.Weapon")
                        {
                            Weapon selectedWeapon = (Weapon)selectedItem;
                            if (selectedWeapon.Energy<=Energy)
                            {
                                Energy -= selectedWeapon.Energy;
                                target.HP -= Convert.ToInt32(AttackDamage * (r.NextDouble() * (selectedWeapon.MaxMultiplier - selectedWeapon.MinMultiplier) + selectedWeapon.MinMultiplier))-target.Defense;
                            }
                        }
                        break;
                }
            }
            if (target.HP > 0)
            {
                target.Attack();
            }
        }

        public static void Death()
        {
            Viewport.Message("You are dead.");
        }

        public static void GoInsane()
        {

        }

        //TODO: DESIGN
        public static void WakeUp()
        {
            // TODO: A "victory screen"-t meg kell designolni, egyenlőre csak törli az összes ui-t és kiírja a játékidőt és pontszámot amit itemekből számol ki.

            Console.Clear();
            Console.SetCursorPosition(0, 0);

            DateTime playTimeEnd = DateTime.UtcNow;
            TimeSpan playTime = playTimeEnd - playTimeStart;
            Console.WriteLine("játékidő: {0:hh} óra {0:mm} perc {0:ss} másodperc", playTime);
            
            Dictionary<ItemRarity, int> itemValue = new Dictionary<ItemRarity, int>() {
                {ItemRarity.Common, 1 },
                {ItemRarity.Uncommon, 2 },
                {ItemRarity.Rare, 3 },
                {ItemRarity.Epic, 4 },
                {ItemRarity.Legendary, 5 },
                {ItemRarity.Fabled, 6 },
            };

            foreach (int itemID in Player.Inventory){
                IItem i = IItem.Items.Find(item => item.Id == itemID);
                score += itemValue[i.Rarity] * 10;
            }

            Console.WriteLine($"Elért pontszám: {score}");
        }

        public static void Respawn()
        {

        }

        public static void ChangeHP (int amount) => HP += amount;
        public static void ChangeEnergy (int amount) => Energy += amount;
        public static void ChangeSanity (int amount) => Sanity += amount;
        public static void ChangeMaxHP (int amount) => MaxHP += amount;
        public static void ChangeAttack (int amount) => AttackDamage += amount;
        public static bool ChangeCoins(int amount) 
        {
            if (DreamCoins + amount < 0)
                return false;
            else
            {
                DreamCoins += amount;
                return true;
            }
        }
        public static void ChangeDefense (int amount) => Defense += amount;
        public static void ChangeName(string name) => Name = name;
    }
}

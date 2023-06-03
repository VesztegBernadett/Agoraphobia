using Agoraphobia.Items;
using Agoraphobia.Rooms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static Agoraphobia.IItem;
using System.IO;

namespace Agoraphobia.Entity
{
    internal class Player
    {
        public static int Defense { get; private set; } = 3;
        public static int MaxHP { get; private set; } = 15;
        private static int hp = 15;
        public static int Slot { get; set; }
        public static int Points = 0;

        public static int EffectDuration = 0;
        public static int ChangedDefense = 0;
        public static int ChangedAttack = 0;
        public static int HP
        {
            get => hp;
            private set
            {
                if (value > MaxHP)
                    hp = MaxHP;
                else if (value < 0)
                {
                    hp = 0;
                    Death();
                }
                else hp = value;
            }
        }
        public static int MaxEnergy = 3;
        private static int energy = 3;
        public static int Energy
        {
            get => energy;
            private set
            {
                if (value < 0)
                    energy = 0;
                else if (value > MaxEnergy)
                    energy = MaxEnergy;
                else energy = value;
            }
        }
        private static int attack = 3;
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
                    GoInsane();
                else if (value >= 100)
                    WakeUp();
                else sanity = value;
            }
        }
        public static List<int> Inventory { get; set; } = new List<int>();
        public static int DreamCoins { get; private set; } = 100;
        public static string Name { get; private set; } = "asdasd";

        public static DateTime playTimeStart;
        
        public static void Attack(IEnemy target)
        {
            Random r = new Random();
            Console.SetCursorPosition((120 - target.Name.Length) / 2, 1);
            Console.WriteLine(target.Name);
            Console.Write(target.Art);
            Viewport.ShowGrid();
            int inventory=0;

            while (Energy>0&&target.HP>0)
            {
                Console.Clear();
                Console.SetCursorPosition((120 - target.Name.Length - 3 - target.MaxHP.ToString().Length - target.HP.ToString().Length) / 2, 1);
                Console.WriteLine($"{target.Name}, {target.HP} / {target.MaxHP}");
                List<string> rows = target.Art.Split('\n').ToList();
                int TargetArtLength = 80 - rows[0].Length;
                for (int i = 0; i < rows.Count(); i++)
                {
                    Console.SetCursorPosition(TargetArtLength, 3 + i);
                    Console.Write(rows[i]);
                }
                Viewport.ShowGrid();
                Viewport.ShowStats();
                Viewport.ShowInventory(inventory);
                Viewport.ShowItemInfo(inventory);
                if (Console.KeyAvailable)
                    Console.ReadKey(true);
                ConsoleKey input = Console.ReadKey(true).Key;

                switch (input)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.LeftArrow:
                        if (inventory == 0)
                            inventory = Player.Inventory.Count - 1;
                        else inventory--;
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.RightArrow:
                        if (inventory == Player.Inventory.Count - 1)
                            inventory = 0;
                        else inventory++;
                        break;
                    case ConsoleKey.Enter:
                        IItem selectedItem = IItem.Items.Find(x => x.Id == Inventory[inventory]);
                        string selectedItemType = selectedItem.GetType().ToString();
                        if (selectedItemType=="Agoraphobia.Items.Weapon")
                        {
                            Weapon selectedWeapon = (Weapon)selectedItem;
                            if (selectedWeapon.Energy<=Energy)
                            {
                                Energy -= selectedWeapon.Energy;
                                target.HP -= Convert.ToInt32(AttackDamage * (r.NextDouble() * (selectedWeapon.MaxMultiplier - selectedWeapon.MinMultiplier) + selectedWeapon.MinMultiplier))-target.Defense;
                            }
                        }
                        else if (selectedItemType=="Agoraphobia.Items.Consumable")
                        {
                            
                            Consumable selectedConsumable = (Consumable)selectedItem;
                            if (selectedConsumable.Duration == 100)
                            {
                                MaxHP += selectedConsumable.HP;
                                MaxEnergy += selectedConsumable.Energy;
                                ChangeDefense(+selectedConsumable.Armor);
                                ChangeAttack(+selectedConsumable.Attack);
                                Inventory.Remove(Inventory[inventory]);
                            }
                            else if (EffectDuration > 0 && EffectDuration < 100)
                            {
                                ChangeAttack(-ChangedAttack);
                                ChangeDefense(-ChangedDefense);
                                EffectDuration = selectedConsumable.Duration;
                                ChangedDefense = selectedConsumable.Armor;
                                ChangedAttack = selectedConsumable.Attack;
                                ChangeEnergy(+selectedConsumable.Energy);
                                ChangeHP(+selectedConsumable.HP);
                                ChangeDefense(+selectedConsumable.Armor);
                                ChangeAttack(+selectedConsumable.Attack);
                                Inventory.Remove(Inventory[inventory]);
                            }
                            else if ((EffectDuration == 0 || EffectDuration < 0) && selectedConsumable.Duration != 100)
                            {
                                EffectDuration = selectedConsumable.Duration;
                                ChangedDefense = selectedConsumable.Armor;
                                ChangedAttack = selectedConsumable.Attack;
                                ChangeEnergy(+selectedConsumable.Energy);
                                ChangeHP(+selectedConsumable.HP);
                                ChangeDefense(+selectedConsumable.Armor);
                                ChangeAttack(+selectedConsumable.Attack);
                                Inventory.Remove(Inventory[inventory]);
                            }
                            if (inventory != 0)
                            {
                                inventory--;
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
            Respawn();
        }

        public static void GoInsane()
        {
            Program.gameEnded = true;

            DateTime playTimeEnd = DateTime.UtcNow;
            TimeSpan playTime = playTimeEnd - playTimeStart;

            Viewport.Message($"You went insane, the game has ended.\n\tPlaytime: {playTime.Hours} hours {playTime.Minutes} minutes {playTime.Seconds} seconds | Score {Points}\n");
            Program.End();
        }

        public static void WakeUp()
        {
            Program.gameEnded = true;
            DateTime playTimeEnd = DateTime.UtcNow;
            TimeSpan playTime = playTimeEnd - playTimeStart;

            Viewport.Message($"You woke up successfully, and dreamt up the best story ever, the game has ended.\n\tPlaytime: {playTime.Hours} hours {playTime.Minutes} minutes {playTime.Seconds} seconds | Score {Points}\n");
            Program.End();
        }

        // ez ilyen.
        public static void Respawn()
        {
            ChangeSanity(-IEnemy.Enemies.Find(x => x.Id == Program.room.Enemy).Sanity);
            hp = MaxHP;
            energy = MaxEnergy;
            Program.room = (Room)IRoom.Rooms.Find(x => x.Id == 0);
            Program.MainScene();
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

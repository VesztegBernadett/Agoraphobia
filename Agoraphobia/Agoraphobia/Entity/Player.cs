using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Entity
{
    internal class Player 
    {
        public static int Defense { get; private set; } = 100;
        public static int MaxHP { get; private set; } = 100;
        private static int hp = 100;
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
        public const int MAXENERGY = 100;
        private static int energy = 100;
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
        private static int attack = 100;
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
        public static int DreamCoins { get; private set; } = 100;
        public static string Name { get; private set; } = "asdasd";
        public static void Attack(IAttackable target)
        {

        }

        public static void Death()
        {

        }

        public static void GoInsane()
        {

        }

        public static void WakeUp()
        {

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

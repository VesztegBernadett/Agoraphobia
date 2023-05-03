using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia.Entity
{
    internal class Player 
    {
        public static int Defense { get; private set; }
        public static int MaxHP { get; private set; }
        public static int HP
        {
            get => HP;
            private set
            {
                if (value > MaxHP)
                    HP = MaxHP;
                else if (value < 0)
                    Death();
                else HP = value;
            }
        }
        public const int MAXENERGY = 100;
        public static int Energy 
        { get => Energy;
            private set
            {
                if (value < 0)
                    Energy = 0;
                else if (value > MAXENERGY)
                    Energy = MAXENERGY;
                else Energy = value;
            }
        }
        public static int AttackDamage 
        { get => AttackDamage;
            private set
            {
                if (value < 0)
                    AttackDamage = 0;
                else AttackDamage = value;
            }
        }
        public static int Sanity 
        { get => Sanity;
            private set
            {
                if (value <= 0)
                    WakeUp();
                else if (value >= 100)
                    GoInsane();
                else Sanity = value;
            }
        }
        public static List<int> Inventory { get; private set; } = new List<int>();
        public static int DreamCoins { get; private set; }
        public static string Name { get; private set; }
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

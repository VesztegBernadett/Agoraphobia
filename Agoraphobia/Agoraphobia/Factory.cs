using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Agoraphobia.Items;
using System.Collections;
using Agoraphobia.Entity;
using static Agoraphobia.IItem;

namespace Agoraphobia
{
    internal class Factory
    {
        private static Random r = new Random();
        public static IElement Create(string filename)
        {
            int id = 0;
            string name = "";
            string desc = "";
            int coins = 0;
            float multiplier = 0;
            int sanity = 0;
            int energy = 0;
            int hp = 0;
            int duration = 0;
            int armor = 0;
            int def = 0;
            int attack = 0;
            int rarity = 0;
            int piece = 0;
            List<int> items = new List<int>();
            List<float> rates = new List<float>();

            foreach (var line in File.ReadLines(filename, Encoding.UTF8))
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
                        desc = data[0];
                        break;
                }
            }
            switch (filename[..3])
            {
                case "Arm":
                    foreach (var line in File.ReadLines(filename, Encoding.UTF8))
                    {
                        string[] data = line.Split('#');
                        switch (data[1])
                        {
                            case "Piece":
                                piece = int.Parse(data[0]);
                                break;
                            case "Defense":
                                def = int.Parse(data[0]);
                                break;
                            case "Attack":
                                attack = int.Parse(data[0]);
                                break;
                            case "Rarity":
                                rarity = int.Parse(data[0]);
                                break;
                        }
                    }
                    return new Armor(id, name, desc, def, attack, piece, rarity);
                case "Con":
                    foreach (var line in File.ReadLines(filename, Encoding.UTF8))
                    {
                        string[] data = line.Split('#');
                        switch (data[1])
                        {
                            case "Energy":
                                energy = int.Parse(data[0]);
                                break;
                            case "HP":
                                hp = int.Parse(data[0]);
                                break;
                            case "Armor":
                                armor = int.Parse(data[0]);
                                break;
                            case "Attack":
                                attack = int.Parse(data[0]);
                                break;
                            case "Duration":
                                duration = int.Parse(data[0]);
                                break;
                            case "Rarity":
                                rarity = int.Parse(data[0]);
                                break;
                        }
                    }
                    return new Consumable(id, name, desc, energy, hp, attack, armor, duration, rarity);
                case "Wea":
                    foreach (var line in File.ReadLines(filename, Encoding.UTF8))
                    {
                        string[] data = line.Split('#');
                        switch (data[1])
                        {
                            case "Energy":
                                energy = int.Parse(data[0]);
                                break;
                            case "Multiplier":
                                multiplier = int.Parse(data[0]);
                                break;
                            case "Rarity":
                                rarity = int.Parse(data[0]);
                                break;
                        }
                    }
                    return new Weapon(id, name, desc, multiplier, energy, rarity);
                case "Ene":
                    foreach (var line in File.ReadLines(filename, Encoding.UTF8))
                    {
                        string[] data = line.Split('#');
                        switch (data[1])
                        {
                            case "Inventory":
                                foreach (var item in data[0].Split(';'))
                                {
                                    int _;
                                    string[] curr = item.Split('(');
                                    if (int.TryParse(curr[0], out _))
                                    {
                                        rates.Add(float.Parse(curr[1]));
                                        items.Add(int.Parse(curr[0]));
                                    }
                                    else
                                    {
                                        int[] interval = Array.ConvertAll(curr[1].Split('-'), int.Parse);
                                        coins = r.Next(interval[0], interval[1] + 1);
                                    }
                                }
                                break;
                            case "Defense":
                                def = int.Parse(data[0]);
                                break;
                            case "HP":
                                hp = int.Parse(data[0]);
                                break;
                            case "Energy":
                                energy = int.Parse(data[0]);
                                break;
                            case "AttackDamage":
                                attack = int.Parse(data[0]);
                                break;
                            case "Sanity":
                                sanity = int.Parse(data[0]);
                                break;
                        }
                    }
                    return new Enemy(id, name, desc, def, attack, sanity, hp, energy, coins, items, rates);
            }
        }
    }
}

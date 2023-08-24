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
using Agoraphobia.Rooms;

namespace Agoraphobia
{
    internal class Factory
    {
        private static Random r = new Random();
        public static void Create(string filename)
        {
            int id = 0;
            string name = "";
            string desc = "";
            int mincoins = 0;
            int maxcoins = 0;
            double minMultiplier = 0;
            double maxMultiplier = 0;
            int sanity = 0;
            int energy = 0;
            int hp = 0;
            int duration = 0;
            int def = 0;
            int attack = 0;
            int rarity = 0;
            int npc = 0;
            int enemy = 0;
            string intro = "";
            int price = 0;
            List<int> items = new List<int>();
            List<double> rates = new List<double>();

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
            switch (filename.Split('/')[6][..3])
            {
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
                                def = int.Parse(data[0]);
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
                            case "Price":
                                price = int.Parse(data[0]);
                                break;
                        }
                    }
                    new Consumable(id, name, desc, energy, hp, attack, def, duration, rarity, price);
                    break;
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
                                double[] current = Array.ConvertAll(data[0].Split('-'), double.Parse);
                                minMultiplier = current[0];
                                maxMultiplier = current[1];
                                break;
                            case "Rarity":
                                rarity = int.Parse(data[0]);
                                break;
                            case "Price":
                                price = int.Parse(data[0]);
                                break;
                        }
                    }
                    new Weapon(id, name, desc, minMultiplier, maxMultiplier, energy, rarity, price);
                    break;
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
                                        rates.Add(Convert.ToDouble(curr[1]));
                                        items.Add(int.Parse(curr[0]));
                                    }
                                    else
                                    {
                                        int[] interval = Array.ConvertAll(curr[1].Split('-'), int.Parse);
                                        mincoins = interval[0];
                                        maxcoins = interval[1];
                                    }
                                }
                                break;
                            case "Defense":
                                def = int.Parse(data[0]);
                                break;
                            case "HP":
                                hp = int.Parse(data[0]);
                                break;
                            case "AttackDamage":
                                attack = int.Parse(data[0]);
                                break;
                            case "Sanity":
                                sanity = int.Parse(data[0]);
                                break;
                        }
                    }
                    new Enemy(id, name, desc, def, attack, sanity, hp, mincoins, maxcoins, items, rates);
                    break;
                case "NPC":
                    foreach (var line in File.ReadLines(filename, Encoding.UTF8))
                    {
                        string[] data = line.Split('#');
                        switch (data[1])
                        {
                            case "Inventory":
                                if (data[0]!="")
                                {
                                    foreach (var item in data[0].Split(';'))
                                        items.Add(int.Parse(item));
                                }
                            break;
                            case "Intro":
                                intro = data[0];
                                break;
                        }
                    }
                    new NPC(id, name, desc, items, intro);
                    break;
                default:
                    foreach (var line in File.ReadLines(filename, Encoding.UTF8))
                    {
                        string[] data = line.Split('#');
                        switch (data[1])
                        {
                            case "NPC":
                                npc = int.Parse(data[0]);
                                break;
                            case "Enemy":
                                enemy = int.Parse(data[0]);
                                break;
                            case "Items":
                                if (data[0] == "")
                                    break;
                                else items = data[0].Split(';').Select(int.Parse).ToList();
                                break;
                        }
                    }
                    new Room(id, name, desc, npc, enemy, items);
                    break;
            }
        }
    }
}

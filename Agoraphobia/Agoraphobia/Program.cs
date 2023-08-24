using Agoraphobia;
using Agoraphobia.Entity;
using Agoraphobia.Items;
using Agoraphobia.Rooms;
using System;
using System.Globalization;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Transactions;
using System.Reflection;
using System.Diagnostics.Contracts;
using static Agoraphobia.IItem;

namespace Agoraphobia
{
    class Program
    {
        private static readonly Random r = new Random();
        public static Room room = (Room)IRoom.Rooms.Find(x => x.Id == 0);
        public static bool gameEnded = false;
        public static int inventory = 0;
        public static string PATH = "../../../Files/";
        public static int slot = 0;
        public static bool newGame = false;
        private static void SwitchRoom(int interaction)
        {
            if (room.NPC == 0)
                interaction++;
            CreateRoom(room.Exits[interaction - 1]);
            room = (Room)IRoom.Rooms.Find(x => x.Id == room.Exits[interaction - 1]);
            MainScene();
        }
        private static void CreateExits()
        {
            room.Exits.Clear();
            List<int> temp = IRoom.Rooms.Select(x => x.Id).ToList();
            while (room.Exits.Count < 3)
            {
                int id = r.Next(0, temp.Count);
                bool containsName = false;
                if (temp[id] != room.Id)
                {
                    foreach (var item in room.Exits)
                    {
                        if (IRoom.Rooms.Find(x => x.Id == item).Name == IRoom.Rooms.Find(x => x.Id == temp[id]).Name || room.Name == IRoom.Rooms.Find(x => x.Id == temp[id]).Name)
                            containsName = true;
                    }
                    if (!containsName)
                    {
                        room.Exits.Add(temp[id]);
                        temp.Remove(temp[id]);
                    }
                }
            }
        }
        public static void CreateRoom(int id)
        {
            if (!IRoom.Rooms.Any(x => x.Id == id))
                Factory.Create($"{PATH}Slot{slot}/Rooms/Room{id}.txt");
        }
        public static void CreateItem(int id)
        {
            if (!IItem.Items.Any(x => x.Id == id))
            {
                if (File.Exists($"{PATH}Slot{slot}/Items/Consumable{id}.txt"))
                    Factory.Create($"{PATH}Slot{slot}/Items/Consumable{id}.txt");
                else Factory.Create($"{PATH}Slot{slot}/Items/Weapon{id}.txt");
            }
        }
        public static void CreateNPC(int id)
        {
            if (!INPC.NPCs.Any(x => x.Id == id))
                Factory.Create($"{PATH}Slot{slot}/NPCs/NPC{id}.txt");
        }
        public static void CreateEnemy(int id)
        {
            if (!IEnemy.Enemies.Any(x => x.Id == id))
                Factory.Create($"{PATH}Slot{slot}/Enemies/Enemy{id}.txt");
        }
        private static void Main()
        {
            Player.playTimeStart = DateTime.UtcNow;
            CultureInfo enCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = enCulture;
            Console.Title = "Agoraphobia";
            Console.CursorVisible = false;

            while (true)
            {
                try
                {
                    Console.SetWindowSize(200, 45);
                    Viewport.Menu();
                    break;
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Please zoom out until this text is slightly smaller, but still readable. Press any key if you're ready.");
                    Console.ReadKey(true);
                    continue;
                }
            }
            //Generate rooms
            for (int i = 0; i < Directory.GetFiles($"{PATH}Slot{slot}/Rooms/").Count(); i++)
                CreateRoom(i);
            for (int i = 1; i <= Directory.GetFiles($"{PATH}Slot{slot}/NPCs/").Count(); i++)
                CreateNPC(i);
            for (int i = 0; i < Directory.GetFiles($"{PATH}Slot{slot}/Items/").Count(); i++)
                CreateItem(i);
            for (int i = 1; i <= Directory.GetFiles($"{PATH}Slot{slot}/Enemies/").Count(); i++)
                CreateEnemy(i);

            //Load player's values from file
            string[] rows = File.ReadAllLines($"{PATH}Slot{slot}/Player.txt");
            Player.ChangeDefense(int.Parse(rows[0].Split('#')[0]) - Player.Defense);
            Player.ChangeMaxHP(int.Parse(rows[1].Split('#')[0]) - Player.MaxHP);
            Player.ChangeHP(int.Parse(rows[2].Split('#')[0]) - Player.HP);
            Player.Points = int.Parse(rows[3].Split('#')[0]);
            Player.MaxEnergy = int.Parse(rows[4].Split('#')[0]);
            Player.ChangeEnergy(int.Parse(rows[5].Split('#')[0]) - Player.Energy);
            Player.ChangeAttack(int.Parse(rows[6].Split('#')[0]) - Player.AttackDamage);
            Player.ChangeSanity(int.Parse(rows[7].Split('#')[0]) - Player.Sanity);
            Player.EffectDuration = 0;
            Player.Inventory.Clear();
            if (rows[8].Split('#')[0] != "")
            {
                Player.Inventory = rows[8].Split('#')[0].Split(';').Select(int.Parse).ToList();
                foreach (var item in Player.Inventory.GroupBy(x => x))
                {
                    if (IItem.Items.Find(x => x.Id == item.Key).GetType().ToString() == "Agoraphobia.Items.Weapon")
                    {
                        Weapon weapon = (Weapon)IItem.Items.Find(x => x.Id == item.Key);
                        for (int i = 0; i < item.Count() - 1; i++)
                            weapon.LevelUp();
                    }
                }
            }
            Player.ChangeCoins(int.Parse(rows[9].Split('#')[0]) - Player.DreamCoins);
            room = (Room)IRoom.Rooms.Find(x => x.Id == int.Parse(rows[12].Split('#')[0]));

            if (newGame)
                Viewport.Intro();

            MainScene();
        }
        private static void RemoveItem(ref int length, ref int interaction, bool isOpened, bool isTriggered)
        {
            if (Player.InventoryLength < 18)
            {
                if (room.NPC == 0)
                    interaction++;
                int temp = interaction;
                int offset = 0;
                if (isTriggered)
                    offset += room.Exits.Count - 1;
                length--;
                IItem item = IItem.Items.Find(x => x.Id == room.Items[temp - 2 - offset]);
                if (item.GetType().ToString() == "Agoraphobia.Items.Consumable")
                {
                    Consumable consumable = (Consumable)item;
                    consumable.Obtain();
                }
                else
                {
                    Weapon weapon = (Weapon)item;
                    IRoom.Rooms.Find(x => x.Id == room.Id).Items.Remove(weapon.Id);
                    weapon.Obtain();
                }
                if (interaction == length)
                    interaction--;
            }
            else
            {
                Viewport.Message("Your inventory is full, you can't pick up this item.");
            }
            Viewport.ShowInventory();
            Viewport.ClearInteraction();
            Viewport.ShowStats();
            Viewport.Interaction(interaction, isOpened, isTriggered);
            Viewport.Show();
        }

        private static void DropItem(ref int interaction, bool isOpened, bool isTriggered)
        {
            IItem.Items.Find(x => x.Id == Player.Inventory.Distinct().ToArray()[inventory]).Drop(Player.Inventory.Distinct().ToArray()[inventory]);

            if (inventory == Player.Inventory.Distinct().ToArray().Count())
                inventory--;

            Viewport.ShowGrid();
            Viewport.ShowInventory();
            Viewport.ClearInteraction();
            Viewport.ShowStats();
            Viewport.Interaction(interaction, isOpened, isTriggered);
            Viewport.Show();
        }

        private static void ZoomOut()
        {
            Console.Clear();
            Console.WriteLine("Please zoom out until this text is slightly smaller, but still readable. Press any key if you're ready.");
            Console.ReadKey(true);
            MainScene();
        }
        public static void MainScene()
        {
            try
            {
                Console.SetWindowSize(200, 45);
                Console.Clear();

                int interaction = 0;
                bool isOpened = false;
                bool isTriggered = false;
                int length;

                Viewport.Show();
                Viewport.ShowGrid();
                Viewport.ShowStats();
                Viewport.ShowInventory();
                Viewport.Interaction(interaction, isOpened, isTriggered);

                ConsoleKey input = Console.ReadKey(true).Key;

                // a gameEnded be lett rakva a input while loop feltételei közé
                // ez azért kelett hogy ha vége a játéknak akkor ne lehessen
                // menübe navigálni, új szobába menni stb. Player.WakeUp és Player.GoInsane használja
                while (input != ConsoleKey.Escape & !gameEnded)
                {
                    Console.CursorVisible = false;
                    length = 1;
                    if (room.NPC != 0)
                        length++;
                    if (room.Items.Count != 0)
                    {
                        if (isOpened)
                            length += room.Items.Count;
                        else length++;
                    }
                    if (isTriggered)
                        length += 2;
                    switch (input)
                    {
                        case ConsoleKey.D:
                            DropItem(ref interaction, isOpened, isTriggered);
                            break;
                        case ConsoleKey.LeftArrow:
                            if (inventory == 0)
                                inventory = Player.Inventory.GroupBy(x => x).Count() - 1;
                            else inventory--;
                            Viewport.ShowInventory();
                            break;
                        case ConsoleKey.RightArrow:
                            if (inventory == Player.Inventory.GroupBy(x => x).Count() - 1)
                                inventory = 0;
                            else inventory++;
                            Viewport.ShowInventory();
                            break;
                        case ConsoleKey.UpArrow:
                            if (interaction == 0)
                                interaction = length - 1;
                            else interaction--;
                            break;
                        case ConsoleKey.DownArrow:
                            if (interaction == length - 1)
                                interaction = 0;
                            else interaction++;
                            break;
                        case ConsoleKey.Enter:
                            if (room.NPC == 0)
                                interaction++;
                            if (isTriggered)
                                interaction++;
                            if (interaction == 0)
                            {
                                INPC.NPCs.Find(x => x.Id == room.NPC).Interact();
                            }
                            else if (interaction == 1)
                            {
                                if (room.Enemy != 0)
                                {
                                    bool hasWeapon = false;
                                    foreach (var item in Player.Inventory)
                                    {
                                        if (IItem.Items.Find(x => x.Id == item).GetType().ToString() == "Agoraphobia.Items.Weapon")
                                            hasWeapon = true;
                                    }
                                    if (hasWeapon)
                                    {
                                        Console.Clear();
                                        Player.Attack(IEnemy.Enemies.Find(x => x.Id == room.Enemy), ref inventory);
                                        length++;
                                    }
                                    else Viewport.Message("You need to pick up a weapon first!");
                                }
                                else if (!isTriggered)
                                {
                                    isTriggered = true;
                                    CreateExits();
                                    length += room.Exits.Count - 1;
                                    if (room.NPC == 0)
                                        interaction--;
                                }
                            }
                            else
                            {
                                if (room.NPC == 0)
                                    interaction--;
                                if (!isOpened)
                                {
                                    if (room.Items.Count > 1 && interaction == length - 1)
                                    {
                                        isOpened = true;
                                        length += room.Items.Count - 1;
                                    }
                                    else if (room.Items.Count == 1 && interaction == length - 1)
                                        RemoveItem(ref length, ref interaction, isOpened, isTriggered);
                                    else SwitchRoom(interaction);
                                }
                                else
                                {
                                    if (interaction > length - room.Items.Count - 1)
                                    {
                                        RemoveItem(ref length, ref interaction, isOpened, isTriggered);
                                        if (room.Items.Count == 0)
                                            isOpened = false;
                                    }
                                    else if (interaction <= length - room.Items.Count - 1)
                                    {
                                        if (isTriggered)
                                            SwitchRoom(interaction);
                                        else
                                        {
                                            isTriggered = true;
                                            CreateExits();
                                            length += room.Exits.Count - 1;
                                        }
                                    }
                                }
                            }
                            Viewport.Interaction(interaction, isOpened, isTriggered);
                            break;
                        case ConsoleKey.I:
                            if (room.NPC == 0)
                                interaction++;
                            if (interaction > 1 && (isOpened || room.Items.Count == 1))
                            {
                                if (!isTriggered)
                                    Viewport.Message(IItem.Items.Find(x => x.Id == room.Items[interaction - 2]).Inspect());
                                else if (interaction > 3)
                                    Viewport.Message(IItem.Items.Find(x => x.Id == room.Items[interaction - 4]).Inspect());
                            }
                            break;
                        default:
                            break;
                    }
                    Viewport.Interaction(interaction, isOpened, isTriggered);
                    if (Console.KeyAvailable)
                        Console.ReadKey(true);
                    input = Console.ReadKey(true).Key;
                }
                //We need an end scene because now if we get out of the loop and finish this method, we get back to the last combat which called mainscene method
                End();
            }
            catch (ArgumentOutOfRangeException e)
            {
                ZoomOut();
            }
        }
        public static void NewGame()
        {
            newGame = true;
            File.WriteAllText($"{PATH}Slot{slot}/Player.txt", File.ReadAllText($"{PATH}Safety/Player.txt"));
            for (int i = 0; i < Directory.GetFiles($"{PATH}Slot{slot}/Rooms/").Count(); i++)
                File.WriteAllText($"{PATH}Slot{slot}/Rooms/Room{i}.txt", File.ReadAllText($"{PATH}Safety/Rooms/Room{i}.txt"));
            for (int i = 1; i <= Directory.GetFiles($"{PATH}Slot{slot}/NPCs/").Count(); i++)
                File.WriteAllText($"{PATH}Slot{slot}/NPCs/NPC{i}.txt", File.ReadAllText($"{PATH}Safety/NPCs/NPC{i}.txt"));
            for (int i = 0; i < Directory.GetFiles($"{PATH}Slot{slot}/Items/").Count(); i++)
            {
                if (File.Exists($"{PATH}Slot{slot}/Items/Consumable{i}.txt"))
                    File.WriteAllText($"{PATH}Slot{slot}/Items/Consumable{i}.txt", File.ReadAllText($"{PATH}Safety/Items/Consumable{i}.txt"));
                else File.WriteAllText($"{PATH}Slot{slot}/Items/Weapon{i}.txt", File.ReadAllText($"{PATH}Safety/Items/Weapon{i}.txt"));
            }
            for (int i = 1; i <= Directory.GetFiles($"{PATH}Slot{slot}/Enemies/").Count(); i++)
                File.WriteAllText($"{PATH}Slot{slot}/Enemies/Enemy{i}.txt", File.ReadAllText($"{PATH}Safety/Enemies/Enemy{i}.txt"));
        }
        public static void End()
        {
            Player.Points = 0;
            foreach (var item in Player.Inventory)
            {
                IItem i = IItem.Items.Find(x => x.Id == item);
                Player.Points += 10 * ItemValue[i.Rarity];
            }
            if (gameEnded)
            {
                NewGame();
                gameEnded = false;
            }
            else
            {
                //Save player's data when quit from the game
                StreamWriter playerData = new StreamWriter($"{PATH}Slot{slot}/Player.txt");
                playerData.Write($"{Player.Defense}#Defense\n{Player.MaxHP}#MaxHP\n" +
                    $"{Player.HP}#HP\n{Player.Points}#Points\n" +
                    $"{Player.MaxEnergy}#MaxEnergy\n{Player.Energy}#Energy\n" +
                    $"{Player.AttackDamage}#Attack\n{Player.Sanity}#Sanity\n" +
                    $"{string.Join(';',Player.Inventory)}#Inventory\n{Player.DreamCoins}#DreamCoins\n{0}#IsNew\n{DateTime.Now}#Modified\n{room.Id}#Room");
                playerData.Close();

                for (int i = 0; i < Directory.GetFiles($"{PATH}Slot{slot}/Rooms/").Count(); i++)
                {
                    StreamWriter roomData = new StreamWriter($"{PATH}Slot{slot}/Rooms/Room{i}.txt");
                    Room room = (Room)IRoom.Rooms.Find(x => x.Id == i);
                    roomData.Write($"{room.Id}#Id\n{room.Name}#Name\n{room.Description}#Description\n{room.NPC}#NPC\n{room.Enemy}#Enemy\n{string.Join(';', room.Items)}#Items");
                    roomData.Close();
                }
                for (int i = 1; i <= Directory.GetFiles($"{PATH}Slot{slot}/NPCs/").Count(); i++)
                {
                    StreamWriter npcData = new StreamWriter($"{PATH}Slot{slot}/NPCs/NPC{i}.txt");
                    NPC npc = (NPC)INPC.NPCs.Find(x => x.Id == i);
                    npcData.Write($"{npc.Id}#Id\n{npc.Name}#Name\n{npc.Description}#Description\n{string.Join(';', npc.Inventory)}#Inventory\n{npc.Intro}#Intro");
                    npcData.Close();
                }
                for (int i = 0; i < Directory.GetFiles($"{PATH}Slot{slot}/Items/").Count(); i++)
                {
                    if (File.Exists($"{PATH}Slot{slot}/Items/Consumable{i}.txt"))
                    {
                        StreamWriter consumableData = new StreamWriter($"{PATH}Slot{slot}/Items/Consumable{i}.txt");
                        Consumable consumable = (Consumable)IItem.Items.Find(x => x.Id == i);
                        consumableData.Write($"{consumable.Id}#Id\n{consumable.Name}#Name\n{consumable.Description}#Description\n{(int)consumable.Rarity}#Rarity\n{consumable.Energy}#Energy\n{consumable.HP}#HP\n{consumable.Armor}#Armor\n{consumable.Attack}#Attack\n{consumable.Duration}#Duration\n{consumable.Price}#Price");
                        consumableData.Close();
                    }
                    else
                    {
                        StreamWriter weaponData = new StreamWriter($"{PATH}Slot{slot}/Items/Weapon{i}.txt");
                        Weapon weapon = (Weapon)IItem.Items.Find(x => x.Id == i);
                        weaponData.Write($"{weapon.Id}#Id\n{weapon.Name}#Name\n{weapon.Description}#Description\n{(int)weapon.Rarity}#Rarity\n{weapon.MinMultiplier}-{weapon.MaxMultiplier}#Multiplier\n{weapon.Energy}#Energy\n{weapon.Price}#Price");
                        weaponData.Close();
                    }
                }
                for (int i = 1; i <= Directory.GetFiles($"{PATH}Slot{slot}/Enemies/").Count(); i++)
                {
                    StreamWriter enemyData = new StreamWriter($"{PATH}Slot{slot}/Enemies/Enemy{i}.txt");
                    Enemy enemy = (Enemy)IEnemy.Enemies.Find(x => x.Id == i);
                    string droprates = "";
                    for (int j = 0; j < enemy.DropRate.Count; j++)
                        droprates += $"{enemy.Inventory[j]}({enemy.DropRate[enemy.Inventory[j]]};";
                    enemyData.Write($"{enemy.Id}#Id\n{enemy.Name}#Name\n{enemy.Description}#Description\n{droprates}DC({enemy.MinDreamCoins}-{enemy.MaxDreamCoins}#Inventory\n{enemy.Defense}#Defense\n{enemy.HP}#HP\n{enemy.AttackDamage}#AttackDamage\n{enemy.Sanity}#Sanity");
                    enemyData.Close();
                }
            }
            IRoom.Rooms.Clear();
            IItem.Items.Clear();
            IEnemy.Enemies.Clear();
            INPC.NPCs.Clear();
            Main();
        }
    }
}

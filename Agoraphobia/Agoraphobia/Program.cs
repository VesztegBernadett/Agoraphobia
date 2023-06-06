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

namespace Agoraphobia
{
    class Program
    {
        private static readonly Random r = new Random();
        public static Room room = (Room)IRoom.Rooms.Find(x => x.Id == 0);
        public static bool gameEnded = false;
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
                Factory.Create($"{IElement.PATH}Rooms/Room{id}.txt");
        }
        public static void CreateItem(int id)
        {
            if (!IItem.Items.Any(x => x.Id == id))
            {
                if (File.Exists($"{IElement.PATH}Items/Consumable{id}.txt"))
                    Factory.Create($"{IElement.PATH}Items/Consumable{id}.txt");
                else Factory.Create($"{IElement.PATH}Items/Weapon{id}.txt");
            }
        }
        public static void CreateNPC(int id)
        {
            if (!INPC.NPCs.Any(x => x.Id == id))
                Factory.Create($"{IElement.PATH}NPCs/NPC{id}.txt");
        }
        public static void CreateEnemy(int id)
        {
            if (!IEnemy.Enemies.Any(x => x.Id == id))
                Factory.Create($"{IElement.PATH}Enemies/Enemy{id}.txt");
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
            for (int i = 0; i < Directory.GetFiles($"{IElement.PATH}Rooms/").Count(); i++)
                CreateRoom(i);
            for (int i = 1; i <= Directory.GetFiles($"{IElement.PATH}NPCs/").Count(); i++)
                CreateNPC(i);
            for (int i = 0; i < Directory.GetFiles($"{IElement.PATH}Items/").Count(); i++)
                CreateItem(i);
            for (int i = 1; i <= Directory.GetFiles($"{IElement.PATH}Enemies/").Count(); i++)
                CreateEnemy(i);

            //Load player's values from file
            string[] rows = File.ReadAllLines($"{IElement.PATH}Player{Player.Slot}.txt");
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
                Player.Inventory = rows[8].Split('#')[0].Split(';').Select(int.Parse).ToList();
            Player.ChangeCoins(int.Parse(rows[9].Split('#')[0]) - Player.DreamCoins);
            room = (Room)IRoom.Rooms.Find(x => x.Id == int.Parse(rows[12].Split('#')[0]));

            if (int.Parse(rows[10].Split('#')[0]) == 1)
                Viewport.Intro();

            MainScene();
        }
        private static void RemoveItem(ref int length, ref int interaction, int inventory, bool isOpened, bool isTriggered)
        {
            if (Player.Inventory.Count() < 18)
            {
                if (room.NPC == 0)
                interaction++;
                int temp = interaction;
                int offset = 0;
                if (isTriggered)
                    offset += room.Exits.Count - 1;
                length--;
                IItem.Items.Find(x => x.Id == room.Items[temp - 2 - offset]).PickUp(room.Id);
                interaction = length - 1;
            }
            else
            {
                Viewport.Message("Your inventory is full, you can't pick up this item.");
            }
            Viewport.ShowInventory(inventory);
            Viewport.ClearInteraction();
            Viewport.ShowStats();
            Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
            Viewport.Show(room.Id);
        }

        private static void DropItem(ref int interaction, bool isOpened, bool isTriggered, ref int inventory)
        {
            int inv = inventory;
            IItem.Items.Find(x => x.Id == Player.Inventory[inv]).Drop(room.Id, inv);

            if (inventory != 0)
                inventory--;

            //Clear inventory??
            Console.Clear();

            Viewport.ShowGrid();
            Viewport.ShowInventory(inventory);
            Viewport.ClearInteraction();
            Viewport.ShowStats();
            Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
            Viewport.Show(room.Id);
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
                Console.CursorVisible = false;
                Console.Clear();

                int inventory = 0;
                int interaction = 0;
                bool isOpened = false;
                bool isTriggered = false;
                int length;

                Viewport.Show(room.Id);
                Viewport.ShowGrid();
                Viewport.ShowStats();
                Viewport.ShowInventory(inventory);
                Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);

                ConsoleKey input = Console.ReadKey(true).Key;

                // a gameEnded be lett rakva a input while loop feltételei közé
                // ez azért kelett hogy ha vége a játéknak akkor ne lehessen
                // menübe navigálni, új szobába menni stb. Player.WakeUp és Player.GoInsane használja
                while (input != ConsoleKey.X && !gameEnded)
                {
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
                            DropItem(ref interaction, isOpened, isTriggered, ref inventory);
                            break;
                        case ConsoleKey.LeftArrow:
                            if (inventory == 0)
                                inventory = Player.Inventory.Count - 1;
                            else inventory--;
                            Viewport.ShowInventory(inventory);
                            break;
                        case ConsoleKey.RightArrow:
                            if (inventory == Player.Inventory.Count - 1)
                                inventory = 0;
                            else inventory++;
                            Viewport.ShowInventory(inventory);
                            break;
                        case ConsoleKey.UpArrow:
                            if (interaction == 0)
                                interaction = length - 1;
                            else interaction--;
                            Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
                            break;
                        case ConsoleKey.DownArrow:
                            if (interaction == length - 1)
                                interaction = 0;
                            else interaction++;
                            Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
                            break;
                        case ConsoleKey.Enter:
                            if (room.NPC == 0)
                                interaction++;
                            if (isTriggered)
                                interaction++;
                            if (interaction == 0 || (interaction == 1 && isTriggered))
                            {
                                if (room.NPC != 0)
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
                                        Player.Attack(IEnemy.Enemies.Find(x => x.Id == room.Enemy));
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
                                if (isTriggered)
                                    interaction--;
                                if (room.NPC == 0)
                                    interaction--;
                                if (!isOpened && room.Items.Count > 1 && interaction == length - 1)
                                {
                                    isOpened = true;
                                    length += room.Items.Count - 1;
                                }
                                else if (!isOpened && room.Items.Count == 1 && interaction == length - 1)
                                    RemoveItem(ref length, ref interaction, inventory, isOpened, isTriggered);
                                else if (!isOpened)
                                    SwitchRoom(interaction);
                                else if (isOpened && interaction > length - room.Items.Count - 1)
                                {
                                    RemoveItem(ref length, ref interaction, inventory, isOpened, isTriggered);
                                    if (room.Items.Count == 0)
                                        isOpened = false;
                                }
                                else if (isOpened && interaction <= length - room.Items.Count - 1)
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
                            Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
                            break;
                        default:
                            break;
                    }
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

        public static void End()
        {
            if (gameEnded)
            {
                string content = File.ReadAllText($"{IElement.PATH}Safety.txt");
                File.WriteAllText($"{IElement.PATH}Player{Player.Slot}.txt", content);
                gameEnded = false;
            }
            else
            {
                //Save player's data when quit from the game
                StreamWriter playerData = new StreamWriter($"{IElement.PATH}Player{Player.Slot}.txt");
                playerData.Write($"{Player.Defense}#Defense\n{Player.MaxHP}#MaxHP\n" +
                    $"{Player.HP}#HP\n{Player.Points}#Points\n" +
                    $"{Player.MaxEnergy}#MaxEnergy\n{Player.Energy}#Energy\n" +
                    $"{Player.AttackDamage}#Attack\n{Player.Sanity}#Sanity\n" +
                    $"{string.Join(';',Player.Inventory)}#Inventory\n{Player.DreamCoins}#DreamCoins\n{0}#IsNew\n{DateTime.Now}#Modified\n{room.Id}#Room");
                playerData.Close();

                Viewport.Message("Your data is saved. See you later!");
            }
            IRoom.Rooms.Clear();
            IItem.Items.Clear();
            IEnemy.Enemies.Clear();
            INPC.NPCs.Clear();
            Main();
        }
    }
}

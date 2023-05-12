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
            if (IRoom.Rooms.Count <= 3)
            {
                for (int i = 0; i < IRoom.Rooms.Count; i++)
                {
                    if (IRoom.Rooms[i].Id != room.Id)
                        room.Exits.Add(IRoom.Rooms[i].Id);
                }
            }
            else
            {
                List<int> temp = IRoom.Rooms.Select(x => x.Id).ToList();
                while (room.Exits.Count < 3)
                {
                    int id = r.Next(0, temp.Count);
                    if (temp[id] != room.Id)
                    {
                        room.Exits.Add(temp[id]);
                        temp.Remove(temp[id]);
                    }
                }
            }
        }
        private static void CreateItems(List<int> items)
        {
            foreach (var item in items)
            {
                if (!IItem.Items.Any(x => x.Id == item))
                {
                    if (File.Exists($"{IElement.PATH}Items/Consumable{item}.txt"))
                        Factory.Create($"{IElement.PATH}Items/Consumable{item}.txt");
                    else if (File.Exists($"{IElement.PATH}Items/Armor{item}.txt"))
                        Factory.Create($"{IElement.PATH}Items/Armor{item}.txt");
                    else Factory.Create($"{IElement.PATH}Items/Weapon{item}.txt");
                }
            }
        }
        private static void CreateRoom(int id)
        {
            if (!IRoom.Rooms.Any(x => x.Id == id))
                Factory.Create($"{IElement.PATH}Rooms/Room{id}.txt");
            Room current = (Room)IRoom.Rooms.Find(x => x.Id == id);
            CreateItems(current.Items);

            if (current.NPC != 0)
            {
                if (!INPC.NPCs.Any(x => x.Id == current.NPC))
                    Factory.Create($"{IElement.PATH}NPCs/NPC{current.NPC}.txt");
                CreateItems(INPC.NPCs.Find(x => x.Id == current.NPC).Inventory);
            }
            if (current.Enemy != 0)
            {
                if (!IEnemy.Enemies.Any(x => x.Id == current.Enemy))
                    Factory.Create($"{IElement.PATH}Enemies/Enemy{current.Enemy}.txt");
                CreateItems(IEnemy.Enemies.Find(x => x.Id == current.Enemy).Inventory);
            }
        }
        private static void Main()
        {
            Player.playTimeStart = DateTime.UtcNow;
            CultureInfo enCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = enCulture;
            Console.Title = "Agoraphobia";

            for (int i = 0; i < Directory.GetFiles($"{IElement.PATH}Rooms/").Count(); i++)
                CreateRoom(i);
            MainScene();
        }
        private static void RemoveItem(ref int length, ref int interaction, int inventory, bool isOpened, bool isTriggered)
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
            Viewport.ShowInventory(inventory);
            Viewport.ClearInteraction();
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
                int length = room.Items.Count == 0 ? 1 : isOpened ? room.Items.Count + 1 : 2;
                if (room.NPC != 0)
                    length++;

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
                    switch (input)
                    {
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
                            switch (interaction)
                            {
                                case 0:
                                    if (room.NPC != 0)
                                        INPC.NPCs.Find(x => x.Id == room.NPC).Interact();
                                    break;
                                case 1:
                                    if (room.Enemy != 0)
                                    {
                                        Console.Clear();
                                        Player.Attack(IEnemy.Enemies.Find(x => x.Id == room.Enemy));
                                        length++;
                                    }
                                    else if (!isTriggered)
                                    {
                                        isTriggered = true;
                                        CreateExits();
                                        length += room.Exits.Count - 1;
                                        if(room.NPC == 0)
                                            interaction--;
                                        Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
                                    }
                                    break;
                                default:
                                    if (isTriggered)
                                        interaction--;
                                    if (room.NPC == 0)
                                        interaction--;
                                    if (!isOpened && room.Items.Count > 1 && interaction == length - 1)
                                    {
                                        isOpened = true;
                                        length += room.Items.Count - 1;
                                        Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
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
                                            Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
                                        }
                                    }
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    input = Console.ReadKey(true).Key;
                }
                //End()
                //We need an end scene because now if we get out of the loop and finish this method, we get back to the last combat which called mainscene method
            }
            catch (ArgumentOutOfRangeException e)
            {
                ZoomOut();
            }
        }
    }
}

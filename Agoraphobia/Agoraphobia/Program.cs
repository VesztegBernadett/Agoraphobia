using Agoraphobia;
using Agoraphobia.Entity;
using Agoraphobia.Items;
using Agoraphobia.Rooms;
using System;
using System.Globalization;

namespace Agoraphobia
{
    class Program
    {
        public static void ItemCreate(List<int> items)
        {
            foreach (var item in items)
            {
                if (File.Exists($"{IElement.PATH}Items/Consumable{item}.txt"))
                {
                    Factory.Create($"{IElement.PATH}Items/Consumable{item}.txt");
                }
                else if (File.Exists($"{IElement.PATH}Items/Armor{item}.txt"))
                {
                    Factory.Create($"{IElement.PATH}Items/Armor{item}.txt");
                }
                else
                {
                    Factory.Create($"{IElement.PATH}Items/Weapon{item}.txt");
                }
            }
        }
        public static void Createroom(int id)
        {
            Factory.Create($"{IElement.PATH}Rooms/Room{id}.txt");
            ItemCreate(room.Items);

            if (File.Exists($"{IElement.PATH}NPCs/NPC{room.NPC}.txt"))
            {
                Factory.Create($"{IElement.PATH}NPCs/NPC{room.NPC}.txt");
                ItemCreate(INPC.NPCs.Find(x => x.Id == room.NPC).Inventory);
            }



            if (File.Exists($"{IElement.PATH}Enemies/Enemy{room.Enemy}.txt"))
            {
                Factory.Create($"{IElement.PATH}Enemies/Enemy{room.Enemy}.txt");
                ItemCreate(IEnemy.Enemies.Find(x => x.Id == room.Enemy).Inventory);
            }
        }
        public static Room room = (Room)IRoom.Rooms.Find(x => x.Id == 0);
        public static void Main()
        {
            CultureInfo enCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = enCulture;
            Console.Title = "Agoraphobia";

            Createroom(0);

            MainScene();
        }

        public static void ZoomOut()
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
                Player.playTimeStart = DateTime.UtcNow;
                Console.SetWindowSize(200, 45);
                Console.CursorVisible = false;
                Console.Clear();

                Viewport.Show(0);
                Viewport.ShowGrid();
                Viewport.ShowStats();
                Viewport.ShowInventory(0);
                Viewport.Interaction(0, 0, false);

                int inventory = 0;
                int interaction = 0;
                bool isOpened = false;
                int length = isOpened ? room.Items.Count : 1;
                if (room.NPC != 0)
                    length++;
                if (room.Enemy != 0)
                    length++;

                ConsoleKey input = Console.ReadKey(true).Key;

                while (input != ConsoleKey.X)
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
                            Viewport.Interaction(room.Id, interaction, isOpened);
                            break;
                        case ConsoleKey.DownArrow:
                            if (interaction == length - 1)
                                interaction = 0;
                            else interaction++;
                            Viewport.Interaction(room.Id, interaction, isOpened);
                            break;
                        case ConsoleKey.Enter:
                            if (room.NPC == 0)
                                interaction++;
                            if (room.Enemy == 0)
                                interaction++;
                            switch (interaction)
                            {
                                case 0:
                                    if (room.NPC != 0)
                                    {
                                        Viewport.ClearInteraction();
                                        INPC.NPCs.Find(x => x.Id == room.Id).Interact();
                                    }
                                    break;
                                case 1:
                                    if (room.Enemy != 0)
                                    {
                                        Player.Attack(IEnemy.Enemies.Find(x => x.Id == room.Enemy));
                                        room.RemoveEnemy();
                                    }
                                    break;
                                default:
                                    if (!isOpened)
                                    {
                                        isOpened = true;
                                        length += room.Items.Count - 1;
                                        Viewport.Interaction(room.Id, interaction, isOpened);
                                    }
                                    else
                                    {
                                        length--;
                                        IItem.Items.Find(x => x.Id == room.Items[interaction - 2]).PickUp(room.Id);
                                        interaction = length - 1;
                                        Viewport.ShowInventory(inventory);
                                        Viewport.ClearInteraction();
                                        Viewport.Interaction(room.Id, interaction, isOpened);
                                        Viewport.Show(room.Id);
                                    }
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    input = Console.ReadKey(true).Key;
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                ZoomOut();
            }
        }
    }
}


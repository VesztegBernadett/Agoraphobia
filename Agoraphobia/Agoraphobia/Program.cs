using Agoraphobia;
using Agoraphobia.Entity;
using Agoraphobia.Items;
using Agoraphobia.Rooms;
using System;

namespace Agoraphobia
{
    class Program
    {
        public static Room room = (Room)IRoom.Rooms.Find(x => x.Id == 0);
        public static void Main()
        {
            Console.Title = "Agoraphobia";

            Factory.Create($"{IElement.PATH}Items/Consumable0.txt");
            Factory.Create($"{IElement.PATH}Items/Consumable1.txt");
            Factory.Create($"{IElement.PATH}Rooms/Room0.txt");
            Factory.Create($"{IElement.PATH}NPCs/NPC1.txt");
            Factory.Create($"{IElement.PATH}Items/Weapon2.txt");
            Factory.Create($"{IElement.PATH}Enemies/Enemy1.txt");

            Player.Inventory.Add(2);
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


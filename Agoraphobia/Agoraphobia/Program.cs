using Agoraphobia;
using Agoraphobia.Entity;
using Agoraphobia.Items;
using Agoraphobia.Rooms;
using System;
using System.Globalization;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace Agoraphobia
{
class Program
{
    public static Room room = (Room)IRoom.Rooms.Find(x => x.Id == 0);
    private static void SwitchRoom (int interaction)
    {
        room = (Room)IRoom.Rooms.Find(x => x.Id == room.Exits[interaction - 1]);
        CreateRoom(room.Id);
        MainScene();
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
        if(!IRoom.Rooms.Any(x => x.Id == id))
            Factory.Create($"{IElement.PATH}Rooms/Room{id}.txt");
        CreateItems(room.Items);

        if (room.NPC != 0)
        {
            if(!INPC.NPCs.Any(x => x.Id == id))
                Factory.Create($"{IElement.PATH}NPCs/NPC{room.NPC}.txt");
            CreateItems(INPC.NPCs.Find(x => x.Id == room.NPC).Inventory);
        }
        if (room.Enemy != 0)
        {
            if (!IEnemy.Enemies.Any(x => x.Id == id))
                Factory.Create($"{IElement.PATH}Enemies/Enemy{room.Enemy}.txt");
            CreateItems(IEnemy.Enemies.Find(x => x.Id == room.Enemy).Inventory);
        }
    }
    private static void Main()
    {
        Player.playTimeStart = DateTime.UtcNow;
        CultureInfo enCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = enCulture;
        Console.Title = "Agoraphobia";

        CreateRoom(0);
        Player.Inventory.Add(2);
        room.Exits.Add(0);
        room.Exits.Add(0);
        room.Exits.Add(0);
        MainScene();
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
                                    {
                                        Viewport.ClearInteraction();
                                        INPC.NPCs.Find(x => x.Id == room.NPC).Interact();
                                    }
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
                                        length += room.Exits.Count - 1;
                                        Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
                                    }
                                    break;
                                default:
                                    if (isTriggered)
                                        interaction--;
                                    if (!isOpened && room.Items.Count != 0 && interaction == length - 1)
                                    {
                                        isOpened = true;
                                        length += room.Items.Count - 1;
                                        Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
                                    }
                                    else if (!isOpened)
                                        SwitchRoom(interaction);
                                    else if (isOpened && interaction > length - room.Items.Count - 1)
                                    {
                                        int offset = 0;
                                        if (isTriggered)
                                            offset += room.Exits.Count - 1;
                                        length--;
                                        IItem.Items.Find(x => x.Id == room.Items[interaction - 2 - offset]).PickUp(room.Id);
                                        interaction = length - 1;
                                        Viewport.ShowInventory(inventory);
                                        Viewport.ClearInteraction();
                                        Viewport.Interaction(room.Id, interaction, isOpened, isTriggered);
                                        Viewport.Show(room.Id);
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
            }
            catch (ArgumentOutOfRangeException e)
            {
                ZoomOut();
            }
        }
    }
}


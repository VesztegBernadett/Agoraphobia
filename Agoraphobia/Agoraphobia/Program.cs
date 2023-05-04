using Agoraphobia;
using Agoraphobia.Entity;
using Agoraphobia.Items;
using Agoraphobia.Rooms;
using System;


Factory.Create($"{IElement.PATH}Items/Consumable0.txt");
Factory.Create($"{IElement.PATH}Items/Consumable1.txt");
Factory.Create($"{IElement.PATH}Rooms/Room0.txt");
Factory.Create($"{IElement.PATH}NPCs/NPC1.txt");
Factory.Create($"{IElement.PATH}Items/Weapon2.txt");
Factory.Create($"{IElement.PATH}Enemies/Enemy1.txt");

Player.Inventory.Add(2);
Player.Inventory.Add(1);
Player.Inventory.Add(0);

Room room = (Room)IRoom.Rooms.Find(x => x.Id == 0);

void ZoomOut()
{
    Console.Clear();
    Console.WriteLine("Please zoom out until this text is slightly smaller, but still readable. Press any key if you're ready.");
    Console.ReadKey();
    Main();
}

void Main() {
    try
    {
        Console.SetWindowSize(200, 45);

        Viewport.Show(0);
        Viewport.ShowGrid();
        Viewport.ShowStats();
        Viewport.ShowInventory(0);
        Viewport.Interaction(0, 0, false);

        int inventory = 0;
        int interaction = 1;
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
                    switch (interaction)
                    {
                        case 2:
                            if (!isOpened)
                            {
                                isOpened = true;
                                length += room.Items.Count - 1;
                                Viewport.Interaction(room.Id, interaction, isOpened);
                            }
                            break;
                        default:
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
Main();


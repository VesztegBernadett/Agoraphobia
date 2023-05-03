using Agoraphobia;
using Agoraphobia.Entity;
using Agoraphobia.Items;
using System;

Console.SetWindowSize(200, 45);

Factory.Create($"{IElement.PATH}Items/Consumable0.txt");
Factory.Create($"{IElement.PATH}Items/Consumable1.txt");
Factory.Create($"{IElement.PATH}Rooms/Room0.txt");
Factory.Create($"{IElement.PATH}NPCs/NPC1.txt");
Factory.Create($"{IElement.PATH}Items/Weapon2.txt");
Factory.Create($"{IElement.PATH}Enemies/Enemy1.txt");

Player.Inventory.Add(2);
Player.Inventory.Add(1);
Player.Inventory.Add(0);

Viewport.Show(0);
Viewport.ShowGrid();
Viewport.ShowStats();
Viewport.ShowInventory(0);

int selected = 0;
ConsoleKey input = ConsoleKey.C;

while (input != ConsoleKey.X)
{
    input = Console.ReadKey(true).Key;
    if (input == ConsoleKey.LeftArrow)
    {
        if (selected == 0)
            selected = Player.Inventory.Count - 1;
        else selected -= 1;
        Viewport.ShowInventory(selected);
    }
    else if (input == ConsoleKey.RightArrow)
    {
        if (selected == Player.Inventory.Count - 1)
            selected = 0;
        else selected += 1;
        Viewport.ShowInventory(selected);
    }
}


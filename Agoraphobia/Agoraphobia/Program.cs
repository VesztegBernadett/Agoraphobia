using Agoraphobia;
using Agoraphobia.Items;
using System;

Console.SetWindowSize(200, 45);
Factory.Create($"{IElement.PATH}Items/Consumable0.txt");
Factory.Create($"{IElement.PATH}Items/Consumable1.txt");
Factory.Create($"{IElement.PATH}Rooms/Room0.txt");
Factory.Create($"{IElement.PATH}NPCs/NPC1.txt");
Factory.Create($"{IElement.PATH}Items/Weapon2.txt");
Factory.Create($"{IElement.PATH}Enemies/Enemy1.txt");
//Console.WriteLine(consumable.Name);
//Console.WriteLine(consumable.Description);
//Console.WriteLine(consumable.art);
Viewport.Show(0);

Viewport.ShowGrid();
Console.ReadKey();

using Agoraphobia;
using Agoraphobia.Items;
using System;

Factory.Create($"{IElement.PATH}Items/Consumable0.txt");
Factory.Create($"{IElement.PATH}Items/Consumable1.txt");
Factory.Create($"{IElement.PATH}Rooms/Room0.txt");
Factory.Create($"{IElement.PATH}NPCs/NPC0.txt");
Factory.Create($"{IElement.PATH}Enemies/Enemy0.txt");
Consumable consumable = (Consumable)IItem.Items[0];
Viewport vp = new Viewport();
//Console.WriteLine(consumable.Name);
//Console.WriteLine(consumable.Description);
//Console.WriteLine(consumable.art);
vp.Show(0);


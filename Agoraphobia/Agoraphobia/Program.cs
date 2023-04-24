using Agoraphobia;
using Agoraphobia.Items;
using System;

Factory.Create($"{IElement.PATH}Items/Consumable0.txt");
Consumable consumable = (Consumable)IItem.Items[0];
Console.WriteLine(consumable.Name);
Console.WriteLine(consumable.Description);
Console.WriteLine(consumable.art);


using Agoraphobia;
using Agoraphobia.Items;

string path = "../../../Files/";
IElement consumable = Factory.Create($"{path}Items/Consumable0.txt");
Console.WriteLine(consumable.Name);
Console.WriteLine(consumable.Description);

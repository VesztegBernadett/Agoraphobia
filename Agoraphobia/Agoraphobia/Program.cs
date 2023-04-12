using Agoraphobia.Items;

string path = "../../../Files/";
Consumable consumable = new Consumable($"{path}Items/Consumable0.txt");
Console.WriteLine(consumable.Name);
Console.WriteLine(consumable.Description);

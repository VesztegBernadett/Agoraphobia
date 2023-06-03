using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agoraphobia.Rooms;
using Agoraphobia.Entity;
using Agoraphobia.Items;
using System.IO;
using System.ComponentModel;
using System.Threading;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;

namespace Agoraphobia
{
    class Viewport
    {
        private static void SelectSlot()
        {
            ConsoleKey input = ConsoleKey.DownArrow;
            int selected = -1;
            string slot1 = File.ReadAllLines($"{IElement.PATH}Player1.txt")[11].Split('#')[0];
            string slot2 = File.ReadAllLines($"{IElement.PATH}Player2.txt")[11].Split('#')[0];
            string slot3 = File.ReadAllLines($"{IElement.PATH}Player3.txt")[11].Split('#')[0];
            
            Console.Clear();
            ShowSingle(File.ReadAllText($"{IElement.PATH}Arts/Title.txt"), new int[] { 65, 2 });
            ShowSingle(File.ReadAllText($"{IElement.PATH}Arts/Book.txt"), new int[] { 74, 35 });
            do
            {
                switch (input)
                {
                    case ConsoleKey.DownArrow:
                        if (selected < 3)
                        {
                            selected++;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (selected > 0)
                        {
                            selected--;
                        }
                        break;
                }
                if (selected == 0)
                    Console.BackgroundColor = ConsoleColor.Magenta;
                ShowSingle($"------------\n| 1st slot | Last modified: {slot1}\n------------", new int[] { 92, 13 });
                Console.BackgroundColor = ConsoleColor.Black;

                if (selected == 1)
                    Console.BackgroundColor = ConsoleColor.Magenta;
                ShowSingle($"------------\n| 2nd slot | Last modified: {slot2}\n------------", new int[] { 92, 18 });
                Console.BackgroundColor = ConsoleColor.Black;

                if (selected == 2)
                    Console.BackgroundColor = ConsoleColor.Magenta;
                ShowSingle($"------------\n| 3rd slot | Last modified: {slot3}\n------------", new int[] { 92, 23 });
                Console.BackgroundColor = ConsoleColor.Black;

                if ( selected == 3)
                    Console.BackgroundColor = ConsoleColor.Magenta;
                ShowSingle("------------\n|   Back   |\n------------", new int[] { 92, 28 });
                Console.BackgroundColor = ConsoleColor.Black;

                input = Console.ReadKey(true).Key;

            } while (input!=ConsoleKey.Enter);
            if (selected == 3)
            {
                Menu();
            }
            else
            {
                Player.Slot = selected + 1;
            }
        }
        public static void Intro()
        {
            string[] text = File.ReadAllText($"{IElement.PATH}Intro.txt").Split('\n');
            for (int i = 0; i < text.Length; i++)
            {
                Console.Clear();
                Console.SetCursorPosition((200 - text[i].Length) / 2, 15);
                for (int j = 0; j < text[i].Length; j++)
                {
                    Console.Write(text[i][j]);
                    Thread.Sleep(10);
                }
                Console.WriteLine();
                Console.SetCursorPosition((200 - "(Press any key to continue)".Length) / 2, 25);
                Console.WriteLine("(Press C to continue or press S to skip)");
                ConsoleKey introinput = Console.ReadKey(true).Key;
                if (introinput == ConsoleKey.C)
                {
                    Console.ReadKey(true);
                }
                else if (introinput == ConsoleKey.S)
                {
                    i = text.Length - 1;
                }
            }
        }
        public static void Menu()
        {
            Console.Clear();
            ShowSingle(File.ReadAllText($"{IElement.PATH}Arts/Title.txt"), new int[] {65, 2});
            ShowSingle(File.ReadAllText($"{IElement.PATH}Arts/Book.txt"), new int[] { 74, 35 });
            int selected = 1;
            ConsoleKey input = ConsoleKey.UpArrow;
            while (true)
            {
                switch (input)
                {
                    case ConsoleKey.UpArrow:
                        if (selected == 0)
                            selected = 2;
                        else selected--;
                
                        break;
                    case ConsoleKey.DownArrow:
                        if (selected == 2)
                            selected = 0;
                        else selected++;
                        break;
                    case ConsoleKey.Enter:
                        switch (selected)
                        {
                            case 0:
                                SelectSlot();
                                string content = File.ReadAllText($"{IElement.PATH}Safety.txt");
                                File.WriteAllText($"{IElement.PATH}Player{Player.Slot}.txt", content);
                                return;
                            case 1:
                                SelectSlot();
                                return;
                            case 2:
                                return;
                        }
                        return;
                }
                ChooseMenuPoint(selected);
                if (Console.KeyAvailable)
                    Console.ReadKey(true);
                input = Console.ReadKey(true).Key;
            }
        }
        private static void Tutorial()
        {

        }
        private static void ChooseMenuPoint(int selected)
        {
            if (selected == 0)
                Console.BackgroundColor = ConsoleColor.Magenta;
            ShowSingle("------------\n| New Game |\n------------", new int[] { 93, 16 });
            Console.BackgroundColor = ConsoleColor.Black;
            if (selected == 1)
                Console.BackgroundColor = ConsoleColor.Magenta;
            ShowSingle("------------\n| Continue |\n------------", new int[] { 93, 21 });
            Console.BackgroundColor = ConsoleColor.Black;
            if (selected == 2)
                Console.BackgroundColor = ConsoleColor.Magenta;
            ShowSingle("------------\n| Tutorial |\n------------", new int[] { 93, 26 });
            Console.BackgroundColor = ConsoleColor.Black;
        }
        private static void ShowSingle(string art, int[] coordinates)
        {
            //Its an universal Show method so we don't need it for each class
            //New interface IArtist contains the arts so we can now access all the showable elements by IArtist
            if (art is not null)
            {
                List<string> rows = art.Split('\n').ToList();
                for (int i = 0; i < rows.Count(); i++)
                {
                    Console.SetCursorPosition(coordinates[0], coordinates[1] + i);
                    Console.Write(rows[i]);
                }
            }
        }
        public static void Show(int roomId)
        {
            ClearRoom();
            Room room = (Room)IRoom.Rooms.Find(x => x.Id == roomId);
            //NPCs
            if (room.NPC != 0)
            {
                //Id starts with 1
                NPC npc = (NPC)INPC.NPCs.Find(x => x.Id == room.NPC);
                ShowSingle(npc.Art, INPC.Coordinates);
            }
            //Enemies
            if (room.Enemy != 0)
            {
                //Id starts with 1
                Enemy enemy = (Enemy)IEnemy.Enemies.Find(x => x.Id == room.Enemy);
                ShowSingle(enemy.Art, IEnemy.Coordinates);
            }
            //Items
            if (room.Items.Count > 0)
            {
                //Id starts with 0
                if (room.Items.Count == 1)
                {
                    IItem item = IItem.Items.Find(x => x.Id == room.Items[0]);
                    ShowSingle(item.Art, IItem.Coordinates);
                }
                else
                {
                    string sack = File.ReadAllText($"{IElement.PATH}Arts/IArt.txt");
                    string[] lines = sack.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        Console.SetCursorPosition(IItem.Coordinates[0], IItem.Coordinates[1] + i);
                        Console.Write(lines[i]);
                    }
                }
            }
            ShowSingle(File.ReadAllText($"{IElement.PATH}Arts/Book.txt"), new int[] { 2, 14 });
        }

        public static void ShowGrid()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            for (int i = 0; i < 200; i++)
            {
                Console.SetCursorPosition(i, 23);
                Console.Write("_");
            }
            for (int i = 0; i < 45; i++)
            {
                Console.SetCursorPosition(120, i);
                Console.Write("|");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ShowStats()
        {
            Console.SetCursorPosition(123, 25);
            Console.Write("Statistics:");
            Console.SetCursorPosition(125, 26);
            Console.Write($"Sanity: {Player.Sanity} / 100      ");
            Console.SetCursorPosition(125, 27);
            Console.Write($"HP: {Player.HP} / {Player.MaxHP}    ");
            Console.SetCursorPosition(125, 28);
            Console.Write($"Attack: {Player.AttackDamage}      ");
            Console.SetCursorPosition(125, 29);
            Console.Write($"Defense: {Player.Defense}      ");
            Console.SetCursorPosition(125, 30);
            Console.Write($"Energy: {Player.Energy} / {Player.MaxEnergy}       ");
            Console.SetCursorPosition(125, 31);
            Console.Write($"DreamCoins: {Player.DreamCoins}       ");
            Console.SetCursorPosition(125, 32);
            Console.Write($"Inventory: {Player.Inventory.Count} / 10");
            Console.SetCursorPosition(125, 33);
            Console.Write($"Current duration: {Player.EffectDuration}");
        }

        public static void ShowInventory(int id)
        {
            Console.SetCursorPosition(157, 0);
            Console.Write("Inventory");
            int vOffset = 0;
            for (int i = 0; i < Player.Inventory.Count; i++)
            {
                if (id == i)
                    Console.BackgroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(125, 2 + i);
                Console.Write(IItem.Items.Find(x => x.Id == Player.Inventory[i]).Name);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            string text = "Hover inspect | Enter use | D Drop";
            Console.SetCursorPosition(120 + (80 - text.Length) / 2, 22);
            Console.Write(text);
        }
        private static void ShowDescription(ref int vOffset, string text, int limit, int start)
        {
            string[] words = text.Split(' ');
            int current = 0;
            vOffset++;
            for (int i = 0; i < words.Length; i++)
            {
                if (current + words[i].Length >= limit)
                {
                    vOffset++;
                    current = 0;
                }
                Console.SetCursorPosition(start + current, 24 + vOffset);
                current += words[i].Length + 1;
                Console.Write($"{words[i]} ");
            }
        }
        public static void Interaction(int roomId, int id, bool isOpened, bool isTriggered)
        {
            ClearInteraction();
            Room room = (Room)IRoom.Rooms.Find(x => x.Id == roomId);
            int selected = 0;
            int vOffset = 0;
            ShowRoomInfo(ref vOffset, room);
            vOffset++;
            if (room.NPC != 0)
            {
                vOffset++;
                NPC npc = (NPC)INPC.NPCs.Find(x => x.Id == room.NPC);
                ShowOption(ref selected, id, 0, vOffset, npc.Art);
                Console.Write($">> Interact with: {npc.Name}");
                Console.BackgroundColor = ConsoleColor.Black;
                if (selected - 1 == id)
                    ShowDescription(ref vOffset, npc.Description, 60, 15);
            }
            if (room.Enemy != 0)
            {
                vOffset++;
                Enemy enemy = (Enemy)IEnemy.Enemies.Find(x => x.Id == room.Enemy);
                ShowOption(ref selected, id, 0, vOffset, enemy.Art);
                Console.Write($">> Fight: {enemy.Name}");
                Console.BackgroundColor = ConsoleColor.Black;
                if (selected - 1 == id)
                    ShowDescription(ref vOffset, enemy.Description, 60, 15);
            }
            else
            {
                vOffset++;
                if (isTriggered)
                {
                    Console.SetCursorPosition(10, 24 + vOffset);
                    Console.Write(">> Exits:");
                    for (int i = 0; i < room.Exits.Count; i++)
                    {
                        vOffset++;
                        IRoom current = IRoom.Rooms.Find(x => x.Id == room.Exits[i]);
                        ShowOption(ref selected, id, 2, vOffset, File.ReadAllText($"{IElement.PATH}Arts/Exit.txt"));
                        Console.Write($">> Go to: {current.Name}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        if (selected - 1 == id)
                            ShowDescription(ref vOffset, current.Description, 60, 15);
                    }
                }
                else
                {
                    ShowOption(ref selected, id, 0, vOffset, File.ReadAllText($"{IElement.PATH}Arts/Exit.txt"));
                    Console.Write($">> Exit room");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            vOffset++;
            if (room.Items.Count > 1)
            {
                if (isOpened)
                {
                    Console.SetCursorPosition(10, 24 + vOffset);
                    Console.Write(">> Sack:");
                    for (int i = 0; i < room.Items.Count(); i++)
                    {
                        vOffset++;
                        IItem item = IItem.Items.Find(x => x.Id == room.Items[i]);
                        ShowOption(ref selected, id, 2, vOffset, item.Art);
                        Console.Write($">> Pick up {item.Name}");
                        Console.BackgroundColor = ConsoleColor.Black;
                        if (selected - 1 == id)
                            ShowDescription(ref vOffset, item.Description, 60, 15);
                    }
                }
                else
                {
                    ShowOption(ref selected, id, 0, vOffset, File.ReadAllText($"{IElement.PATH}Arts/IArt.txt"));
                    Console.Write(">> Inspect Sack...");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            else if (room.Items.Count == 1)
            {
                IItem item = IItem.Items.Find(x => x.Id == room.Items[0]);
                ShowOption(ref selected, id, 0, vOffset, item.Art);
                Console.Write($">> Pick up {item.Name}");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            
        }
        public static void Shop(int id)
        {
            int current = 0;
            int selected = 0;
            NPC npc = (NPC)INPC.NPCs.Find(x => x.Id == id);
            int length = npc.Inventory.Count + 1;
            ChooseItem(id, current, selected);

            while (true)
            {
                ChooseItem(id, current, selected);
                if (Console.KeyAvailable)
                    Console.ReadKey(true);
                ConsoleKey input = Console.ReadKey(true).Key;
                switch (input)
                {
                    case ConsoleKey.UpArrow:
                        if (selected == 0)
                            selected = length - 1;
                        else selected--;
                        ChooseItem(id, current, selected);
                        break;
                    case ConsoleKey.DownArrow:
                        if (selected == length - 1)
                            selected = 0;
                        else selected++;
                        ChooseItem(id, current, selected);
                        break;
                    case ConsoleKey.Enter:
                        if (selected == length - 1)
                            Program.MainScene();
                        else
                        {
                            IItem item = IItem.Items.Find(x => x.Id == npc.Inventory[selected]);
                            //Console.Write(selected);
                            //Console.ReadKey();
                            if (Player.Inventory.Count <= 18)
                            {
                                if (Player.ChangeCoins(-item.Price))
                                {
                                    length--;
                                    npc.Inventory.Remove(item.Id);
                                    Player.Inventory.Add(item.Id);
                                    ShowInventory(0);
                                    ShowStats();
                                    ClearInteraction();
                                    ChooseItem(id, current, selected);
                                }
                                else
                                {
                                    Message("Not enough DreamCoins!");
                                }
                            }
                            else Message("Your inventory is full, you can't pick up this item.");
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private static void ChooseItem(int id, int current, int selected)
        {
            ClearInteraction();
            NPC npc = (NPC)INPC.NPCs.Find(x => x.Id == id);
            int length = npc.Inventory.Count;
            if (npc.Inventory.Count() > 0)
            {
                List<int> items = npc.Inventory;
                for (int i = 0; i < items.Count; i++)
                {
                    IItem item = IItem.Items.Find(x => x.Id == items[i]);
                    ShowOption(ref current, selected, 0, current + 1, item.Art);
                    Console.Write($">> Purchase: {item.Name} - {item.Price} DC");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            else
            {
                Console.SetCursorPosition(10, 24);
                Console.Write("You can't buy anything from this creature.");
            }
            ShowOption(ref current, selected, 0, current + 2, File.ReadAllText($"{IElement.PATH}Arts/Exit.txt"));
            Console.Write(">> Exit");
            Console.BackgroundColor = ConsoleColor.Black;
        }
        private static void ShowRoomInfo (ref int vOffset, Room room)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition((120 - room.Name.Length) / 2, 0);
            Console.Write(room.Name);
            Console.BackgroundColor = ConsoleColor.Black;
            ShowDescription(ref vOffset, room.Description, 110, 5);
        }

        private static void ShowOption(ref int selected, int id, int hOffset, int vOffset, string art)
        {
            if (selected == id)
            {
                int[] coordinates = new int[] { 80, 30 };
                ShowSingle(art, coordinates);
                Console.BackgroundColor = ConsoleColor.Magenta;
            }
            Console.SetCursorPosition(10 + hOffset, 24 + vOffset);
            selected++;
        }
        public static void Message(string msg)
        {
            ClearInteraction();
            Console.SetCursorPosition(5, 25);
            Console.Write(msg);
            Console.SetCursorPosition(5, 26);
            Console.Write("Press any key to dream on.");
            Console.ReadKey();
        }

        public static void ClearInteraction()
        {
            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(5, 25 + i);
                Console.Write("                                                                                                                   ");
            }
        }
        public static void ClearRoom()
        {
            for (int i = 1; i < 22; i++)
            {
                Console.SetCursorPosition(0, 1 + i);
                Console.Write("                                                                                                                        ");
            }
        }
        public static void ShowItemInfo(int SelectedItemNumber)
        {
            int selecteditemnumber = SelectedItemNumber;
            Console.SetCursorPosition(5, 25);
            IItem selectedItem = IItem.Items.Find(x => x.Id == Player.Inventory[selecteditemnumber]);
            if (selectedItem.GetType().ToString() == "Agoraphobia.Items.Weapon")
            {
                Weapon selectedWeapon = (Weapon)selectedItem;
                Console.Write($"Weapon multiplier: {selectedWeapon.MinMultiplier}-{selectedWeapon.MaxMultiplier} Potential damage: {selectedWeapon.MinMultiplier*Player.AttackDamage}-{selectedWeapon.MaxMultiplier*Player.AttackDamage}");
            }
            else if (selectedItem.GetType().ToString() == "Agoraphobia.Items.Consumable")
            {
                Consumable selectedConsumable = (Consumable)selectedItem;
                if (selectedConsumable.Energy == 100)
                {
                    Console.Write($"This item or buff adds: Energy: {selectedConsumable.Energy}, HP: {selectedConsumable.HP} and it adds: Defense: {selectedConsumable.Armor}, Attack {selectedConsumable.Attack} and it's permanent");
                }
                else
                {
                    Console.Write($"Consumable replenishes: Energy: {selectedConsumable.Energy}, HP: {selectedConsumable.HP} and it adds: Defense: {selectedConsumable.Armor}, Attack {selectedConsumable.Attack} and it lasts for {selectedConsumable.Duration} rounds");
                }
            }
        }
    }
}

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

namespace Agoraphobia
{
    class Viewport
    {
        private static void ShowSingle(string art, int[] coordinates)
        {
            //Its an universal Show method so we don't need it for each class
            //New interface IArtist contains the arts so we can now access all the showable elements by IArtist

            List<string> rows = art.Split('\n').ToList();
            for (int i = 0; i < rows.Count(); i++)
            {
                Console.SetCursorPosition(coordinates[0], coordinates[1] + i);
                Console.Write(rows[i]);
            }
        }

        public static void Show(int roomId)
        {
            Console.Clear();
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
            string book = @"  __
 (`/\
 `=\/\ __...--~~~~~-._   _.-~~~~~--...__
  `=\/\               \ /               \\
   `=\/                V                 \\
   //_\___--~~~~~~-._  |  _.-~~~~~~--...__\\
  //  ) (..----~~~~._\ | /_.~~~~----.....__\\
 ===( INK )==========\\|//====================  
__ejm\___/________dwb`---`____________________________________________";
            string[] lines = book.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(2, 14 + i);
                Console.Write(lines[i]);
            }
        }

        public static void ShowStats()
        {
            Console.SetCursorPosition(123, 25);
            Console.Write("Statistics:");
            Console.SetCursorPosition(125, 26);
            Console.Write($"Sanity: {Player.Sanity} / 100");
            Console.SetCursorPosition(125, 27);
            Console.Write($"HP: {Player.MaxHP} / {Player.HP}");
            Console.SetCursorPosition(125, 28);
            Console.Write($"Attack: {Player.AttackDamage}");
            Console.SetCursorPosition(125, 29);
            Console.Write($"Defense: {Player.Defense}");
            Console.SetCursorPosition(125, 30);
            Console.Write($"Energy: {Player.MAXENERGY} / {Player.Energy}");
            Console.SetCursorPosition(125, 31);
            Console.Write($"DreamCoins: {Player.DreamCoins}");
        }

        public static void ShowInventory(int id)
        {
            Console.SetCursorPosition(157, 0);
            Console.Write("Inventory");
            for (int i = 0; i < Player.Inventory.Count; i++)
            {
                if (id == i)
                    Console.BackgroundColor = ConsoleColor.Magenta;
                Console.SetCursorPosition(125, 2 + i);
                Console.Write(IItem.Items.Find(x => x.Id == Player.Inventory[i]).Name);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.SetCursorPosition(148, 22);
            Console.Write(". Inspect | + Use | - Drop");
        }

        public static void Interaction(int roomId, int id, bool isOpened)
        {
            Room room = (Room)IRoom.Rooms.Find(x => x.Id == roomId);
            int height = room.Intro.Length / 110 + 1;
            int selected = 0;
            int vOffset = 0;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition((120 - room.Name.Length) / 2, 0);
            Console.Write(room.Name);
            Console.BackgroundColor = ConsoleColor.Black;

            string[] words = room.Intro.Split(' ');
            int current = 0;
            for (int i = 0; i < words.Length; i++)
            {
                if (current + words[i].Length >= 110)
                {
                    vOffset++;
                    current = 0;
                }
                Console.SetCursorPosition(5 + current, 25 + vOffset);
                current += words[i].Length + 1;
                Console.Write($"{words[i]} ");
            }

            if (room.NPC != 0)
            {
                NPC npc = (NPC)INPC.NPCs.Find(x => x.Id == room.NPC);
                ShowOption(ref selected, id, 0, 0 + height, npc.Art);
                Console.Write($">> Interact with: {npc.Name}");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            if (room.Enemy != 0)
            {
                Enemy enemy = (Enemy)IEnemy.Enemies.Find(x => x.Id == room.Enemy);
                ShowOption(ref selected, id, 0, 0 + height, enemy.Art);
                Console.Write($">> Fight: {enemy.Name}");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            if (room.Items.Count > 1)
            {

                if (isOpened)
                {
                    Console.SetCursorPosition(10, 26 + selected + height);
                    Console.Write(">> Sack:          ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    for (int i = 0; i < room.Items.Count(); i++)
                    {
                        IItem item = IItem.Items.Find(x => x.Id == room.Items[i]);
                        ShowOption(ref selected, id, 2, 1 + height, item.Art);
                        Console.Write($">> Pick up {item.Name}");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
                else
                {
                    ShowOption(ref selected, id, 0, 0 + height, File.ReadAllText($"{IElement.PATH}Arts/IArt.txt"));
                    Console.Write(">> Inspect Sack...");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            else if (room.Items.Count == 1)
            {
                IItem item = IItem.Items.Find(x => x.Id == room.Items[0]);
                ShowOption(ref selected, id, 0, 0 + height, item.Art);
                Console.Write($">> Pick up {item.Name}");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        private static void ShowOption(ref int selected, int id, int hOffset, int vOffset, string art)
        {
            if (selected == id)
            {
                ShowSingle(art, new int[] {80, 30});
                Console.BackgroundColor = ConsoleColor.Magenta;
            }
            Console.SetCursorPosition(10 + hOffset, 26 + selected + vOffset);
            selected++;
        }
    }
}

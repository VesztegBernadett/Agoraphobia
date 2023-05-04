using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agoraphobia.Rooms;
using Agoraphobia.Entity;
using Agoraphobia.Items;

namespace Agoraphobia
{
    class Viewport
    {
        private static void ShowSingle(IArtist element)
        {
            //Its an universal Show method so we don't need it for each class
            //New interface IArtist contains the arts so we can now access all the showable elements by IArtist
            int[] Coordinates = new int[2];
            string type = element.GetType().ToString();
            if (type == "Agoraphobia.Entity.NPC")
            {
                Coordinates = INPC.Coordinates;
            } else if (type == "Agoraphobia.Entity.Enemy")
            {
                Coordinates = IEnemy.Coordinates;
            }
            else
            {
                Coordinates = IItem.Coordinates;
            }

            List<string> rows = element.Art.Split('\n').ToList();
            for (int i = 0; i < rows.Count(); i++)
            {
                Console.SetCursorPosition(Coordinates[0], Coordinates[1] + i);
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
                ShowSingle(npc);
            }

            //Enemies
            if (room.Enemy != 0)
            {
                //Id starts with 1
                Enemy enemy = (Enemy)IEnemy.Enemies.Find(x => x.Id == room.Enemy);
                ShowSingle(enemy);
            }

            //Items
            if (room.Items.Count > 0)
            {
                //Id starts with 0
                if (room.Items.Count == 1)
                {
                    IItem item = IItem.Items.Find(x => x.Id == room.Items[0]);
                    ShowSingle(item);
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
            int selected = 0;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition((120 - room.Name.Length) / 2, 0);
            Console.Write(room.Name);
            Console.BackgroundColor = ConsoleColor.Black;
            
            if (room.NPC != 0)
            {
                ShowOption(ref selected, id, 0, 0);
                Console.Write($">> Interact with: {INPC.NPCs.Find(x => x.Id == room.NPC).Name}");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            if (room.Enemy != 0)
            {
                ShowOption(ref selected, id, 0, 0);
                Console.Write($">> Fight: {IEnemy.Enemies.Find(x => x.Id == room.Enemy).Name}");
                Console.BackgroundColor = ConsoleColor.Black;
            }
            if (room.Items.Count > 1)
            {

                if (isOpened)
                {
                    Console.SetCursorPosition(3, 25 + selected);
                    Console.Write(">> Sack:          ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    for (int i = 0; i < room.Items.Count(); i++)
                    {
                        ShowOption(ref selected, id, 2, 1);
                        Console.Write($">> Pick up {IItem.Items.Find(x => x.Id == room.Items[0]).Name}");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }
                else
                {
                    ShowOption(ref selected, id, 0, 0);
                    Console.Write(">> Inspect Sack...");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            else if (room.Items.Count == 1)
            {
                ShowOption(ref selected, id, 0, 0);
                Console.Write($">> Pick up {IItem.Items.Find(x => x.Id == room.Items[0]).Name}");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }

        private static void ShowOption(ref int selected, int id, int hOffset, int vOffset)
        {
            if (selected == id)
                Console.BackgroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(3 + hOffset, 25 + selected + vOffset);
            selected++;
        }
    }
}

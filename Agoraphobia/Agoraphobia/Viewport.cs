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
        public void ShowSingle(IArtist element)
        {
            //Its an universal Show method so we don't need it for each class
            //New interface IArtist contains the arts so we can now access all the showable elements by IArtist
            int[] Coordinates = new int[2];
            string type = element.GetType().ToString();
            if (type=="Agoraphobia.Entity.NPC")
            {
                Coordinates=INPC.Coordinates;
            }else if (type=="Agoraphobia.Entity.Enemy")
            {
                Coordinates=IEnemy.Coordinates;
            }
            else
            {
                Coordinates=IItem.Coordinates;
            }

            List<string> rows = element.Art.Split('\n').ToList();
            for (int i = 0; i < rows.Count(); i++)
            {
                Console.SetCursorPosition(Coordinates[0], Coordinates[1] + i);
                Console.Write(rows[i]);
            }
        }

        public void Show(int roomId)
        {
            Console.Clear();
            Room room = (Room)IRoom.Rooms[roomId];

            //NPCs
            if (room.NPCs.Count > 0)
            {
                Console.SetCursorPosition(INPC.Coordinates[0], INPC.Coordinates[1]);
                NPC npc = (NPC)INPC.NPCs.Find(x => x.Id == room.NPCs[0]);
                ShowSingle(npc);
            }

            //Enemies
            if (room.Enemies.Count > 0)
            {
                Console.SetCursorPosition(IEnemy.Coordinates[0], IEnemy.Coordinates[1]);
                Enemy enemy = (Enemy)IEnemy.Enemies.Find(x => x.Id == room.Enemies[0]);
                ShowSingle(enemy);
            }

            //Items
            if (room.Items.Count > 0)
            {
                Console.SetCursorPosition(IItem.Coordinates[0], IItem.Coordinates[1]);
                IItem item = IItem.Items.Find(x => x.Id == room.Items[0]);
                ShowSingle(item);
            }
        }

    }
}

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

        public void Show(int roomId)
        {
            Console.Clear();
            Room room = (Room)IRoom.Rooms[roomId];

            //NPCs
            if (room.NPCs.Count>0)
            {
                Console.SetCursorPosition(INPC.Coordinates[0], INPC.Coordinates[1]);
                NPC npc = (NPC)INPC.NPCs.Find(x => x.Id == room.NPCs[0]);
                Console.Write(npc.art);
            }

            //Enemies
            if (room.Enemies.Count > 0)
            {
                Console.SetCursorPosition(IEnemy.Coordinates[0], IEnemy.Coordinates[1]);
                Enemy enemy = (Enemy)IEnemy.Enemies.Find(x => x.Id == room.Enemies[0]);
                Console.Write(enemy.art);
            }

            //Items
            if (room.Items.Count > 0)
            {
                Console.SetCursorPosition(IItem.Coordinates[0], IItem.Coordinates[1]);
                IItem item = IItem.Items.Find(x => x.Id == room.Items[0]);
                Console.Write(item.art);
            }
        }

    }
}

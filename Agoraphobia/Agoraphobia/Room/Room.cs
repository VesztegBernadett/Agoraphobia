using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Agoraphobia.Entity;

namespace Agoraphobia.Rooms
{
    internal class Room : IRoom
    {
        private readonly int id;
        public int Id { get => id; }
        private readonly string name;
        public string Name { get => name; }
        private readonly string description;
        public string Description { get => description; }
        public int NPC { get; private set; }
        public int Enemy { get; private set; }
        public List<int> Items { get; private set; }
        public List<int> Exits { get; private set; } = new List<int>(3);
        public Room (int id, string name, string desc, int npc, int enemy, List<int> items)
        {
            this.id = id;
            this.name = name;
            description = desc;
            NPC = npc;
            Items = items;
            Enemy = enemy;
            IRoom.Rooms.Add(this);
        }
        public void RemoveEnemy()
        {
            Enemy = 0;
        }
    }
}

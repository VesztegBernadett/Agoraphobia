using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        public List<int> NPCs { get; }
        public List<int> Enemies { get; }
        public List<int> Items { get; }
        public List<int> Exits { get; private set; }
        public int ItemsNum { get; set; }
        public IRoom.Orientation Orientation { get; private set; }
        public bool IsQuest { get; private set; }
        public string View()
        {
            return $"";
        }
        public Room (int id, string name, string desc, bool type, int orientation, List<int> npcs, List<int> enemies, List<int> items, List<int> exits)
        {
            this.id = id;
            this.name = name;
            description = desc;
            IsQuest = type;
            Orientation = (IRoom.Orientation)orientation;
            NPCs = npcs;
            Items = items;
            Enemies = enemies;
            Exits = exits;
            ItemsNum = items.Count;
            IRoom.Rooms.Add(this);
        }
    }
}

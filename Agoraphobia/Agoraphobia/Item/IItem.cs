using Agoraphobia.Entity;
using Agoraphobia.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agoraphobia
{
    internal interface IItem : IElement, IArtist
    {
        static int[] Coordinates = {80, 8};
        static List<IItem> Items = new List<IItem>();//Add the instance to this list in the constructor
        void PickUp(int roomId)
        {
            IRoom.Rooms.Find(x => x.Id == roomId).Items.Remove(Id);
            Player.Inventory.Add(Id);
        }
        string Inspect();
        void Drop();
        void Delete();
        ItemRarity Rarity { get; set; }
        int Price { get; }

        enum ItemRarity
        {
            Common,
            Uncommon,
            Rare,
            Epic,
            Legendary,
            Fabled
        };
    }
}

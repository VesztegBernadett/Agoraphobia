using Agoraphobia.Entity;
using Agoraphobia.Items;
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
        public static Dictionary<ItemRarity, int> ItemValue = new Dictionary<ItemRarity, int>() {
                {ItemRarity.Common, 1 },
                {ItemRarity.Uncommon, 2 },
                {ItemRarity.Rare, 3 },
                {ItemRarity.Epic, 4 },
                {ItemRarity.Legendary, 5 },
                {ItemRarity.Fabled, 6 },
            };
        void PickUp(int roomId);
        string Inspect();
        void Drop(int roomId, int id)
        {
            IRoom.Rooms.Find(x => x.Id == roomId).Items.Add(Id);
            Player.Inventory.RemoveAt(Player.Inventory.LastIndexOf(id));
            if (IItem.Items.Find(x => x.Id == id).GetType().ToString() == "Agoraphobia.Items.Weapon" && Player.Inventory.Any(x => x == id))
            {
                Weapon weapon = (Weapon)IItem.Items.Find(x => x.Id == id);
                weapon.LevelDown();
            }
        }
        void Delete(int itemID)
        {
            Player.Inventory.Remove(itemID);
        }
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

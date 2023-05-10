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

            // this is cancer
            Dictionary<ItemRarity, int> itemValue = new Dictionary<ItemRarity, int>() {
                {ItemRarity.Common, 1 },
                {ItemRarity.Uncommon, 2 },
                {ItemRarity.Rare, 3 },
                {ItemRarity.Epic, 4 },
                {ItemRarity.Legendary, 5 },
                {ItemRarity.Fabled, 6 },
            };

            // TODO: count items that the player purchased not picked up
            IItem i = Items.Find(x => x.Id == Id);
            Player.Points += 10 * itemValue[i.Rarity];
        }
        string Inspect();
        void Drop();
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

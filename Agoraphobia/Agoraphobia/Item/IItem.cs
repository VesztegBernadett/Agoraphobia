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
        void Obtain();
        string Inspect()
        {
            if (this.GetType().ToString() == "Agoraphobia.Items.Consumable")
            {
                Consumable item = (Consumable)this;
                if (item.Duration == 100)
                    return $"{item.Name}\n     {item.Description}\n\n     Type: Consumable\n     Rarity: {item.Rarity}\n     Duration: Infinite\n     MaxHP: {item.HP}\n     Armor: {item.Armor}\n     Attack: {item.Attack}\n     MaxEnergy: {item.Energy}";
                else return $"{item.Name}\n     {item.Description}\n\n     Type: Consumable\n     Rarity: {item.Rarity}\n     Duration: {item.Duration}\n     HP: {item.HP}\n     Armor: {item.Armor}\n     Attack: {item.Attack}\n     Energy: {item.Energy}";
            }
            else
            {
                Weapon item = (Weapon)this;
                return $"{item.Name}\n     {item.Description}\n\n     Type: Weapon\n     Rarity: {item.Rarity}\n     Multiplier: {item.MinMultiplier}-{item.MaxMultiplier}\n     Energy cost: {item.Energy}";
            }
        }
        void Drop(int id)
        {
            IRoom.Rooms.Find(x => x.Id == Program.room.Id).Items.Add(Id);
            Player.Inventory.RemoveAt(Player.Inventory.LastIndexOf(id));
            if (IItem.Items.Find(x => x.Id == id).GetType().ToString() == "Agoraphobia.Items.Weapon" && Player.Inventory.Any(x => x == id))
            {
                Weapon weapon = (Weapon)IItem.Items.Find(x => x.Id == id);
                weapon.LevelDown();
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatheredFugitive
{
    [System.Serializable]
    public class InventoryItem
    {
        public string _itemName;
        public int _quantity;

        public InventoryItem(string itemName, int quantity)
        {
            this._itemName = itemName;
            this._quantity = quantity;
        }
    }

    [System.Serializable]
    public class Inventory
    {
        public List<InventoryItem> _items = new List<InventoryItem>();

        public void AddItem(string itemName, int quantity)
        {
            InventoryItem item = _items.Find(i => i._itemName == itemName);
            if (item != null)
            {
                item._quantity += quantity;
            }
            else
            {
                _items.Add(new InventoryItem(itemName, quantity));
            }
        }

        public void RemoveItem(string itemName, int quantity)
        {
            InventoryItem item = _items.Find(i => i._itemName == itemName);
            if (item != null)
            {
                item._quantity -= quantity;
                if (item._quantity <= 0)
                    _items.Remove(item);
            }
        }
    }

}
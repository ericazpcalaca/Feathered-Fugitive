using System.IO;
using UnityEngine;

namespace FeatheredFugitive
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        private Inventory _inventory = new Inventory();
        private string _filePath;

        private void Start()
        {
            _filePath = Path.Combine(Application.persistentDataPath, "inventory.json");
            LoadInventory();
        }

        public void SaveInventory()
        {
            string json = JsonUtility.ToJson(_inventory);
            File.WriteAllText(_filePath, json);
            //Debug.Log("Inventory saved to " + _filePath);
        }

        public void LoadInventory()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _inventory = JsonUtility.FromJson<Inventory>(json);
                //Debug.Log("Inventory loaded from " + _filePath);
            }
            else
            {
                //Debug.Log("No inventory file found, creating a new one.");
                _inventory = new Inventory();
            }
        }

        public void AddItem(string itemName, int quantity)
        {
            _inventory.AddItem(itemName, quantity);
            SaveInventory();
        }

        public void RemoveItem(string itemName, int quantity)
        {
            _inventory.RemoveItem(itemName, quantity);
            SaveInventory();
        }

        public void DisplayInventory()
        {
            foreach (var item in _inventory._items)
            {
                Debug.Log($"Item: {item._itemName}, Quantity: {item._quantity}");
            }
        }

        public bool CheckItem(string itemName)
        {
            InventoryItem item = _inventory._items.Find(i => i._itemName == itemName);

            return item != null;
        }
    }
}

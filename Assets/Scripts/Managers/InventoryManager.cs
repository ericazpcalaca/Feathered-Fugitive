using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FeatheredFugitive
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        private Inventory _inventory = new Inventory();
        private string _filePath;

        [SerializeField] private Transform _containerInventory;  
        [SerializeField] private GameObject _buttonPrefab;    

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
                DisplayInventoryOnUI();
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
            DisplayInventoryOnUI();
        }

        public void RemoveItem(string itemName, int quantity)
        {
            _inventory.RemoveItem(itemName, quantity);
            SaveInventory();
            DisplayInventoryOnUI();
        }

        // Delete later. For debug purposes now
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

        public void DisplayInventoryOnUI()
        {
            foreach (var item in _inventory._items)
            {
                GameObject button = Instantiate(_buttonPrefab, _containerInventory);
                if (button == null)
                {
                    Debug.LogError($"[{typeof(InventoryManager).Name}] - Can't instantiate button from prefab!");
                    continue;
                }

                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                if (buttonText == null)
                {
                    Debug.LogError($"[{typeof(InventoryManager).Name}] - Can't instantiate buttonText from prefab!");
                    continue;
                }
                buttonText.text = $"{item._itemName}";
            }
        }
    }
}

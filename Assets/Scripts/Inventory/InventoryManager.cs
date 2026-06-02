using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Data Pemain")]
    public int totalMoney = 0; // Uang pusat (termasuk hasil jualan)
    public List<ItemData> collectedItems = new List<ItemData>();

    [Header("Referensi UI")]
    public InventoryUI inventoryUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else Destroy(gameObject);
    }

    // Dipanggil oleh PlayerShop (saat jual balon) atau CollectibleItem (saat nemu duit)
    public void AddMoney(int amount)
    {
        totalMoney += amount;
        Debug.Log("Uang bertambah! Total: " + totalMoney);

        // Update tampilan inventory kalau sedang terbuka
        if (inventoryUI != null) inventoryUI.UpdateDisplay();
    }

    // Dipanggil oleh CollectibleItem
    public void AddItem(ItemData item)
    {
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
            Debug.Log("Item baru masuk tas: " + item.itemName);
        }
        if (inventoryUI != null) inventoryUI.UpdateDisplay();
        
    }
}
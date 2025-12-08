using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance; // Singleton agar mudah diakses

    // List untuk menyimpan item yang dimiliki player
    public List<CultureItemData> collection = new List<CultureItemData>();

    // Event untuk memberitahu UI kalau ada perubahan data
    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Fungsi untuk menambah item (dipanggil saat player mengambil item/membeli)
    public void AddItem(CultureItemData item)
    {
        collection.Add(item);

        // Update UI
        if (onInventoryChangedCallback != null)
            onInventoryChangedCallback.Invoke();

        Debug.Log("Item ditambahkan: " + item.itemName);
    }

    // Fungsi menghapus item (jika dijual/dibuang)
    public void RemoveItem(CultureItemData item)
    {
        if (collection.Contains(item))
        {
            collection.Remove(item);

            if (onInventoryChangedCallback != null)
                onInventoryChangedCallback.Invoke();
        }
    }
}

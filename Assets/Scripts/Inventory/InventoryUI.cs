using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;   // Drag object 'Content' di sini
    public GameObject slotPrefab;   // Drag 'InventorySlotPrefab' di sini

    InventorySystem inventory;

    void Start()
    {
        inventory = InventorySystem.Instance;
        // Subscribe ke event, jadi kalau data berubah, UI update otomatis
        inventory.onInventoryChangedCallback += UpdateUI;

        UpdateUI(); // Update pertama kali
    }

    void UpdateUI()
    {
        // 1. Hapus semua slot lama (reset)
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // 2. Buat slot baru sesuai jumlah item di inventory
        foreach (CultureItemData item in inventory.collection)
        {
            GameObject newSlot = Instantiate(slotPrefab, itemsParent);
            InventorySlot slotScript = newSlot.GetComponent<InventorySlot>();

            if (slotScript != null)
            {
                slotScript.SetupSlot(item);
            }
        }
    }
}
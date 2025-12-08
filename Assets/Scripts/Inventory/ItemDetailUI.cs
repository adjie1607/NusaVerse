using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailUI : MonoBehaviour
{
    public static ItemDetailUI Instance; // Singleton biar mudah dipanggil dari slot manapun

    [Header("UI Components")]
    public GameObject contentParent; // Objek yang menampung gambar/text (bisa panel itu sendiri)
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    private void Awake()
    {
        // Set Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Pastikan panel tertutup saat game mulai
        CloseDetail();
    }

    // Fungsi ini dipanggil oleh InventorySlot saat diklik
    public void OpenDetail(CultureItemData item)
    {
        if (item == null) return;

        // 1. Isi data UI
        itemIcon.sprite = item.icon;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;

        // 2. Munculkan Panel
        contentParent.SetActive(true);
    }

    // Pasang fungsi ini di tombol Close (X)
    public void CloseDetail()
    {
        contentParent.SetActive(false);
    }
}
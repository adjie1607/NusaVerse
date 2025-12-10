using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetailUI : MonoBehaviour
{
    public static ItemDetailUI Instance;

    [Header("UI Components")]
    public GameObject contentParent;
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        CloseDetail();
    }

    public void OpenDetail(CultureItemData item)
    {
        if (item == null) return;

        // 1. Isi Data
        itemIcon.sprite = item.icon;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;

        // 2. Munculkan Panel
        contentParent.SetActive(true);

        // 3. PENTING: Bebaskan Kursor Mouse biar bisa nge-klik!
        Cursor.lockState = CursorLockMode.None; // Lepas kuncian kamera
        Cursor.visible = true;                  // Munculkan gambar panah mouse
    }

    public void CloseDetail()
    {
        // 1. Sembunyikan Panel
        contentParent.SetActive(false);

        // 2. PENTING: Kunci lagi Kursor Mouse biar bisa jalan/nengok lagi
        Cursor.lockState = CursorLockMode.Locked; // Kunci ke tengah layar
        Cursor.visible = false;                   // Sembunyikan gambar panah
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggler : MonoBehaviour
{
    public GameObject inventoryPanelUI; // Drag Panel Inventory (diri sendiri) ke sini
    public KeyCode toggleKey = KeyCode.I; // Tombol untuk buka/tutup (Default 'I')

    void Start()
    {
        // Saat game mulai, pastikan inventory tertutup
        inventoryPanelUI.SetActive(false);
    }

    void Update()
    {
        // Jika tombol ditekan
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        // Cek kondisi sekarang (Nyala atau Mati?)
        bool isActive = inventoryPanelUI.activeSelf;

        // Ubah jadi kebalikannya (Nyala -> Mati, Mati -> Nyala)
        inventoryPanelUI.SetActive(!isActive);

        // Opsional: Kunci kursor mouse saat inventory terbuka
        if (!isActive)
        {
            // Inventory Terbuka -> Munculkan Mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Inventory Tertutup -> Sembunyikan Mouse (untuk game FPS)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}

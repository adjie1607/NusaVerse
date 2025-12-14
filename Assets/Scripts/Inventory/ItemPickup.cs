using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public CultureItemData itemData; // Masukkan ScriptableObject item di sini (Inspector)

    private bool isPlayerNearby = false;

    // Deteksi kalau player masuk area item
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // Opsional: Munculkan teks kecil "Tekan E untuk ambil" di sini
            Debug.Log("Tekan E untuk mengambil " + itemData.itemName);
        }
    }

    // Deteksi kalau player menjauh
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void Update()
    {
        // Jika player dekat DAN tekan tombol E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    void PickUp()
    {
        
        InventorySystem.Instance.AddItem(itemData);

      
        ItemDetailUI.Instance.OpenDetail(itemData);

       
        Destroy(gameObject);
    }
}

using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Data Item Budaya")]
    public ItemData itemData; // Drag ScriptableObject item (Keris/Batik/dll) kesini. Boleh kosong.

    [Header("Hadiah EXP")]
    public int expAmount = 20; // Jumlah EXP yang didapat saat ambil item ini

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Masukkan Item ke Inventory (Jika ini adalah item koleksi)
            if (itemData != null)
            {
                // Pastikan InventoryManager ada
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.AddItem(itemData);
                }
            }

            // 2. Tambah EXP ke Player
            // Kita cari script PlayerLevel yang nempel di badan Player
            PlayerLevel playerLevel = other.GetComponent<PlayerLevel>();

            if (playerLevel != null)
            {
                // Panggil fungsi tambah EXP (sesuaikan nama fungsinya: AddExp atau GainEXP)
                playerLevel.AddExp(expAmount);
                Debug.Log($"Item diambil! Dapat {expAmount} EXP.");
            }
            else
            {
                Debug.LogWarning("Script PlayerLevel tidak ditemukan di Player!");
            }

            // 3. Efek Suara/Partikel (Opsional, tambahkan jika punya)
            // AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // 4. Hapus benda ini dari dunia
            Destroy(gameObject);
        }
    }
}
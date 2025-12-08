using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image iconDisplay;
    public TextMeshProUGUI nameDisplay;
    public Button slotButton; // Tambahan: Referensi ke component Button

    CultureItemData itemData;

    public void SetupSlot(CultureItemData newItem)
    {
        itemData = newItem;
        iconDisplay.sprite = itemData.icon;
        iconDisplay.enabled = true;
        nameDisplay.text = itemData.itemName;

        // Hapus listener lama biar gak numpuk, lalu tambah listener baru
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(OnSlotClicked);
    }

    // Fungsi saat slot diklik
    void OnSlotClicked()
    {
        // Panggil script Detail UI yang kita buat di Tahap 2
        if (ItemDetailUI.Instance != null)
        {
            ItemDetailUI.Instance.OpenDetail(itemData);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("Komponen UI di Prefab")]
    public Image iconDisplay;
    public TMP_Text nameDisplay;
    public Button slotButton;

    // Fungsi ini dipanggil oleh InventoryUI untuk mengisi data
    public void SetupSlot(ItemData item, System.Action<ItemData> onClickCallback)
    {
        // 1. Set Icon
        if (item.icon != null)
        {
            iconDisplay.sprite = item.icon;
            iconDisplay.enabled = true; // Pastikan nyala
        }
        else
        {
            iconDisplay.enabled = false; // Matikan kalau ga ada gambar
        }

        // 2. Set Nama
        if (nameDisplay != null)
        {
            nameDisplay.text = item.itemName;
        }

        // 3. Set Tombol Klik
        slotButton.onClick.RemoveAllListeners(); // Hapus event lama biar ga numpuk
        slotButton.onClick.AddListener(() => onClickCallback(item));
    }
}
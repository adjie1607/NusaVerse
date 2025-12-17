using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Panel Utama")]
    public GameObject inventoryPanel; // Panel besar inventory
    public TMP_Text moneyText;

    [Header("Item Slot System")]
    public Transform contentArea;   // Tempat nge-spawn list (biasanya di dalam ScrollView)
    public GameObject slotPrefab;   // Prefab tombol item

    [Header("Detail Info Area")]
    public GameObject detailPanel;  // Panel pop-up penjelasan
    public Image detailImage;
    public TMP_Text detailName;
    public TMP_Text detailDescription;

    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false); // Sembunyikan saat mulai
        detailPanel.SetActive(false);
    }

    void Update()
    {
        // Tekan I untuk buka/tutup inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if (isOpen)
        {
            UpdateDisplay();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // Bebaskan mouse
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Kunci mouse lagi (kalau game FPS/TPS)
        }
    }

    public void UpdateDisplay()
    {
        // 1. Update Uang
        if (moneyText != null)
            moneyText.text = "Rp " + InventoryManager.Instance.money.ToString();

        // 2. Bersihkan slot lama biar gak dobel
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        // 3. Spawn slot baru sesuai data inventory
        foreach (ItemData item in InventoryManager.Instance.collectedItems)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentArea);

            // Cari komponen Image dan Text di dalam prefab slot
            // Asumsi: Slot punya Image (Icon) dan Button
            Image iconImg = newSlot.transform.Find("Icon").GetComponent<Image>();
            TMP_Text nameTxt = newSlot.transform.Find("NameText").GetComponent<TMP_Text>();
            Button btn = newSlot.GetComponent<Button>();

            iconImg.sprite = item.icon;
            nameTxt.text = item.itemName;

            // Saat tombol diklik, tampilkan detail budaya
            btn.onClick.AddListener(() => ShowDetail(item));
        }
    }

    void ShowDetail(ItemData item)
    {
        detailPanel.SetActive(true);
        detailImage.sprite = item.icon;
        detailName.text = item.itemName;
        detailDescription.text = item.description;
    }

    public void CloseDetail()
    {
        detailPanel.SetActive(false);
    }
}
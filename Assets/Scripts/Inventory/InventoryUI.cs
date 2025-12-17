using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Panel Utama")]
    public GameObject inventoryPanel;
    public TMP_Text moneyTextUI;

    [Header("Item Slot System")]
    public Transform contentArea;
    public GameObject slotPrefab;

    [Header("Detail Info Area")]
    public GameObject detailPanel;
    public Image detailImage;
    public TMP_Text detailName;
    public TMP_Text detailDescription;

    public static bool isInventoryOpen = false;
    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
        detailPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        

        inventoryPanel.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            UpdateDisplay();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else
        {
            // === SAAT INVENTORY TUTUP ===

            // Sembunyikan Kursor & Kunci ke tengah layar
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            // Opsional: Lanjut Waktu
            Time.timeScale = 1f;
        }
    }

    public void UpdateDisplay()
    {
        // 1. Bersihkan item lama
        foreach (Transform child in contentArea) Destroy(child.gameObject);

        // 2. Spawn item baru
        if (InventoryManager.Instance != null)
        {
            foreach (ItemData item in InventoryManager.Instance.collectedItems)
            {
                GameObject newSlot = Instantiate(slotPrefab, contentArea);

                // Reset Transform biar gak ngaco
                newSlot.transform.localScale = Vector3.one;
                newSlot.transform.localPosition = Vector3.zero;

                InventorySlot slotScript = newSlot.GetComponent<InventorySlot>();
                if (slotScript != null) slotScript.SetupSlot(item, ShowDetail);
            }
        }

        // 3. JALANKAN COROUTINE BUAT MAKSA UPDATE (Ini kuncinya!)
        if (gameObject.activeInHierarchy) // Cek biar ga error kalau panel mati
        {
            StartCoroutine(FixLayoutGlitch());
        }
    }

    // Jurus Paksa Update Layout
    System.Collections.IEnumerator FixLayoutGlitch()
    {
        // Tunggu sampai frame selesai (biar item bener-bener udah lahir)
        yield return new WaitForEndOfFrame();

        // Paksa Content Size Fitter mikir ulang
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentArea.GetComponent<RectTransform>());

        // Tunggu lagi dikit (opsional, kadang layout butuh 2 frame)
        yield return null;

        // Paksa lagi (Double tap biar yakin)
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentArea.GetComponent<RectTransform>());
    }

    // Fungsi ini dikirim ke slot untuk dipanggil saat diklik
    void ShowDetail(ItemData item)
    {
        detailPanel.SetActive(true);
        if (detailImage) detailImage.sprite = item.icon;
        if (detailName) detailName.text = item.itemName;
        if (detailDescription) detailDescription.text = item.description;
    }

    public void CloseDetail()
    {
        detailPanel.SetActive(false);
    }
}
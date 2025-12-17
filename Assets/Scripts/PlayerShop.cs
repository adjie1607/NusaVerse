using UnityEngine;
using TMPro;

public class PlayerShop : MonoBehaviour
{
    [Header("Data Toko")]
    public int balloonStock = 10;
    public int balloonPrice = 5;

    // (Uang dihapus dari sini, kita pakai uang di InventoryManager)

    [Header("EXP System")]
    public PlayerLevel playerLevel;
    public int expPerSale = 10;

    [Header("UI & FX")]
    public TMP_Text balloonText;
    public TMP_Text moneyText; // Menampilkan uang dari InventoryManager
    public GameObject floatingExpPrefab;
    public Transform floatingSpawnPoint;
    public AudioSource sellSound;
    public AudioSource expSound;

    void Update()
    {
        // Update UI terus menerus agar sinkron dengan Inventory
        UpdateUI();
    }

    public bool JualBarang()
    {
        if (balloonStock > 0)
        {
            balloonStock--;

            // === PERUBAHAN UTAMA DISINI ===
            // Setor uang ke InventoryManager
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddMoney(balloonPrice);
            }
            // ==============================

            if (playerLevel != null) playerLevel.AddExp(expPerSale);

            // FX logic
            if (floatingExpPrefab != null && floatingSpawnPoint != null)
            {
                GameObject fx = Instantiate(floatingExpPrefab, floatingSpawnPoint.position, Quaternion.identity);
                var floatText = fx.GetComponent<FloatingText>();
                if (floatText != null) floatText.Setup($"+{expPerSale} EXP", Color.yellow);
            }
            if (expSound != null) expSound.Play();
            if (sellSound != null) sellSound.Play();

            return true;
        }
        else
        {
            return false;
        }
    }

    void UpdateUI()
    {
        if (balloonText != null) balloonText.text = $"Balon: {balloonStock}";

        // Ambil info uang dari InventoryManager
        if (InventoryManager.Instance != null && moneyText != null)
        {
            moneyText.text = $"Uang: {InventoryManager.Instance.totalMoney}";
        }
    }
}
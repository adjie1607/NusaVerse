using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShop : MonoBehaviour
{
    [Header("Data")]
    public int balloonStock = 10;
    public int balloonPrice = 5;
    public int playerMoney = 0;

    [Header("EXP System")]
    public PlayerLevel playerLevel;   // Reference ke script PlayerLevel
    public int expPerSale = 10;       // Exp yang didapat tiap jual balon

    [Header("UI - TextMeshPro")]
    public TMP_Text balloonText;
    public TMP_Text moneyText;

    [Header("FX & Audio")]
    public GameObject floatingExpPrefab;
    public Transform floatingSpawnPoint;
    public AudioSource sellSound;
    public AudioSource expSound;

    void Start()
    {
        UpdateUI();
    }

    // Fungsi ini dipanggil oleh NPCBuyer ketika Spasi ditekan
    // Mengembalikan TRUE jika berhasil, FALSE jika stok habis
    public bool JualBarang()
    {
        if (balloonStock > 0)
        {
            // 1. Logika Kurangi Barang & Tambah Uang
            balloonStock--;
            playerMoney += balloonPrice;
            UpdateUI();

            Debug.Log("Jualan berhasil! Stok sisa: " + balloonStock);

            // 2. Tambah EXP (Jika script PlayerLevel terpasang)
            if (playerLevel != null)
            {
                playerLevel.AddExp(expPerSale);
            }

            // 3. Efek Visual (Floating Text)
            if (floatingExpPrefab != null && floatingSpawnPoint != null)
            {
                GameObject fx = Instantiate(floatingExpPrefab, floatingSpawnPoint.position, Quaternion.identity);
                // Pastikan prefab kamu punya script FloatingText
                // Kalau error disini, cek apakah script FloatingText ada
                var floatText = fx.GetComponent<FloatingText>();
                if (floatText != null)
                {
                    floatText.Setup($"+{expPerSale} EXP", Color.yellow);
                }
            }

            // 4. Efek Suara
            if (expSound != null) expSound.Play();
            if (sellSound != null) sellSound.Play();

            return true; // Lapor ke NPC: "Oke, transaksi sukses"
        }
        else
        {
            Debug.Log("Stok balon habis! Tidak bisa jual.");
            return false; // Lapor ke NPC: "Gagal, stok kosong"
        }
    }

    public void UpdateUI()
    {
        if (balloonText != null)
            balloonText.text = $"Balon: {balloonStock}";

        if (moneyText != null)
            moneyText.text = $"Uang: {playerMoney}";
    }
}
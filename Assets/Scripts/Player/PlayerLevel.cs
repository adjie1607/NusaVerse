using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Wajib ada untuk TextMeshPro

public class PlayerLevel : MonoBehaviour
{
    [Header("Player Stats")]
    public int level = 1;
    public int currentExp = 0;
    public int maxLevel = 3;
    public int currentMoney = 0;   // Tambahan uang
    public int balloonCount = 10;  // Tambahan stok balon

    [Header("Settings")]
    public int[] expNeeded = { 0, 100, 200 };

    [Header("UI References")]
    public Slider expSlider;
    public TMP_Text levelText;
    public TMP_Text moneyText;     // Slot baru untuk Money Text
    public TMP_Text balloonText;   // Slot baru untuk Balloon Text

    [Header("Effects")]
    public GameObject levelUpPanel;
    public Animator levelAnimator;
    public AudioSource levelUpSound;
    public AudioSource expGainSound;

    void Start()
    {
        UpdateUI();
    }

    // Fungsi untuk menambah Uang & Mengurangi Balon (Dipanggil Shop)
    public void TransaksiBerhasil(int uangMasuk, int balonKeluar)
    {
        currentMoney += uangMasuk;
        balloonCount -= balonKeluar;
        UpdateUI(); // Update tampilan UI
    }

    public void AddExp(int amount)
    {
        if (level >= maxLevel) return;

        currentExp += amount;
        CheckLevelUp();
        UpdateUI();

        if (expGainSound != null) expGainSound.Play();
    }

    void CheckLevelUp()
    {
        while (level < maxLevel && currentExp >= expNeeded[level])
        {
            currentExp -= expNeeded[level];
            level++;
            TriggerLevelUpAnimation();
        }
    }

    void TriggerLevelUpAnimation()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
            StartCoroutine(HideLevelUpPanel());
        }
        if (levelUpSound != null) levelUpSound.Play();
    }

    IEnumerator HideLevelUpPanel()
    {
        yield return new WaitForSeconds(1.5f);
        levelUpPanel.SetActive(false);
    }

    // --- BAGIAN UPDATE UI ---
    void UpdateUI()
    {
        // 1. Update Slider EXP
        if (expSlider != null)
        {
            // Mencegah error index out of range
            int safeLevel = Mathf.Clamp(level, 0, expNeeded.Length - 1);
            expSlider.maxValue = expNeeded[safeLevel];
            expSlider.value = currentExp;
        }

        // 2. Update Text Level
        if (levelText != null)
        {
            levelText.text = "LV " + level.ToString();
        }

        // 3. Update Text Uang (BARU)
        if (moneyText != null)
        {
            moneyText.text = "Rp " + currentMoney.ToString();
        }

        // 4. Update Text Balon (BARU)
        if (balloonText != null)
        {
            balloonText.text = "Balon: " + balloonCount.ToString();
        }
    }
}
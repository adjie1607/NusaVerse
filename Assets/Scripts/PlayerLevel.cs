using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevel : MonoBehaviour
{
    public int level = 1;
    public int currentExp = 0;

    public int maxLevel = 3;

    // EXP yang dibutuhkan per level
    public int[] expNeeded = { 0, 100, 200 };

    // UI ----------------------------
    public Slider expSlider;
    public TMP_Text levelText; // TextMeshPro
    public GameObject levelUpPanel;
    public Animator levelAnimator;
    public AudioSource levelUpSound;
    public AudioSource expGainSound;

    void Start()
    {
        UpdateUI();
    }

    public void ClampEXP()
    {
        if (currentExp < 0) currentExp = 0;
        if (currentExp > maxLevel) currentExp = maxLevel    ;
    }

    public int playerMoney = 200;

    public void GainEXP(int amount)
    {
        currentExp += amount;
        UpdateUI();
    }

    public void AddExp(int amount)
    {
        if (level >= maxLevel) return;

        currentExp += amount;
        CheckLevelUp();
        UpdateUI();
        if (expGainSound != null)
            expGainSound.Play();
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

        if (levelAnimator != null)
        {
            levelAnimator.SetTrigger("LevelUp");
        }

        if (levelUpSound != null)
            levelUpSound.Play();
    }

    IEnumerator HideLevelUpPanel()
    {
        yield return new WaitForSeconds(1.5f);
        levelUpPanel.SetActive(false);
    }

    void UpdateUI()
    {
        if (expSlider != null)
        {
            expSlider.maxValue = expNeeded[Mathf.Clamp(level, 0, expNeeded.Length - 1)];
            expSlider.value = currentExp;
        }

        if (levelText != null)
        {
            levelText.text = "LV " + level.ToString();
        }

    }
}

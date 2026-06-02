using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Data")]
    public int playerMoney = 100;
    public int playerEXP = 0;

    [Header("UI")]
    public TMP_Text moneyText;
    public TMP_Text expText;

    

    void Start()
    {
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UpdateUI();
    }

    public void SpendMoney(int amount)
    {
        playerMoney -= amount;
        UpdateUI();
    }

    public void GainEXP(int amount)
    {
        playerEXP += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = $"Money: {playerMoney}";

        if (expText != null)
            expText.text = $"EXP: {playerEXP}";
    }

    

}

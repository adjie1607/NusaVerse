using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerShop : MonoBehaviour
{
    [Header("Food Purchase")]
    public int expPerPurchase = 5;
    public AudioSource buyFoodSFX;

    public TMP_Text interactText;

    private FoodVendor currentVendor;


    [Header("Data")]
    public int balloonStock = 10;
    public int balloonPrice = 5;
    public int playerMoney = 0;

    [Header("EXP System")]
    public PlayerLevel playerLevel;   // <— reference ke PlayerLevel
    public int expPerSale = 10;       // exp yang didapat tiap jual balon

    [Header("UI - TextMeshPro")]
    public TMP_Text balloonText;
    public TMP_Text moneyText;

    [Header("Interaction")]
    public float interactRange = 3f;

    [Header("FX")]
    public GameObject floatingExpPrefab;
    public Transform floatingSpawnPoint;
    public AudioSource sellSound;
    public AudioSource expSound;

    private NPCBuyer currentBuyer;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentBuyer != null)
            {
                float dist = Vector3.Distance(transform.position, currentBuyer.transform.position);
                if (dist <= interactRange)
                {
                    SellBalloonTo(currentBuyer);
                }
                else
                {
                    Debug.Log("↔️ Terlalu jauh dari pembeli.");
                }
            }
            else
            {
                Debug.Log("Gak ada pembeli.");
            }
        }

    }

    public void UpdateUI()
        {
            if (balloonText != null)
                balloonText.text = $"Balon: {balloonStock}";
            else
                Debug.LogWarning("balloonText belum di-assign!");

            if (moneyText != null)
                moneyText.text = $"Uang: {playerMoney}";
            else
                Debug.LogWarning("moneyText belum di-assign!");

        }


    void SellBalloonTo(NPCBuyer buyer)
    {
        if (balloonStock > 0)
        {
            balloonStock--;
            playerMoney += balloonPrice;
            UpdateUI();

            Debug.Log($"{buyer.name} beli 1 balon!");

            // === Tambahin EXP di sini ===
            if (playerLevel != null)
                playerLevel.AddExp(expPerSale);

            // buyer lanjut pergi
            buyer.OnBoughtBalloon();

            if (currentBuyer == buyer)
                currentBuyer = null;
        }
        else
        {
            Debug.Log(" Stok balon habis!");
            buyer.OnBuyFailed_NoStock();

            if (currentBuyer == buyer)
                currentBuyer = null;
        }

        // spawn floating text
        if (floatingExpPrefab != null && floatingSpawnPoint != null)
        {
            GameObject fx = Instantiate(floatingExpPrefab, floatingSpawnPoint.position, Quaternion.identity);
            fx.GetComponent<FloatingText>().Setup($"+{expPerSale} EXP", Color.yellow);
        }

        // play sound
        if (expSound != null)
            expSound.Play();

        if (sellSound != null)
            sellSound.Play();

    }

    public bool TrySetCurrentBuyer(NPCBuyer buyer)
    {
        if (buyer == null) return false;
        if (currentBuyer != null) return false;

        currentBuyer = buyer;
        return true;
    }

    public void ClearCurrentBuyer(NPCBuyer buyer)
    {
        if (currentBuyer == buyer)
            currentBuyer = null;
    }


}

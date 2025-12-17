using UnityEngine;
using TMPro;

public class VendorTrigger : MonoBehaviour
{
    private FoodVendor vendor;
    private bool isPlayerInside = false;

    [Header("UI")]
    public TextMeshProUGUI notifText;
    public TextMeshProUGUI notEnoughMoneyText;
    public TextMeshProUGUI boughtText;

    private PlayerShop playerShop;
    private PlayerLevel playerLevel;

    private void Start()
    {
        vendor = GetComponent<FoodVendor>();

        // pastiin semua notif mati di awal
        notifText.gameObject.SetActive(false);
        notEnoughMoneyText.gameObject.SetActive(false);
        boughtText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerShop = other.GetComponent<PlayerShop>();
            playerLevel = other.GetComponent<PlayerLevel>();

            isPlayerInside = true;

            ClearAllNotif(); // anti UI tumpuk

            notifText.text = "Tekan F untuk membeli " + vendor.foodName;
            notifText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            ClearAllNotif(); // bersihin semua notif saat keluar
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.F))
        {
            TryBuyFood();
        }
    }

    void TryBuyFood()
    {
        ClearAllNotif(); // bersihin notif sebelum munculin yang baru

        if (playerShop.playerMoney < vendor.foodPrice)
        {
            notEnoughMoneyText.text = "Uang kamu tidak cukup!";
            notEnoughMoneyText.gameObject.SetActive(true);
            Invoke(nameof(HideNotEnough), 1.3f);
            return;
        }

        // sukses beli
        playerShop.playerMoney -= vendor.foodPrice;
        playerShop.UpdateUI();

        if (playerLevel != null)
            playerLevel.GainEXP(vendor.expReward);

        boughtText.text = "Kamu membeli " + vendor.foodName;
        boughtText.gameObject.SetActive(true);
        Invoke(nameof(HideBought), 1.3f);
    }

    void ClearAllNotif()
    {
        notifText.gameObject.SetActive(false);
        notEnoughMoneyText.gameObject.SetActive(false);
        boughtText.gameObject.SetActive(false);
    }

    void HideNotEnough() => notEnoughMoneyText.gameObject.SetActive(false);
    void HideBought() => boughtText.gameObject.SetActive(false);
}

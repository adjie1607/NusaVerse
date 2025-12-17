using UnityEngine;
using TMPro;

public class VendorTrigger : MonoBehaviour
{
    private FoodVendor vendor;
    private bool isPlayerInside = false;

    [Header("UI References")]
    public TextMeshProUGUI notifText;
    public TextMeshProUGUI notEnoughMoneyText;
    public TextMeshProUGUI boughtText;

    // Kita butuh referensi ke PlayerLevel untuk nambah EXP
    private PlayerLevel playerLevel;

    private void Start()
    {
        vendor = GetComponent<FoodVendor>();

        // Pastikan semua notif mati di awal
        if (notifText) notifText.gameObject.SetActive(false);
        if (notEnoughMoneyText) notEnoughMoneyText.gameObject.SetActive(false);
        if (boughtText) boughtText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ambil komponen PlayerLevel dari player
            playerLevel = other.GetComponent<PlayerLevel>();

            isPlayerInside = true;

            ClearAllNotif();

            if (vendor != null && notifText != null)
            {
                notifText.text = "Tekan [F] Beli " + vendor.foodName + " (" + vendor.foodPrice + ")";
                notifText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            ClearAllNotif();
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
        ClearAllNotif();

        // 1. CEK UANG KE INVENTORY MANAGER (BUKAN PLAYER SHOP LAGI)
        if (InventoryManager.Instance.totalMoney < vendor.foodPrice)
        {
            if (notEnoughMoneyText)
            {
                notEnoughMoneyText.text = "Uang tidak cukup!";
                notEnoughMoneyText.gameObject.SetActive(true);
                Invoke(nameof(HideNotEnough), 1.3f);
            }
            Debug.Log("Gagal beli: Uang kurang.");
            return;
        }

        // 2. PROSES TRANSAKSI
        // Kita pakai trik: AddMoney tapi minus (untuk mengurangi uang & otomatis update UI)
        InventoryManager.Instance.AddMoney(-vendor.foodPrice);

        // 3. TAMBAH EXP
        // (Pastikan nama method di script PlayerLevel kamu adalah AddExp atau GainEXP, sesuaikan disini)
        if (playerLevel != null)
        {
            playerLevel.AddExp(vendor.expReward);
        }

        // 4. UI SUKSES
        if (boughtText)
        {
            boughtText.text = "Kamu membeli " + vendor.foodName;
            boughtText.gameObject.SetActive(true);
            Invoke(nameof(HideBought), 1.3f);
        }

        Debug.Log("Berhasil beli makanan!");
    }

    void ClearAllNotif()
    {
        if (notifText) notifText.gameObject.SetActive(false);
        if (notEnoughMoneyText) notEnoughMoneyText.gameObject.SetActive(false);
        if (boughtText) boughtText.gameObject.SetActive(false);
    }

    void HideNotEnough()
    {
        if (notEnoughMoneyText) notEnoughMoneyText.gameObject.SetActive(false);
    }

    void HideBought()
    {
        if (boughtText) boughtText.gameObject.SetActive(false);
    }
}
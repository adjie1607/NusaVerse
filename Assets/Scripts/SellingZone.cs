using UnityEngine;
using TMPro;

public class SellingZone : MonoBehaviour
{
    [Header("Setup")]
    public QueueManager queueManager; // Drag QueueManager di Inspector
    public TextMeshProUGUI textNotif; // Drag Text UI "Tekan F"

    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (textNotif)
            {
                textNotif.text = "Tekan [F] Jual Balon";
                textNotif.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (textNotif) textNotif.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // LOGIKA UTAMA: Hanya bisa jika di dalam area DAN tekan F
        if (isPlayerInside && Input.GetKeyDown(KeyCode.F))
        {
            ProsesJual();
        }
    }

    void ProsesJual()
    {
        // 1. Ambil orang paling depan
        NPCBuyer customer = queueManager.GetFirstCustomer();

        // 2. Cek apakah ada orang DAN statusnya boleh beli (sudah sampai depan)
        if (customer != null && customer.canBuy)
        {
            customer.BuyBalloon();
            // Opsional: Play Sound Effect Kaching disini
        }
        else
        {
            Debug.Log("Belum ada pembeli di depan meja!");
        }
    }
}
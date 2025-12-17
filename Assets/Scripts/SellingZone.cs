using UnityEngine;
using TMPro;

public class SellingZone : MonoBehaviour
{
    [Header("Requirements")]
    public QueueManager queueManager; // Drag object Manager Antrian
    public TextMeshProUGUI interactionText; // Drag Text "Tekan F Jual"

    private bool isPlayerInside = false;

    void Start()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            // Tampilkan Text UI
            if (interactionText != null)
            {
                interactionText.text = "Tekan [F] Jual Balon";
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

            // Sembunyikan Text UI
            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Logika Interact: Player di dalam area DAN tekan F
        if (isPlayerInside && Input.GetKeyDown(KeyCode.F))
        {
            TrySellBalloon();
        }
    }

    void TrySellBalloon()
    {
        // 1. Ambil NPC paling depan dari QueueManager
        NPCBuyer customer = queueManager.GetFirstCustomer();

        // 2. Validasi: Ada orang gak? Dia udah siap beli (sudah sampai depan) gak?
        if (customer != null && customer.canBuy)
        {
            // 3. Eksekusi Jual
            customer.BuyBalloon();

            // Opsional: Animasi atau feedback ke player kalau sukses tekan
        }
        else
        {
            Debug.Log("Belum ada pembeli di depan antrian / pembeli belum sampai meja.");
        }
    }
}
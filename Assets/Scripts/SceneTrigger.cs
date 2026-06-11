using UnityEngine;
using TMPro;

public class SceneTrigger : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI pressEText;
    public TextMeshProUGUI levelWarningText;

    [Header("Single Scene Setup")]
    public string houseID;               // ISI DI INSPECTOR (Contoh: Rumah1)
    public Transform interiorSpawnPoint; // Titik spawn di DALAM Rumah Adat
    public int requiredLevel = 2;

    private bool isPlayerInside = false;
    private PlayerLevel playerLevel;
    private GameObject playerObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerObj = other.gameObject;
            playerLevel = other.GetComponent<PlayerLevel>();
            if (playerLevel == null)
                playerLevel = other.GetComponentInParent<PlayerLevel>();

            isPlayerInside = true;
            pressEText.gameObject.SetActive(true);

            // Cek apakah rumah sudah selesai diselesaikan sebelumnya
            if (GameSessionManager.Instance != null && GameSessionManager.Instance.IsHouseComplete(houseID))
            {
                pressEText.text = "Rumah ini sudah selesai dipelajari!";
            }
            else
            {
                pressEText.text = "Tekan E untuk masuk";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            pressEText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isPlayerInside) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryEnterHouse();
        }
    }

    void TryEnterHouse()
    {
        if (playerLevel == null || playerObj == null) return;

        // PROTEKSI 1: Jika sudah selesai, hadang player agar tidak masuk lagi
        if (GameSessionManager.Instance != null && GameSessionManager.Instance.IsHouseComplete(houseID))
        {
            levelWarningText.text = "Kamu sudah menyelesaikan kuis rumah ini!";
            levelWarningText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideWarning));
            Invoke(nameof(HideWarning), 1.5f);
            return;
        }

        // PROTEKSI 2: Cek kecukupan Level
        if (playerLevel.level < requiredLevel)
        {
            levelWarningText.text = "Level kamu belum cukup!";
            levelWarningText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideWarning));
            Invoke(nameof(HideWarning), 1.5f);
            return;
        }

        pressEText.gameObject.SetActive(false);

        // Jalankan Teleport ke interior rumah adat
        Rigidbody rb = playerObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.position = interiorSpawnPoint.position;
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            playerObj.transform.position = interiorSpawnPoint.position;
        }
    }

    void HideWarning()
    {
        levelWarningText.gameObject.SetActive(false);
    }
}
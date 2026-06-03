using UnityEngine;
using TMPro;

public class SceneTrigger : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI pressEText;
    public TextMeshProUGUI levelWarningText;

    [Header("Single Scene Setup")]
    public Transform interiorSpawnPoint; // Seret objek titik spawn di DALAM Rumah Adat disini
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
            pressEText.text = "Tekan E untuk masuk";
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

        if (playerLevel.level < requiredLevel)
        {
            levelWarningText.text = "Level kamu belum cukup!";
            levelWarningText.gameObject.SetActive(true);

            CancelInvoke(nameof(HideWarning));
            Invoke(nameof(HideWarning), 1.5f);
            return;
        }

        pressEText.gameObject.SetActive(false);

        // Jalankan Teleportasi ke interior rumah
        playerObj.transform.position = interiorSpawnPoint.position;

        // Reset kecepatan Rigidbody agar player tidak meluncur setelah teleport
        Rigidbody rb = playerObj.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = Vector3.zero;
    }

    void HideWarning()
    {
        levelWarningText.gameObject.SetActive(false);
    }
}
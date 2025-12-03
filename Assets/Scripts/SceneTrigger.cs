using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTrigger : MonoBehaviour
{
    public string targetScene;

    public TextMeshProUGUI pressEText;
    public TextMeshProUGUI levelWarningText;

    public int requiredLevel = 2;

    private bool isPlayerInside = false;
    private PlayerLevel playerLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aman: cek di parent juga
            playerLevel = other.GetComponent<PlayerLevel>();
            if (playerLevel == null)
                playerLevel = other.GetComponentInParent<PlayerLevel>();

            Debug.Log("PlayerLevel FOUND? → " + (playerLevel != null));

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
        if (playerLevel == null)
        {
            Debug.LogError("PlayerLevel ga ditemukan di Player!");
            return;
        }

        Debug.Log("Cek level player: " + playerLevel.level);

        if (playerLevel.level < requiredLevel)
        {
            levelWarningText.text = "Level kamu belum cukup!";
            levelWarningText.gameObject.SetActive(true);

            CancelInvoke(nameof(HideWarning));
            Invoke(nameof(HideWarning), 1.5f);

            return;
        }

        // Kalau lolos level → masuk scene
        SceneManager.LoadScene(targetScene);
    }

    void HideWarning()
    {
        levelWarningText.gameObject.SetActive(false);
    }
}
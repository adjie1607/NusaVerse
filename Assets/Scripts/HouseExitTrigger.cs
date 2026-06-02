using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseExitTrigger : MonoBehaviour
{
    public string sceneTujuan = "GameScene";

    [Header("Identitas Rumah")]
    public string houseID; // DISINI KITA ISI MANUAL DI INSPECTOR (Misal: "Rumah1")

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeluarRumah();
        }
    }

    public void KeluarRumah()
    {
        if (GameSessionManager.Instance != null)
        {
            GameSessionManager.Instance.isReturningFromHouse = true;

            // Kirim ID rumah ini ke Manager
            GameSessionManager.Instance.lastVisitedID = houseID;
        }

        SceneManager.LoadScene(sceneTujuan);
    }
}
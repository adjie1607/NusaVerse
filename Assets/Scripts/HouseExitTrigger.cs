using UnityEngine;

public class HouseExitTrigger : MonoBehaviour
{
    [Header("Identitas & Spawn")]
    public string houseID; // Contoh: "Rumah1"
    public Transform outsideSpawnPoint; // Seret titik objek spawn di LUAR rumah adat

    [Header("Referensi Kuis")]
    public GameObject panelQuiz;
    public QuizManager quizManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KeluarRumah(other.gameObject);
        }
    }

    public void KeluarRumah(GameObject player)
    {
        // 1. Teleport player kembali ke luar di map utama
        player.transform.position = outsideSpawnPoint.position;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = Vector3.zero;

        // 2. Set data riwayat kunjungan rumah
        if (GameSessionManager.Instance != null)
        {
            GameSessionManager.Instance.isReturningFromHouse = true;
            GameSessionManager.Instance.lastVisitedID = houseID;
        }

        // 3. Langsung munculkan kuis tanpa reload scene
        if (quizManager != null && panelQuiz != null)
        {
            quizManager.SiapkanSoal(houseID);
            panelQuiz.SetActive(true);

            // Buka kursor dan pause game sementara untuk menjawab kuis
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }
}
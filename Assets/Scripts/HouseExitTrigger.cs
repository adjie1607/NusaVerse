using UnityEngine;

public class HouseExitTrigger : MonoBehaviour
{
    [Header("Identitas & Spawn")]
    public string houseID;              // ISI DI INSPECTOR (Contoh: Rumah1)
    public Transform outsideSpawnPoint; // Titik spawn di LUAR rumah adat

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
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.position = outsideSpawnPoint.position;
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            player.transform.position = outsideSpawnPoint.position;
        }

        // 2. Ambil status apakah kuis rumah ini sudah berstatus tamat
        bool rumahSudahSelesai = false;
        if (GameSessionManager.Instance != null)
        {
            rumahSudahSelesai = GameSessionManager.Instance.IsHouseComplete(houseID);
        }

        // 3. Jika BELUM tamat, barulah munculkan kuisnya
        if (!rumahSudahSelesai)
        {
            if (GameSessionManager.Instance != null)
            {
                GameSessionManager.Instance.isReturningFromHouse = true;
                GameSessionManager.Instance.lastVisitedID = houseID;
            }

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
        else
        {
            // Jika sudah tamat, matikan kursor & normalkan waktu berjalan kembali
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            Debug.Log($"Rumah {houseID} terdeteksi sudah tamat. Kuis diblokir.");
        }
    }
}
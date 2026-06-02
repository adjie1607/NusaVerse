using UnityEngine;
using TMPro;

public class MainSceneController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panelQuiz;
    public QuizManager quizManager; // Referensi ke script QuizManager

    [Header("Game Objects")]
    public GameObject gembokRumah2; // Object penghalang masuk rumah 2
    public GameObject gembokRumah3; // Object penghalang masuk rumah 3

    void Start()
    {
        // 1. Cek Trigger Kuis
        CheckReturnStatus();

        // 2. Load Progress Gembok (Buka gembok kalau rumah sebelumnya sudah selesai)
        UpdateWorldProgress();
    }

    void CheckReturnStatus()
    {
        // 1. Cek apakah Manager ada?
        if (GameSessionManager.Instance == null) return;

        // 2. Cek apakah pemain baru pulang?
        if (GameSessionManager.Instance.isReturningFromHouse)
        {
            string idRumah = GameSessionManager.Instance.lastVisitedID;
            Debug.Log("DETEKTIF: Player pulang dari ID = " + idRumah);

            // 3. Cek apakah QuizManager sudah dipasang?
            if (quizManager != null)
            {
                Debug.Log("DETEKTIF: Menyuruh QuizManager menyiapkan soal...");

                // --- PENTING: SIAPKAN SOAL DULU ---
                quizManager.SiapkanSoal(idRumah);

                // --- BARU NYALAKAN PANEL ---
                if (panelQuiz != null)
                {
                    panelQuiz.SetActive(true);
                    Debug.Log("DETEKTIF: Panel dinyalakan.");
                }
            }
            else
            {
                Debug.LogError("DETEKTIF ERROR: Slot 'Quiz Manager' di Inspector masih KOSONG (None)!");
            }

            // Reset status
            GameSessionManager.Instance.isReturningFromHouse = false;
        }
    }

    void UpdateWorldProgress()
    {
        if (GameSessionManager.Instance == null) return;

        // 1. Cek Rumah 1 -> Buka Rumah 2
        bool rumah1Selesai = GameSessionManager.Instance.IsHouseComplete("Rumah1");
        if (rumah1Selesai)
        {
            if (gembokRumah2 != null) gembokRumah2.SetActive(false);
        }

        // 2. Cek Rumah 2 -> TAMAT GAME
        bool rumah2Selesai = GameSessionManager.Instance.IsHouseComplete("Rumah2");
        if (rumah2Selesai)
        {
            // Logika Tamat
            Debug.Log("SELAMAT! SEMUA RUMAH SELESAI!");
            // Kamu bisa memunculkan panel tamat di sini
            // contoh: panelTamat.SetActive(true);
        }
    }
}
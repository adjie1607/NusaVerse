using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        [TextArea] public string questionText;
        public string answerA;
        public string answerB;
        public string answerC;
        public string answerD;
        public int correctAnswerIndex; // 0=A, 1=B, 2=C, 3=D
    }

    [Header("UI References")]
    public GameObject quizPanel;
    public TextMeshProUGUI questionUI;
    public TextMeshProUGUI[] buttonTexts;

    [Header("Bank Soal")]
    public List<Question> soalRumah1;
    public List<Question> soalRumah2;

    private List<Question> currentQuestionList = new List<Question>(); // Inisialisasi biar gak null
    private int currentQuestionIndex = 0;

    // Dipanggil otomatis saat Panel dinyalakan (SetActive true)
    private void OnEnable()
    {
        DisplayQuestion();
    }

    public void SiapkanSoal(string houseID)
    {
        currentQuestionList.Clear(); 

        Debug.Log("Mencoba memuat soal untuk ID: " + houseID);

        // Cek ID dan isi list
        if (houseID == "Rumah1")
        {
            if (soalRumah1.Count > 0) currentQuestionList.AddRange(soalRumah1);
            else Debug.LogError("Soal Rumah 1 di Inspector Kosong!");
        }
        else if (houseID == "Rumah2")
        {
            if (soalRumah2.Count > 0) currentQuestionList.AddRange(soalRumah2);
            else Debug.LogError("Soal Rumah 2 di Inspector Kosong!");
        }
        else
        {
            Debug.LogWarning("ID Rumah tidak dikenal: " + houseID + ". Pastikan penulisan sama persis.");
        }

        currentQuestionIndex = 0;
        // Tidak perlu panggil DisplayQuestion disini, karena OnEnable akan menanganinya
    }

    void DisplayQuestion()
    {
        // PENTING: Cek apakah ada soal?
        if (currentQuestionList == null || currentQuestionList.Count == 0)
        {
            questionUI.text = "Belum ada soal yang dimuat.";
            return;
        }

        if (currentQuestionIndex < currentQuestionList.Count)
        {
            Question q = currentQuestionList[currentQuestionIndex];

            questionUI.text = q.questionText;
            buttonTexts[0].text = q.answerA;
            buttonTexts[1].text = q.answerB;
            buttonTexts[2].text = q.answerC;
            buttonTexts[3].text = q.answerD;
        }
        else
        {
            EndQuiz();
        }
    }

    public void OnAnswerSelected(int index)
    {
        // Cek dulu biar gak error kalau list kosong
        if (currentQuestionList.Count == 0) return;

        if (index == currentQuestionList[currentQuestionIndex].correctAnswerIndex)
        {
            Debug.Log("Benar!");
            if (GameSessionManager.Instance != null) GameSessionManager.Instance.AddEXP(10);
        }

        currentQuestionIndex++;
        DisplayQuestion();
    }

    void EndQuiz()
    {
        if (GameSessionManager.Instance != null)
        {
            GameSessionManager.Instance.MarkHouseAsComplete(GameSessionManager.Instance.lastVisitedID);
        }
        quizPanel.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
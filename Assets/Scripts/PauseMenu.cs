using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pausePanel;

    [Header("Tombol Audio (Drag Game Object Tombolnya)")]
    public GameObject muteButtonObj;   // Tombol untuk Mematikan Suara
    public GameObject unmuteButtonObj; // Tombol untuk Menghidupkan Suara

    [Header("Settings")]
    public bool isPaused = false;
    private bool isMuted = false;

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        // Cek status awal audio, update tampilan tombol
        UpdateAudioButtonUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        // Cursor.lockState = CursorLockMode.Locked; // Hidupkan jika FPS
        // Cursor.visible = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Keluar Game");
    }

    // --- LOGIKA MUTE / UNMUTE (Ganti Tombol) ---

    public void ToggleMute()
    {
        isMuted = !isMuted; // Tukar status (True <-> False)

        // Atur suara global Unity
        AudioListener.pause = isMuted;

        // Update tampilan tombol
        UpdateAudioButtonUI();
    }

    void UpdateAudioButtonUI()
    {
        if (isMuted)
        {
            // Kalau lagi Mute (Hening) -> Tampilkan tombol Unmute
            if (muteButtonObj) muteButtonObj.SetActive(false);
            if (unmuteButtonObj) unmuteButtonObj.SetActive(true);
        }
        else
        {
            // Kalau lagi Hidup (Berisik) -> Tampilkan tombol Mute
            if (muteButtonObj) muteButtonObj.SetActive(true);
            if (unmuteButtonObj) unmuteButtonObj.SetActive(false);
        }
    }
}
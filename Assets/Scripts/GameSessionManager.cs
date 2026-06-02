using UnityEngine;
using System.Collections.Generic; // Wajib ada untuk List

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance;

    [Header("Stats Player")]
    public int playerMoney = 10000;
    public int currentEXP = 0; // Exp Player

    [Header("Navigation Data")]
    public bool isReturningFromHouse = false;
    public string lastVisitedID = ""; // ID rumah terakhir (misal: "Rumah1")

    [Header("Progress Data")]
    public List<string> completedHouses = new List<string>(); // Daftar rumah yang sudah tamat

    private void Awake()
    {
        // Singleton Logic
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- FUNGSI UNTUK EXP (Ini yang tadi hilang) ---
    public void AddEXP(int amount)
    {
        currentEXP += amount;
        Debug.Log("EXP Bertambah! Total EXP: " + currentEXP);
    }
    // ----------------------------------------------

    // Fungsi untuk menandai rumah selesai
    public void MarkHouseAsComplete(string houseID)
    {
        if (!completedHouses.Contains(houseID))
        {
            completedHouses.Add(houseID);
            Debug.Log("Rumah " + houseID + " Selesai!");
        }
    }

    // Fungsi untuk mengecek status rumah
    public bool IsHouseComplete(string houseID)
    {
        return completedHouses.Contains(houseID);
    }
}
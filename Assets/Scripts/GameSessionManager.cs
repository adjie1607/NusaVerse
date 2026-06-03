using UnityEngine;
using System.Collections.Generic; // Wajib ada untuk List

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance;

    [Header("Stats Player Global")]
    public int globalLevel = 1;
    public int globalEXP = 0;
    public int globalMoney = 0;
    public int globalBalloons = 10;

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

    // Fungsi untuk menambah EXP dari kuis
    public void AddEXP(int amount)
    {
        globalEXP += amount; // Menggunakan variabel globalEXP yang baru
        Debug.Log("EXP Kuis masuk! Total EXP Global: " + globalEXP);
    }
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
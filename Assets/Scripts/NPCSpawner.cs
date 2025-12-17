using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public float spawnInterval = 3f;

    [Header("Batasan Populasi")]
    public int maxNPC = 20; // Maksimal NPC di scene

    [Header("Referensi Sistem")]
    public QueueManager queueManager; // Drag QueueManager kesini (Wajib!)
    public PlayerShop playerShop;     // Drag PlayerShop kesini (Wajib!)

    [Header("Lokasi Jalan-Jalan (Jika Antrian Penuh)")]
    public Transform[] wanderSpots;   // Drag titik kumpul/gerobak makanan

    [Header("Titik Keluar (Tempat NPC Pulang)")]
    public Transform exitPoint;       // Drag titik di ujung map buat mereka pulang

    private float timer;

    void Update()
    {
        // 1. Cek Jumlah NPC saat ini
        // (Cara ini agak berat kalau ratusan, tapi untuk 20 aman)
        int currentCount = FindObjectsOfType<NPCBuyer>().Length;

        // Kalau sudah 20 atau lebih, JANGAN SPAWN
        if (currentCount >= maxNPC) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnNPC();
            timer = 0;
        }
    }

    void SpawnNPC()
    {
        GameObject newNPC = Instantiate(npcPrefab, transform.position, Quaternion.identity);
        NPCBuyer npcScript = newNPC.GetComponent<NPCBuyer>();

        if (npcScript != null)
        {
            // === LOGIKA PRIORITAS ===

            // Cek 1: Apakah Antrian Balon Masih Muat?
            if (!queueManager.IsQueueFull())
            {
                // Kalau muat, WAJIB MASUK ANTRIAN (Prioritas Utama)
                npcScript.Initialize(queueManager, playerShop);

                // Set exit point biar nanti pas abis beli tau jalan pulang
                npcScript.exitPoint = exitPoint;
            }
            else
            {
                // Cek 2: Kalau Antrian Penuh, baru jadi Wanderer (Jalan-jalan)
                if (wanderSpots.Length > 0)
                {
                    int randomIndex = Random.Range(0, wanderSpots.Length);
                    npcScript.wanderCenter = wanderSpots[randomIndex];
                    npcScript.exitPoint = exitPoint; // Kasih tau jalan pulang

                    // Aktifkan mode wander manual
                    npcScript.StartWandering();
                }
            }
        }
    }
}
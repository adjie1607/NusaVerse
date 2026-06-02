using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public QueueManager queueManager;
    public PlayerShop playerShop; // <-- Drag Script PlayerShop kesini di Inspector
    public Transform spawnLocation;

    public float interval = 5f;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            SpawnNPC();
            timer = 0;
        }
    }

    void SpawnNPC()
    {
        if (queueManager == null || npcPrefab == null || playerShop == null) return;

        GameObject newNPC = Instantiate(npcPrefab, spawnLocation.position, Quaternion.identity);
        NPCBuyer script = newNPC.GetComponent<NPCBuyer>();

        if (script != null)
        {
            // Kirim Manager DAN Shop ke NPC
            script.Initialize(queueManager, playerShop);
        }
    }
}
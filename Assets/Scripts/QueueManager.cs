using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public Transform[] queuePoints;
    public GameObject npcPrefab;
    public Transform spawnPoint;
    public Transform player;
    public Transform exitPoint; // <-- assign satu titik keluar (atau array jika mau variasi)
    public float spawnInterval = 5f;

    private List<NPCBuyer> npcQueue = new List<NPCBuyer>();
    private bool isProcessing = false;

    void Start()
    {
        InvokeRepeating(nameof(SpawnNPC), 1.5f, spawnInterval);
    }

    void SpawnNPC()
    {
        if (npcQueue.Count >= queuePoints.Length) return;

        GameObject npcObj = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
        NPCBuyer npc = npcObj.GetComponent<NPCBuyer>();
        if (npc == null) return;

        npc.targetPosition = queuePoints[npcQueue.Count];
        npc.player = player;
        npc.exitPoint = exitPoint; // assign exit point agar gak ilang ke samping

        npcQueue.Add(npc);
        if (!isProcessing)
            StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        isProcessing = true;
        PlayerShop shop = player.GetComponent<PlayerShop>();

        while (npcQueue.Count > 0)
        {
            // buang null/destroyed entries
            if (npcQueue[0] == null)
            {
                npcQueue.RemoveAt(0);
                ShiftQueuePositions();
                yield return null;
                continue;
            }

            NPCBuyer current = npcQueue[0];

            // jika current lagi leaving karena timeout atau sdh beli, langsung remove
            if (current.IsLeaving())
            {
                npcQueue.RemoveAt(0);
                ShiftQueuePositions();
                yield return null;
                continue;
            }

            // tunggu sampai npc nyampe pos antrian
            yield return new WaitUntil(() => Vector3.Distance(current.transform.position, current.targetPosition.position) < 0.12f);

            // mulai proses wait-to-buy
            yield return StartCoroutine(current.WaitToBuy(shop));

            // setelah proses selesai (beli atau timeout), remove front kalau masih sama
            if (npcQueue.Count > 0 && npcQueue[0] == current)
            {
                npcQueue.RemoveAt(0);
            }
            else
            {
                int idx = npcQueue.IndexOf(current);
                if (idx >= 0) npcQueue.RemoveAt(idx);
            }

            // shift posisi untuk seluruh antrian
            ShiftQueuePositions();

            yield return new WaitForSeconds(0.12f);
        }

        isProcessing = false;
    }

    void ShiftQueuePositions()
    {
        for (int i = 0; i < npcQueue.Count; i++)
        {
            if (npcQueue[i] != null)
                npcQueue[i].targetPosition = queuePoints[i];
        }
    }
}

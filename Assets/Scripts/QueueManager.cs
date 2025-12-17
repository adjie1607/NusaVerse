using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [Header("Titik Antrian (Urut dari depan ke belakang)")]
    public Transform[] queuePoints;

    [Header("Titik Keluar")]
    public Transform exitPoint;

    // List orang yang sedang ngantri
    private List<NPCBuyer> currentQueue = new List<NPCBuyer>();

    // 1. Dipanggil saat NPC baru lahir
    public void AddToQueue(NPCBuyer npc)
    {
        if (currentQueue.Count < queuePoints.Length)
        {
            currentQueue.Add(npc);
            UpdateAllPositions(); // Atur ulang barisan
        }
        else
        {
            Debug.Log("Antrian Penuh! NPC langsung pulang.");
            npc.LeaveShop(exitPoint.position);
        }
    }

    // 2. Dipanggil saat NPC selesai beli / pergi
    public void RemoveFromQueue(NPCBuyer npc)
    {
        if (currentQueue.Contains(npc))
        {
            currentQueue.Remove(npc);
            UpdateAllPositions(); // Yang belakang maju ke depan
        }
    }

    // 3. Fungsi untuk menyuruh semua orang ke posisi masing-masing
    void UpdateAllPositions()
    {
        for (int i = 0; i < currentQueue.Count; i++)
        {
            // Suruh NPC jalan ke titik antrian sesuai urutannya (Index 0 ke Point 0, dst)
            currentQueue[i].GoToQueuePosition(queuePoints[i].position);

            // Cek status: Kalau dia urutan 0 (paling depan), dia boleh beli
            if (i == 0)
            {
                currentQueue[i].EnableBuyingInteraction(true);
            }
            else
            {
                currentQueue[i].EnableBuyingInteraction(false);
            }
        }
    }
}
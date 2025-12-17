using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [Header("Titik Antrian")]
    public Transform[] queuePoints;
    public Transform exitPoint;

    private List<NPCBuyer> currentQueue = new List<NPCBuyer>();

    // === TAMBAHAN BARU: FUNGSI CEK PENUH ===
    public bool IsQueueFull()
    {
        // Kalau jumlah pengantri >= jumlah titik antrian, berarti penuh
        return currentQueue.Count >= queuePoints.Length;
    }
    // ========================================

    public void AddToQueue(NPCBuyer npc)
    {
        if (currentQueue.Count < queuePoints.Length)
        {
            currentQueue.Add(npc);
            UpdateAllPositions();
        }
        else
        {
            // Kalau penuh, suruh pulang
            if (exitPoint) npc.LeaveShop(exitPoint.position);
            else Destroy(npc.gameObject);
        }
    }

    public void RemoveFromQueue(NPCBuyer npc)
    {
        if (currentQueue.Contains(npc))
        {
            currentQueue.Remove(npc);
            UpdateAllPositions();
        }
    }

    void UpdateAllPositions()
    {
        for (int i = 0; i < currentQueue.Count; i++)
        {
            currentQueue[i].GoToQueuePosition(queuePoints[i].position);

            if (i == 0) currentQueue[i].EnableBuyingInteraction(true);
            else currentQueue[i].EnableBuyingInteraction(false);
        }
    }

    public NPCBuyer GetFirstCustomer()
    {
        if (currentQueue.Count > 0) return currentQueue[0];
        return null;
    }
}
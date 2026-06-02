using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    [Header("Titik Antrian")]
    public Transform[] queuePoints;
    public Transform exitPoint;

    private List<NPCBuyer> currentQueue = new List<NPCBuyer>();

    // === FIX ERROR CS1061 (Spawner butuh ini) ===
    public bool IsQueueFull()
    {
        return currentQueue.Count >= queuePoints.Length;
    }

    // === FIX LOGIC JUALAN (SellingZone butuh ini) ===
    public NPCBuyer GetFirstCustomer()
    {
        if (currentQueue.Count > 0) return currentQueue[0];
        return null;
    }

    public void AddToQueue(NPCBuyer npc)
    {
        if (currentQueue.Count < queuePoints.Length)
        {
            currentQueue.Add(npc);
            UpdatePositions();
        }
        else
        {
            if (exitPoint) npc.LeaveShop(exitPoint.position);
            else Destroy(npc.gameObject);
        }
    }

    public void RemoveFromQueue(NPCBuyer npc)
    {
        if (currentQueue.Contains(npc))
        {
            currentQueue.Remove(npc);
            UpdatePositions();
        }
    }

    void UpdatePositions()
    {
        for (int i = 0; i < currentQueue.Count; i++)
        {
            if (currentQueue[i] == null) continue;

            currentQueue[i].GoToQueuePosition(queuePoints[i].position);

            // Yang paling depan (index 0) statusnya boleh beli
            if (i == 0) currentQueue[i].EnableBuyingInteraction(true);
            else currentQueue[i].EnableBuyingInteraction(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBuyer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public Transform targetPosition; // posisi antrian
    public Transform player;

    [Header("Exit")]
    public Transform exitPoint; // <-- assign dari QueueManager
    public float leaveThreshold = 0.1f;

    // states
    private bool waitingToBuy = false;
    private bool hasBought = false;
    private bool isLeaving = false;

    // safety
    public float waitTimeout = 12f; // timeout nunggu interaction

    void Update()
    {
        // kalau lagi leaving, gerak ke exitPoint
        if (isLeaving && exitPoint != null)
        {
            MoveTo(exitPoint.position);
            return;
        }

        // normal: gerak ke posisi antrian
        if (!waitingToBuy && targetPosition != null && !isLeaving)
        {
            MoveTo(targetPosition.position);

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.05f)
            {
                transform.position = targetPosition.position;
            }
        }
    }

    void MoveTo(Vector3 pos)
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);

        // optional: rotate smoothly toward movement direction
        Vector3 dir = pos - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }
    }

    // dipanggil QueueManager ketika posisi sudah di-set
    public IEnumerator WaitToBuy(PlayerShop shop)
    {
        if (hasBought || isLeaving) yield break;

        // tunggu sampai di posisi antrian
        yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPosition.position) < 0.12f);

        float timer = 0f;
        bool becameCurrent = false;

        while (timer < waitTimeout && !becameCurrent && !hasBought && !isLeaving)
        {
            if (shop != null && shop.TrySetCurrentBuyer(this))
            {
                waitingToBuy = true;
                becameCurrent = true;
                Debug.Log($"{name} siap beli! Tekan [SPACE] buat jual balon.");

                float localTimer = 0f;
                while (waitingToBuy && localTimer < waitTimeout && !isLeaving)
                {
                    localTimer += Time.deltaTime;
                    yield return null;
                }

                // kalau masih waiting (timeout), clear di shop
                if (waitingToBuy)
                {
                    waitingToBuy = false;
                    shop.ClearCurrentBuyer(this);
                }

                break;
            }
            else
            {
                yield return new WaitForSeconds(0.15f);
                timer += 0.15f;
            }
        }

        // kalau gagal jadi current buyer (timeout atau ditolak), langsung leave pelan
        if (!becameCurrent && !hasBought && !isLeaving)
        {
            Debug.Log($"{name} skip/timeout, pergi.");
            StartLeaving();
        }
    }

    // dipanggil ketika transaksi berhasil
    public void OnBoughtBalloon()
    {
        if (hasBought || isLeaving) return;

        waitingToBuy = false;
        hasBought = true;

        // pastiin dia clear di player shop if needed (player akan clear currentBuyer)
        StartLeaving();
    }

    // dipanggil kalau stok habis
    public void OnBuyFailed_NoStock()
    {
        if (isLeaving) return;

        waitingToBuy = false;
        StartLeaving();
    }

    void StartLeaving()
    {
        // nonaktifkan ability buat jadi current buyer lagi
        isLeaving = true;
        waitingToBuy = false;

        // optional: disable collider / interactions so other systems won't set this again
        Collider c = GetComponent<Collider>();
        if (c != null) c.enabled = false;
    }

    // optional public helper to check if NPC is leaving (QueueManager might want to skip)
    public bool IsLeaving()
    {
        return isLeaving;
    }

    // OnDrawGizmos buat debugging visual di editor
    void OnDrawGizmosSelected()
    {
        if (targetPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetPosition.position);
        }

        if (exitPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, exitPoint.position);
            Gizmos.DrawSphere(exitPoint.position, 0.12f);
        }
    }
}
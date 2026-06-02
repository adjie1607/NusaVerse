using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCWalker : MonoBehaviour
{
    private NavMeshAgent agent;
    public Animator anim; // Opsional jika punya animasi duduk

    [Header("Tujuan")]
    public Transform[] foodSpots;   // Daftar lokasi tukang makanan
    public Transform[] sitSpots;    // Daftar kursi/bangku
    public Transform exitPoint;     // Titik pulang

    [Header("Setting Waktu")]
    public float waitAtFoodDuration = 3f;
    public float sitDuration = 10f;

    private int currentTargetIndex = 0;
    private bool isSitting = false;
    private bool taskCompleted = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Mulai logika keliling
        StartCoroutine(RoutineActivity());
    }

    IEnumerator RoutineActivity()
    {
        // 1. PERGI KE FOOD VENDOR (Random)
        if (foodSpots.Length > 0)
        {
            Transform targetFood = foodSpots[Random.Range(0, foodSpots.Length)];
            agent.SetDestination(targetFood.position);

            // Tunggu sampai sampai
            yield return new WaitUntil(() => HasArrived());

            // Pura-pura beli/makan
            yield return new WaitForSeconds(waitAtFoodDuration);
        }

        // 2. CARI TEMPAT DUDUK
        if (sitSpots.Length > 0)
        {
            Transform targetChair = sitSpots[Random.Range(0, sitSpots.Length)];
            agent.SetDestination(targetChair.position);

            // Tunggu sampai di depan kursi
            yield return new WaitUntil(() => HasArrived());

            // 3. LOGIKA DUDUK
            StartSitting(targetChair);
            yield return new WaitForSeconds(sitDuration);
            StopSitting();
        }

        // 4. PULANG
        if (exitPoint != null)
        {
            agent.SetDestination(exitPoint.position);
            yield return new WaitForSeconds(1f); // Beri waktu agent kalkulasi path

            // Tunggu sampai hilang atau destroy timer
            Destroy(gameObject, 15f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    bool HasArrived()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }

    void StartSitting(Transform chair)
    {
        isSitting = true;
        agent.isStopped = true;
        agent.enabled = false; // Matikan agent biar bisa diposisikan manual

        // Posisikan pas di kursi
        transform.position = chair.position;
        transform.rotation = chair.rotation;

        // Jika ada animasi
        if (anim != null) anim.SetBool("isSitting", true);
    }

    void StopSitting()
    {
        isSitting = false;
        if (anim != null) anim.SetBool("isSitting", false);

        // Hidupkan agent lagi buat jalan pulang
        agent.enabled = true;
        agent.isStopped = false;
    }
}
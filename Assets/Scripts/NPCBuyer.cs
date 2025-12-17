using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCBuyer : MonoBehaviour
{
    private NavMeshAgent agent;
    private QueueManager myManager;
    private PlayerShop myShop;

    [Header("Status NPC")]
    public bool canBuy = false;
    public bool isInQueue = false;
    public bool hasBought = false;

    [Header("Wander System")]
    public Transform wanderCenter;
    public Transform exitPoint; // Referensi jalan pulang
    public float wanderRadius = 5f;

    // Settingan Waktu
    public float timeToDisappear = 5f; // Waktu tunggu sebelum pulang
    private float timer;
    private bool isLeaving = false; // Status apakah dia lagi OTW pulang?

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 3.5f;
    }

    public void StartWandering()
    {
        // Fungsi helper buat nandain dia bukan pengantri
        isInQueue = false;
        canBuy = false;
    }

    void Update()
    {
        // Kalau statusnya lagi pulang, jangan lakukan logika lain
        if (isLeaving) return;

        // KONDISI 1: Kalau lagi antri
        if (isInQueue)
        {
            if (!agent.pathPending && agent.remainingDistance <= 0.5f)
            {
                agent.isStopped = true;
                transform.LookAt(transform.position + Vector3.back);
            }
            return;
        }

        // KONDISI 2: Mode Gabut (Wander)
        WanderBehavior();
    }

    void WanderBehavior()
    {
        if (wanderCenter == null) return;

        agent.isStopped = false;

        // Cek apakah sudah sampai di titik tujuan (Titik Kumpul Makanan)
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Mulai hitung mundur 5 detik
            timer += Time.deltaTime;

            if (timer >= timeToDisappear)
            {
                // SUDAH 5 DETIK -> PULANG
                GoHome();
            }
        }
        else
        {
            // Kalau belum sampai, jalan terus ke titik kumpul
            agent.SetDestination(wanderCenter.position);
        }
    }

    void GoHome()
    {
        isLeaving = true; // Set status biar update berhenti mikir

        if (exitPoint != null)
        {
            agent.isStopped = false;
            agent.SetDestination(exitPoint.position);
            Destroy(gameObject, 10f); // Hapus 10 detik kemudian (biar sempet jalan keluar layar)
        }
        else
        {
            // Kalau lupa set exit point, langsung hapus aja
            Destroy(gameObject);
        }
    }

    // --- INTERAKSI ANTRIAN ---

    public void Initialize(QueueManager manager, PlayerShop shop)
    {
        myManager = manager;
        myShop = shop;

        isInQueue = true;
        canBuy = false;
        myManager.AddToQueue(this);
    }

    public void GoToQueuePosition(Vector3 pos)
    {
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    public void EnableBuyingInteraction(bool status)
    {
        canBuy = status;
    }

    public void BuyBalloon()
    {
        if (myShop != null)
        {
            bool sukses = myShop.JualBarang();

            if (sukses)
            {
                hasBought = true;
                LeaveQueueAndGoHome();
            }
            else
            {
                LeaveQueueAndGoHome();
            }
        }
    }

    public void LeaveQueueAndGoHome()
    {
        isInQueue = false;
        canBuy = false;
        myManager.RemoveFromQueue(this);

        // Pulang
        GoHome();
    }

    public void LeaveShop(Vector3 exitPos)
    {
        // Ini dipanggil QueueManager kalau antrian penuh tiba-tiba
        isInQueue = false;
        canBuy = false;

        // Update exit point kalau belum ada
        if (exitPoint == null && exitPos != Vector3.zero)
        {
            // Bikin object dummy sementara kalau perlu, atau set manual
            // Tapi biasanya logic GoHome cukup
        }

        agent.isStopped = false;
        agent.SetDestination(exitPos);
        Destroy(gameObject, 10f);
    }
}
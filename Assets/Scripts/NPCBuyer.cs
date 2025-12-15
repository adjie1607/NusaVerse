using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCBuyer : MonoBehaviour
{
    private NavMeshAgent agent;
    private QueueManager myManager;
    private PlayerShop myShop; // <-- Tambahan referensi ke Shop

    private bool canBuy = false;
    private bool hasBought = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 3.5f;
        agent.stoppingDistance = 0f;
    }

    // Initialize sekarang menerima Shop juga
    public void Initialize(QueueManager manager, PlayerShop shop)
    {
        myManager = manager;
        myShop = shop; // <-- Simpan info shop
        myManager.AddToQueue(this);
    }

    public void GoToQueuePosition(Vector3 pos)
    {
        if (!hasBought)
        {
            agent.isStopped = false;
            agent.SetDestination(pos);
        }
    }

    public void EnableBuyingInteraction(bool status)
    {
        canBuy = status;
    }

    void Update()
    {
        if (canBuy && !hasBought)
        {
            if (!agent.pathPending && agent.remainingDistance <= 2.0f)
            {
                agent.isStopped = true;
                transform.LookAt(transform.position + Vector3.back);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TryToBuy(); // <-- Ganti jadi fungsi TryToBuy
                }
            }
        }
    }

    void TryToBuy()
    {
        // Panggil fungsi JualBarang di PlayerShop
        if (myShop != null)
        {
            bool sukses = myShop.JualBarang(); // <-- INI KUNCINYA

            if (sukses)
            {
                // Kalau sukses, NPC senang dan pergi
                hasBought = true;
                canBuy = false;
                myManager.RemoveFromQueue(this);

                if (myManager.exitPoint != null) LeaveShop(myManager.exitPoint.position);
            }
            else
            {
                // Kalau stok habis (return false)
                Debug.Log("Yah stok habis, NPC kecewa.");
                // Opsional: NPC pergi tanpa beli atau nunggu restock
                // Untuk sekarang kita buat dia pergi aja
                hasBought = true;
                canBuy = false;
                myManager.RemoveFromQueue(this);
                if (myManager.exitPoint != null) LeaveShop(myManager.exitPoint.position);
            }
        }
    }

    public void LeaveShop(Vector3 exitPos)
    {
        agent.isStopped = false;
        agent.SetDestination(exitPos);
        Destroy(gameObject, 10f);
    }
}
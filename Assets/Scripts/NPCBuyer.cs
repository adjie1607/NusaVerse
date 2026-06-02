using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCBuyer : MonoBehaviour
{
    private NavMeshAgent agent;
    private QueueManager myManager;
    private PlayerShop myShop;

    [Header("Status")]
    public bool canBuy = false; // Wajib Public biar terbaca SellingZone
    public bool hasBought = false;
    public bool isInQueue = false;

    // Supaya bisa pulang
    public Transform exitPoint;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = 3.5f;
    }

    public void Initialize(QueueManager manager, PlayerShop shop)
    {
        myManager = manager;
        myShop = shop;
        isInQueue = true;
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
        // Kalau sedang antri dan sudah sampai titik, hadap belakang
        if (isInQueue && !agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            agent.isStopped = true;
            transform.LookAt(transform.position + Vector3.back);
        }
    }

    // Dipanggil oleh SellingZone saat tombol F ditekan
    public void BuyBalloon()
    {
        if (myShop != null)
        {
            bool sukses = myShop.JualBarang();

            if (sukses)
            {
                Debug.Log("NPC Beli Sukses!");
                hasBought = true;
                LeaveShop(myManager.exitPoint.position);
            }
            else
            {
                Debug.Log("Stok Habis / Uang Kurang");
                LeaveShop(myManager.exitPoint.position);
            }
            // Hapus dari antrian
            myManager.RemoveFromQueue(this);
        }
    }

    public void LeaveShop(Vector3 exitPos)
    {
        isInQueue = false;
        canBuy = false;
        agent.isStopped = false;
        agent.SetDestination(exitPos);
        Destroy(gameObject, 10f);
    }
}
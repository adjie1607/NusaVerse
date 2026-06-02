using UnityEngine;

public class PlayerShop : MonoBehaviour
{
    [Header("Shop Settings")]
    public int balloonPrice = 15;
    public int expReward = 20;

    private PlayerLevel playerLevel;

    private void Start()
    {
        // Cari script PlayerLevel secara otomatis di Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerLevel = player.GetComponent<PlayerLevel>();
        }
    }

    // Fungsi ini dipanggil NPCBuyer saat tombol F ditekan
    public bool JualBarang()
    {
        // 1. Cek PlayerLevel ada atau tidak
        if (playerLevel == null) return false;

        // 2. Cek Stok Balon
        if (playerLevel.balloonCount <= 0)
        {
            Debug.Log("Stok Balon Habis!");
            return false;
        }

        // 3. Update UI & Data (Uang nambah, Balon kurang)
        playerLevel.TransaksiBerhasil(balloonPrice, 1);
        playerLevel.AddExp(expReward);

        return true;
    }
}
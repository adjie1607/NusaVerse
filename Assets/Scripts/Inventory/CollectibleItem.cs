using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public ItemData itemData; // Drag file data (ScriptableObject) kesini
    public bool isMoney = false;
    public int moneyAmount = 100;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isMoney)
            {
                InventoryManager.Instance.AddMoney(moneyAmount);
            }
            else
            {
                if (itemData != null)
                {
                    InventoryManager.Instance.AddItem(itemData);
                }
            }

            // Efek suara atau partikel bisa ditaruh disini
            Destroy(gameObject); // Hilangkan benda dari dunia
        }
    }
}
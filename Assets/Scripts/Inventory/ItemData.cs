using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea(3, 10)]
    public string description; // Penjelasan budaya
    public Sprite icon;        // Gambar untuk UI
    public GameObject prefab;  // (Opsional) Jika item bisa dibuang ke dunia 3D
}
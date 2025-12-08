using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Culture Item", menuName = "NusaVerse/Culture Item")]
public class CultureItemData : ScriptableObject
{
    public string id;              // ID unik (misal: "keris_01")
    public string itemName;        // Nama (misal: "Keris Empu Gandring")
    [TextArea]
    public string description;     // Deskripsi edukasi budaya
    public Sprite icon;            // Gambar untuk UI Inventory
    public GameObject modelPrefab; // (Opsional) Jika item bisa didrop jadi 3D object
}

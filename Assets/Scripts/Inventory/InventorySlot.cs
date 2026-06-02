using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image iconDisplay;
    public TMP_Text nameDisplay;
    public Button slotButton;

    public void SetupSlot(ItemData item, System.Action<ItemData> onClickCallback)
    {
        if (item == null) return;
        if (iconDisplay) iconDisplay.sprite = item.icon;
        if (nameDisplay) nameDisplay.text = item.itemName;

        if (slotButton)
        {
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => onClickCallback(item));
        }
    }
}
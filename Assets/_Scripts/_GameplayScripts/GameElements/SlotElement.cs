using UnityEngine;

[System.Serializable]
public class SlotElement : MonoBehaviour
{
    public int slotItemID;
    public string slotItemName;
    public float slotItemWeight;
    public Sprite slotItemImage;

    public SpriteRenderer slotItemViewSpriteRenderer;

    public void ChangeSlotItemDetails(SlotElement newSlotElementInfo)
    {
        this.slotItemID = newSlotElementInfo.slotItemID;
        this.slotItemName = newSlotElementInfo.slotItemName;
        this.slotItemWeight = newSlotElementInfo.slotItemWeight;
        this.slotItemImage = newSlotElementInfo.slotItemImage;
        
        this.slotItemViewSpriteRenderer.sprite = newSlotElementInfo.slotItemImage;
    }
}

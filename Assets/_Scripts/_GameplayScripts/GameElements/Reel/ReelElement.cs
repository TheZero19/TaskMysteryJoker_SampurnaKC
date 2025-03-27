using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ReelElement : MonoBehaviour
{
    [Header("Information of Reel Element (All Slot Elements)")]
    [SerializeField] private ReelElementSO reelElementSOAssetData;
    [SerializeField] private float reelScrollSpeed;
    [SerializeField] private Transform reelBottomEdgeTransform;

    [Header("Reel Element Generation")]
    [SerializeField] private Vector3 slotElementOffset;
    [SerializeField] private Vector2 slotElementDimensions;

    private Vector3 nextSlotElementInstantiationPos;

    private Vector3 slotElementResetPosition;
    private List<SlotElement> slotElementsInReel = new List<SlotElement>();
    private SlotElement topSlotElementInReelView;

    [SerializeField] private bool beginScroll = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerateSlotElements();
        nextSlotElementInstantiationPos = this.transform.position;
    }

    void FixedUpdate()
    {
        if (beginScroll == true)
        {
            ScrollReel();
        }
    }

    private void LateUpdate()
    {
        foreach (var slotElement in slotElementsInReel)
        {
            if (slotElement.transform.position.y <= reelBottomEdgeTransform.position.y)
            {
                RepositionTheSlotElementToTheTop(slotElement);
            }
        }
    }

    void GenerateSlotElements()
    {
        foreach (var slotElement in reelElementSOAssetData.allSlotElements)
        {
            var instantiatedSlotElement= Instantiate(slotElement, nextSlotElementInstantiationPos, Quaternion.identity);
            instantiatedSlotElement.transform.parent = this.transform;
            slotElementsInReel.Add(instantiatedSlotElement);
            DetermineNextSlotElementTransform();
            topSlotElementInReelView = instantiatedSlotElement;
        }
    }
    void DetermineNextSlotElementTransform()
    {
        nextSlotElementInstantiationPos += (slotElementOffset + new Vector3(0, slotElementDimensions.y, 0));
    }

    public void RepositionTheSlotElementToTheTop(SlotElement slotElement)
    {
        // slotElementResetPosition = topSlotElementInReelView.transform.position +
        //                            slotElementOffset + new Vector3(0, slotElementDimensions.y, 0); 
        // slotElement.gameObject.transform.position = slotElementResetPosition;
        slotElementResetPosition = topSlotElementInReelView.transform.localPosition+
                                   slotElementOffset + new Vector3(0, slotElementDimensions.y, 0); 
        slotElement.gameObject.transform.localPosition= slotElementResetPosition;
        
        topSlotElementInReelView = slotElement;
    }

    public void ScrollReel()
    {
        foreach (var slotElement in slotElementsInReel)
        {
            // slotElement.gameObject.transform.position -= new Vector3(0, reelScrollSpeed, 0);
            slotElement.gameObject.transform.localPosition -= new Vector3(0, reelScrollSpeed, 0);
        }
    }
}

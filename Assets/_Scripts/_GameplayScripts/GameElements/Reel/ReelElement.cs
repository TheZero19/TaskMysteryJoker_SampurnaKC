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
    [SerializeField] private Transform reelTopEdgeTransform;
    [SerializeField] private SlotElement resultSlotElementPrefab;

    [Header("Reel Element Generation")]
    [SerializeField] private Vector3 slotElementOffset;
    [SerializeField] private Vector2 slotElementDimensions;

    private Vector3 nextSlotElementInstantiationPos;
    private Vector3 nextResultSlotElementInstantiationPos;

    private Vector3 slotElementResetPosition;
    private Vector3 resultHoldingSlotElementResetPosition;
    private List<SlotElement> slotElementsInReel = new List<SlotElement>();
    private List<SlotElement> applicableResultHoldingSlotElementsInReel = new List<SlotElement>();
    private List<SlotElement> unapplicableResultHoldingSlotElementsInReel = new List<SlotElement>();
    private List<SlotElement> allResultsHoldingSlotElementsInReel= new List<SlotElement>();
    private SlotElement topSlotElementInReelView;
    private SlotElement topResultHoldingSlotElementInReelView;

    public bool beginScroll = false;

    // Start is called before the first frame update
    void Start()
    {
        nextSlotElementInstantiationPos = this.gameObject.transform.position;
        nextResultSlotElementInstantiationPos = this.gameObject.transform.position + slotElementOffset*6 + 
                                                new Vector3(0, slotElementDimensions.y, 0) * 6;
        GenerateSlotElements();
        GenerateResultHoldingSlotElements();
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

        foreach (var resultHoldingSlotElement in allResultsHoldingSlotElementsInReel)
        {
            if (resultHoldingSlotElement.transform.position.y <= reelBottomEdgeTransform.position.y)
            {
                RepositionTheResultHoldingSlotElementToTheTop(resultHoldingSlotElement);
            }
        }
    }

    void GenerateSlotElements()
    {
        int count = 0;
        foreach (var slotElement in reelElementSOAssetData.allSlotElements)
        {
            var instantiatedSlotElement= Instantiate(slotElement, nextSlotElementInstantiationPos, Quaternion.identity);
            instantiatedSlotElement.transform.parent = this.transform;
            slotElementsInReel.Add(instantiatedSlotElement);
            DetermineNextSlotElementTransform();
            topSlotElementInReelView = instantiatedSlotElement;
            if (count >= 3)
            {
                instantiatedSlotElement.gameObject.SetActive(false);
            }
            count += 1;
        }
    }

    void GenerateResultHoldingSlotElements()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                nextResultSlotElementInstantiationPos = new Vector3(nextResultSlotElementInstantiationPos.x, nextResultSlotElementInstantiationPos.y, 1);
                var resultHoldingSlotElement = Instantiate(resultSlotElementPrefab, nextResultSlotElementInstantiationPos, Quaternion.identity);
                resultHoldingSlotElement.transform.parent = this.transform;
                allResultsHoldingSlotElementsInReel.Add(resultHoldingSlotElement);
                unapplicableResultHoldingSlotElementsInReel.Add(resultHoldingSlotElement);
                nextResultSlotElementInstantiationPos += new Vector3(0, slotElementDimensions.y, 0) + slotElementOffset;
                topResultHoldingSlotElementInReelView = resultHoldingSlotElement;
            }
            else
            {
                nextResultSlotElementInstantiationPos = new Vector3(nextResultSlotElementInstantiationPos.x, nextResultSlotElementInstantiationPos.y, -1);
                var resultHoldingSlotElement = Instantiate(resultSlotElementPrefab, nextResultSlotElementInstantiationPos, Quaternion.identity);
                resultHoldingSlotElement.transform.parent = this.transform;
                allResultsHoldingSlotElementsInReel.Add(resultHoldingSlotElement);
                applicableResultHoldingSlotElementsInReel.Add(resultHoldingSlotElement);
                nextResultSlotElementInstantiationPos += new Vector3(0, slotElementDimensions.y, 0) + slotElementOffset;
                topResultHoldingSlotElementInReelView = resultHoldingSlotElement;
            }
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

    public void RepositionTheResultHoldingSlotElementToTheTop(SlotElement resultHoldingSlotElement)
    {
        resultHoldingSlotElementResetPosition = topResultHoldingSlotElementInReelView.transform.localPosition+
                                   slotElementOffset + new Vector3(0, slotElementDimensions.y, 0);
        if (applicableResultHoldingSlotElementsInReel.Contains(resultHoldingSlotElement))
        {
            resultHoldingSlotElementResetPosition = new Vector3(resultHoldingSlotElementResetPosition.x, resultHoldingSlotElementResetPosition.y, -1);
        }
        else
        {
            resultHoldingSlotElementResetPosition = new Vector3(resultHoldingSlotElementResetPosition.x, resultHoldingSlotElementResetPosition.y, 1);
        }
        resultHoldingSlotElement.gameObject.transform.localPosition= resultHoldingSlotElementResetPosition;
        
        topResultHoldingSlotElementInReelView= resultHoldingSlotElement;
    }

    public void ScrollReel()
    {
        foreach (var slotElement in slotElementsInReel)
        {
            // slotElement.gameObject.transform.position -= new Vector3(0, reelScrollSpeed, 0);
            slotElement.gameObject.transform.localPosition -= new Vector3(0, reelScrollSpeed, 0);
            if (slotElement.gameObject.transform.position.y + slotElementDimensions.y + slotElementOffset.y *2 
                <= reelTopEdgeTransform.position.y)
            {
                //show
                slotElement.gameObject.SetActive(true);
            }
            else
            {
                //hide
                slotElement.gameObject.SetActive(false);
            }
        }

        foreach (var resultSlotElement in allResultsHoldingSlotElementsInReel)
        {
            resultSlotElement.gameObject.transform.localPosition -= new Vector3(0, reelScrollSpeed, 0);
            if (resultSlotElement.gameObject.transform.position.y + slotElementDimensions.y/2 
                <= reelTopEdgeTransform.position.y)
            {
                //show
                resultSlotElement.gameObject.SetActive(true);
            }
            else
            {
                //hide
                resultSlotElement.gameObject.SetActive(false);
            }
        }
    }

    public void ToggleSpinningReel(bool status)
    {
        if (status == true)
        {
            this.beginScroll = status;
        }
        else
        {
            StartCoroutine(StopSpin());
        }
    }

    private IEnumerator StopSpin()
    {
        yield return new WaitUntil(
            () => allResultsHoldingSlotElementsInReel[2] == topResultHoldingSlotElementInReelView);
        this.beginScroll = false;
        foreach (var slotElement in unapplicableResultHoldingSlotElementsInReel)
        {
            slotElement.gameObject.SetActive(false);
        }
    }

    public List<SlotElement> GetApplicableResultSlotElements()
    {
        return this.applicableResultHoldingSlotElementsInReel;
    }
}

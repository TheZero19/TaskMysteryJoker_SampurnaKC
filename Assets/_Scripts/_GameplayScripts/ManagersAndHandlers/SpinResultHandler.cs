using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class SpinResultHandler: MonoBehaviour
{
    private List<int> uniqueSlotItemIds = new List<int>();

    [SerializeField] private List<SlotElement> allUniqueSlotElements;
    [SerializeField] private List<ReelElement> allReelElements;
    private Random rnd = new Random();
    void Start()
    {
        GameManager.OnSpinStarted += OnSpinStarted;
        GameManager.OnSpinStopped += OnSpinStopped;
        
        PopulateUniqueSlotItemID();
    }

    private void OnSpinStarted()
    {
        Debug.Log("Current Spin Started");
        GameManager.CurrentOutcome = DetermineOutcome();
        foreach (var list in GameManager.CurrentOutcome)
        {
            Debug.Log($"Outcome: {list[0]}, {list[1]}, {list[2]}");
        }
        for (int i = 0; i < allReelElements.Count; i++)
        {
            var reelElement = allReelElements[i];
            var applicableResultSlotElements = reelElement.GetApplicableResultSlotElements();
            var firstElement = allUniqueSlotElements.FirstOrDefault(slotElement => slotElement.slotItemID == GameManager.CurrentOutcome[i][2]);
            var secondElement = allUniqueSlotElements.FirstOrDefault(slotElement => slotElement.slotItemID == GameManager.CurrentOutcome[i][1]);
            var thirdElement = allUniqueSlotElements.FirstOrDefault(slotElement => slotElement.slotItemID == GameManager.CurrentOutcome[i][0]);
            applicableResultSlotElements[0].ChangeSlotItemDetails(firstElement);
            applicableResultSlotElements[1].ChangeSlotItemDetails(secondElement); 
            applicableResultSlotElements[2].ChangeSlotItemDetails(thirdElement);
        }
    }

    private void OnSpinStopped()
    {
        //If we'd like to run something from SpinResultHandler after the ending of each spin.
        Debug.Log("Current Spin Ended");

    }

    private void PopulateUniqueSlotItemID()
    {
        foreach (var uniqueSlotElement in allUniqueSlotElements)
        {
            var isSlotElementIDUnique = true;
            foreach (int uniqueSlotItemId in uniqueSlotItemIds)
            {
                if (uniqueSlotItemId == uniqueSlotElement.slotItemID)
                {
                    isSlotElementIDUnique = false;
                    break;
                }
            }
            if(isSlotElementIDUnique) uniqueSlotItemIds.Add(uniqueSlotElement.slotItemID);
        }

        foreach (var itemID in uniqueSlotItemIds)
        {
            Debug.Log($"Unique Slot Item ID: {itemID}");
        }
    }

    private List<List<int>> DetermineOutcome()
    {
        var outcome = rnd.Next(2);
        List<List<int>> serverGeneratedOutcome = new List<List<int>>();
        if (outcome == 0)
        {
            //lose
            GameManager.IsCurrentSpinAWin= false;
            Debug.Log("Selected outcome is a loss");
            serverGeneratedOutcome = Server.RespondLoss(uniqueSlotItemIds);
        }else if (outcome == 1)
        {
            //win
            GameManager.IsCurrentSpinAWin= true;
            Debug.Log("Selected outcome is a win");
            serverGeneratedOutcome = Server.RespondWin(uniqueSlotItemIds);
        }
        else
        {
            //not possible but still
            GameManager.IsCurrentSpinAWin= false;
            serverGeneratedOutcome = Server.RespondLoss(uniqueSlotItemIds);
        }
        
        return serverGeneratedOutcome;
    }

   

    private void OnDestroy()
    {
        GameManager.OnSpinStarted -= OnSpinStarted;
        GameManager.OnSpinStopped -= OnSpinStopped;
    }
}

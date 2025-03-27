using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphicsHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> winLines;
    [SerializeField] private Transform winLinesParentTransform;
    [Header("For finding the proper position of lines, Any one reel element will be good enough.")]
    [SerializeField] private ReelElement reelElement;
    void Start()
    {
        GameManager.OnSpinStarted += OnSpinStarted;
        GameManager.OnSpinStopped += OnSpinStopped;
        DisableAllWinLines();
    }

    private void OnSpinStarted()
    {
        DisableAllWinLines();
    }
    
    private void OnSpinStopped()
    {
        var applicableResultSlotElements= reelElement.GetApplicableResultSlotElements();
        winLinesParentTransform.position = new Vector3(winLinesParentTransform.position.x, applicableResultSlotElements[1].transform.position.y, 
            winLinesParentTransform.position.z);
        DetermineActiveWinLines();
    }

    private void DetermineActiveWinLines()
    {
        for (int i = 0; i < 5; i++)
        {
            winLines[i].gameObject.SetActive(GameManager.ActiveWinLines[i]);
        }
    }

    private void DisableAllWinLines()
    {
        foreach (var winLine in winLines) winLine.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.OnSpinStarted -= OnSpinStarted;
        GameManager.OnSpinStopped -= OnSpinStopped;
    }
}

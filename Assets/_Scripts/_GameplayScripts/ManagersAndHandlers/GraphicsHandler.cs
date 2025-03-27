using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphicsHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> winLines;
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

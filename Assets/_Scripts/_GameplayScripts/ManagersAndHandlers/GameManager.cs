using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<ReelElement> allReelElements;
    [SerializeField] private bool _isSpinning = false;
    [SerializeField] private float maxSpinTime = 4f;
    [SerializeField] private List<float> minSpinTimeList = new List<float>(){0.1f, 0.2f, 0.3f, 0.4f, 0.5f};

    public static float CurrentBetAmount;
    public static Action OnSpinStarted;
    public static Action OnSpinStopped;
    public static bool IsCurrentSpinAWin;
    public static List<List<int>> CurrentOutcome;
    public static int NumberOfTrios;
    public static List<bool> ActiveWinLines;
    public static float CurrencyAmount;
    
    Random rnd = new Random();
    void Start()
    {
       LoadCurrencyAmount();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSpin();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if(_isSpinning)
                StopSpin();
        }
    }

    private void LoadCurrencyAmount()
    {
        if (PlayerPrefs.HasKey(GameplayConstants.CURRENCY_AMOUNT))
        {
            CurrencyAmount = PlayerPrefs.GetFloat(GameplayConstants.CURRENCY_AMOUNT);
        }
        else
        {
            //If new user, grant 500 currency initially
            CurrencyAmount = 500;
            PlayerPrefs.SetFloat(GameplayConstants.CURRENCY_AMOUNT, CurrencyAmount);
            PlayerPrefs.Save();
        }
        Debug.Log($"Player has {CurrencyAmount} currency amount");
    }

    public void StartSpin()
    {
        if (CurrencyAmount >= CurrentBetAmount)
        {
            StartCoroutine(StartSpinCoroutine());
        }
    }
    private void StopSpin()
    {
        StartCoroutine(StopSpinCoroutine());
    }
    private IEnumerator StartSpinCoroutine()
    {
        _isSpinning = true;
        foreach (ReelElement reelElement in allReelElements)
        {
            reelElement.ToggleSpinningReel(true);
        }
        OnSpinStarted?.Invoke();
        yield return new WaitForSeconds(maxSpinTime);
        //if the spin outcome is to be obtained from the server, we have to wait for that too.
        //but for this, WaitForSeconds should be enough of a timeframe to generate the spin outcome
        
        if(_isSpinning)
            StopSpin();
    }

    private IEnumerator StopSpinCoroutine()
    {
        var minSpinTime = minSpinTimeList[rnd.Next(minSpinTimeList.Count)];
        yield return new WaitForSeconds(minSpinTime);
        _isSpinning = false;
        foreach (ReelElement reelElement in allReelElements)
        {
            reelElement.ToggleSpinningReel(false);
        }

        yield return new WaitUntil(() => allReelElements.All(reelElement => reelElement.beginScroll == false));
        GameManager_OnSpinStopped();
        Debug.Log("Before SpinStopped Public event is called: " + ActiveWinLines[0] + ", " + ActiveWinLines[1] + ", " + ActiveWinLines[2] + ", " + ActiveWinLines[3] + ", " + ActiveWinLines[4]);
        OnSpinStopped?.Invoke();
    }

    private void GameManager_OnSpinStopped()
    {
        DetermineTriosInCurrentSpin();
        if (!IsCurrentSpinAWin)
        {
            NumberOfTrios = 0;
        }
        else
        {
            foreach (var boolValue in ActiveWinLines.Where(boolValue => boolValue == true))
            {
                NumberOfTrios += 1;
            }
        }
    }
    
    private static void DetermineTriosInCurrentSpin()
    {
        List<List<int>> winConditionsIndexPool = new List<List<int>>()
        {
            new List<int> {CurrentOutcome[0][0], CurrentOutcome[1][0], CurrentOutcome[2][0]},
            new List<int> {CurrentOutcome[0][1], CurrentOutcome[1][1], CurrentOutcome[2][1]},
            new List<int> {CurrentOutcome[0][2], CurrentOutcome[1][2], CurrentOutcome[2][2]},
            new List<int> {CurrentOutcome[0][0], CurrentOutcome[1][1], CurrentOutcome[2][2]},
            new List<int> {CurrentOutcome[0][2], CurrentOutcome[1][1], CurrentOutcome[2][0]},
        };
        
        bool isWinCondition1 = winConditionsIndexPool[0].All(element => element == winConditionsIndexPool[0][0]);
        bool isWinCondition2 = winConditionsIndexPool[1].All(element => element == winConditionsIndexPool[0][1]);
        bool isWinCondition3 = winConditionsIndexPool[2].All(element => element == winConditionsIndexPool[0][2]);
        bool isWinCondition4 = winConditionsIndexPool[3].All(element => element == winConditionsIndexPool[0][0]);
        bool isWinCondition5 = winConditionsIndexPool[4].All(element => element == winConditionsIndexPool[0][2]);

        var boolList = new List<bool>()
        {
            isWinCondition1,
            isWinCondition2,
            isWinCondition3,
            isWinCondition4,
            isWinCondition5
        };

        ActiveWinLines = boolList;
    }
}

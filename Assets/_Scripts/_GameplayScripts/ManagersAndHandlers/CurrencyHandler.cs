using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyHandler : MonoBehaviour
{
    void Start()
    {
        GameManager.OnSpinStarted += OnSpinStarted;
        GameManager.OnSpinStopped += OnSpinStopped;
    }

    private void OnSpinStarted()
    {
        DeductCurrencyForSpin();
    }

    private void OnSpinStopped()
    {
        if (!GameManager.IsCurrentSpinAWin) return;
        HandleOnSpinWon();
    }

    private void DeductCurrencyForSpin()
    {
        GameManager.CurrencyAmount -= GameManager.CurrentBetAmount;
        PlayerPrefs.SetFloat(GameplayConstants.CURRENCY_AMOUNT, GameManager.CurrencyAmount);
        PlayerPrefs.Save();
        Debug.Log("Deducted bet amount to start spin. Remaining: " + GameManager.CurrencyAmount);
    }

    private void HandleOnSpinWon()
    {
        var wonCurrency = GameManager.NumberOfTrios * GameManager.CurrentBetAmount * 2;
        GameManager.CurrencyAmount += wonCurrency;
        PlayerPrefs.SetFloat(GameplayConstants.CURRENCY_AMOUNT, GameManager.CurrencyAmount);
        PlayerPrefs.Save();
        Debug.Log("Spin won! Adding to currency. Remaining: " + GameManager.CurrencyAmount);
    }

    private void OnDestroy()
    {
        GameManager.OnSpinStarted -= OnSpinStarted;
        GameManager.OnSpinStopped -= OnSpinStopped;
    }
}

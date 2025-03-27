using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInfoManager : MonoBehaviour
{
    [SerializeField] private TMP_Text balanceAmountText;
    [SerializeField] private TMP_Text betAmountText;

    [SerializeField] private TMP_Text totalWinAmountText;
    void Start()
    {
        GameManager.OnGameManagerLoaded += InitializeUIInfo;
        CurrencyHandler.OnCurrencyChanged += ChangeUIInfo;
        ButtonsHandler.OnBetChanged += ChangeBetInfo;
    }

    void InitializeUIInfo()
    {
        balanceAmountText.text = GameManager.CurrencyAmount.ToString();
        betAmountText.text = GameManager.CurrentBetAmount.ToString();
        totalWinAmountText.text = 0.ToString();
    }

    void ChangeUIInfo()
    {
        balanceAmountText.text = GameManager.CurrencyAmount.ToString();
        betAmountText.text = GameManager.CurrentBetAmount.ToString();
        totalWinAmountText.text = GameManager.CurrentSpinWinAmount.ToString();
    }

    void ChangeBetInfo()
    {
        betAmountText.text = GameManager.CurrentBetAmount.ToString();
    }

    void OnDestroy()
    {
        GameManager.OnGameManagerLoaded -= InitializeUIInfo;
        CurrencyHandler.OnCurrencyChanged -= ChangeUIInfo;
        ButtonsHandler.OnBetChanged -= ChangeBetInfo;
    }
}

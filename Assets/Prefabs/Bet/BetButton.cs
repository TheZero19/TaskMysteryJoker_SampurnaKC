using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BetButton : MonoBehaviour
{
    [SerializeField] private float betAmount;
    [SerializeField] private Button buttonComponent;
    [SerializeField] private TMP_Text betAmountText;

    private void Start()
    {
        this.betAmountText.text = $"Bet {this.betAmount}";
    }
    public void Bet()
    {
        GameManager.CurrentBetAmount = betAmount;
        buttonComponent.interactable = false;
        Debug.Log($"Bet Amount {GameManager.CurrentBetAmount}");
    }
}

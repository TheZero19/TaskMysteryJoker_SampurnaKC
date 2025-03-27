using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsHandler : MonoBehaviour
{
    [SerializeField] private List<Button> betButtons;
    [SerializeField] private Button defaultBetHoldingButton;

    [SerializeField] private Button spinButton, stopButton;
    
    public static ButtonsHandler Singleton;
    public static Action OnBetChanged;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
    }
    void Start()
    {
        defaultBetHoldingButton.onClick.Invoke();
        GameManager.OnSpinStopped += OnSpinStopped;
    }

    public void OnOneOfTheBetButtonsClicked_ChangeUI(Button clickedButton)
    {
        foreach (Button button in betButtons)
        {
            if (button != clickedButton)
            {
                button.interactable = true;
            }
        }
    }
    private void OnSpinStopped()
    {
        spinButton.interactable = true;
    }
    public void OnSpinButtonClicked()
    {
        spinButton.interactable = false;
        spinButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
        GameManager.Singleton.StartSpin();
    }

    public void OnStopButtonClicked()
    {
        stopButton.gameObject.SetActive(false);
        spinButton.gameObject.SetActive(true);
        GameManager.Singleton.StopSpin();
    }

    private void OnDestroy()
    {
        GameManager.OnSpinStopped -= OnSpinStopped;
    }
}

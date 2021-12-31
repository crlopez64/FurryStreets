using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDCurrencyHolder : MonoBehaviour
{
    private float timer;
    private int moneyAmount;
    private int noviAmount;

    public Image moneyImage;
    public Image noviImage;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI noviText;

    void Update()
    {
        if (timer >= 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            TurnOffVisuals();
        }
    }

    /// <summary>
    /// Set the novi amount and turn on the canvas.
    /// </summary>
    /// <param name="novi"></param>
    public void SetNovi(int novi)
    {
        noviAmount = novi;
        TurnOnVisuals();
    }
    /// <summary>
    /// Set the money amount and turn on the canvas.
    /// </summary>
    /// <param name="money"></param>
    public void SetMoney(int money)
    {
        moneyAmount = money;
        TurnOnVisuals();
    }
    /// <summary>
    /// Turn on the visuals as well as the canvas.
    /// </summary>
    public void TurnOnVisuals()
    {
        timer = 5f;
        moneyText.text = moneyAmount.ToString();
        noviText.text = noviAmount.ToString();
        moneyImage.enabled = true;
        noviImage.enabled = true;
        moneyText.enabled = true;
        noviText.enabled = true;
        gameObject.SetActive(true);
    }
    /// <summary>
    /// Turn off the visuals as well as the canvas.
    /// </summary>
    public void TurnOffVisuals()
    {
        moneyImage.enabled = false;
        noviImage.enabled = false;
        moneyText.enabled = false;
        noviText.enabled = false;
        gameObject.SetActive(false);
    }
}

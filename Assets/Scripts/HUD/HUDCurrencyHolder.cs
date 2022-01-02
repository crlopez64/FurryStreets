using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script in charge of keeping track of currency.
/// </summary>
public class HUDCurrencyHolder : MonoBehaviour
{
    private float timer;
    private int moneyAmount;

    public Image moneyImage;
    public TextMeshProUGUI moneyText;

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
        moneyImage.enabled = true;
        moneyText.enabled = true;
        gameObject.SetActive(true);
    }
    /// <summary>
    /// Turn off the visuals as well as the canvas.
    /// </summary>
    public void TurnOffVisuals()
    {
        moneyImage.enabled = false;
        moneyText.enabled = false;
        gameObject.SetActive(false);
    }
}

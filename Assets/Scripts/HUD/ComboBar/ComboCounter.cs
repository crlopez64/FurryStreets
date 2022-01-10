using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of showing a player's current combo.
/// </summary>
public class ComboCounter : MonoBehaviour
{
    private Image staticText;
    private ComboNumber number;
    private byte everyOtherTick;
    private float timer;
    private float halfTimer;

    private void Awake()
    {
        number = GetComponentInChildren<ComboNumber>();
        staticText = GetComponentInChildren<Image>();
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            TurnOffCombo();
        }
    }
    private void FixedUpdate()
    {
        if ((timer <= halfTimer) && (timer > 0f))
        {
            if (everyOtherTick == 0)
            {
                number.gameObject.SetActive(!number.gameObject.activeInHierarchy);
                everyOtherTick = 2;
            }
            else
            {
                everyOtherTick--;
            }
        }
    }


    /// <summary>
    /// Set the text and timer of this combo value.
    /// </summary>
    /// <param name="currentValue"></param>
    public void SetText(int currentValue, float timerValue)
    {
        if (timerValue >= 5f)
        {
            timerValue = 5f;
        }
        if (!staticText.gameObject.activeInHierarchy)
        {
            staticText.gameObject.SetActive(true);
        }
        everyOtherTick = 2;
        number.gameObject.SetActive(true);
        number.SetText(currentValue);
        timer = timerValue;
        halfTimer = timerValue / 2;
    }
    /// <summary>
    /// Flash the combo meter before turning off. If timer already below pre-determined threshold, this does nothing.
    /// </summary>
    public void PlayerHit()
    {
        if (timer <= 0.5f)
        {
            return;
        }
        halfTimer = 0.5f;
        timer = 0.5f;
    }
    /// <summary>
    /// Turn off the combo counter and set reset it.
    /// </summary>
    public void TurnOffCombo()
    {
        number.SetText(0);
        number.gameObject.SetActive(false);
        staticText.gameObject.SetActive(false);
    }
}

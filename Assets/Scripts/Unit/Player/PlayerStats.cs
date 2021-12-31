using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of the Player's stats.
/// </summary>
public class PlayerStats : UnitStats
{

    public void Start()
    {
        SetTest();
        if (staminaBar != null)
        {
            staminaBar.SetMaxValue(maxHealth);
            staminaBar.SetSliderValue(currentHealth);
        }
        if (meterBar != null)
        {
            meterBar.SetMaxValue(maxMeter);
            meterBar.SetSliderValue(currentMeter);
        }
    }
}

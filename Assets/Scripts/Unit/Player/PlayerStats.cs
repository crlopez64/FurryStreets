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
        if (healthBar != null)
        {
            healthBar.SetMaxValue(maxHealth);
            healthBar.SetSliderValue(currentHealth);
        }
        if (meterBar != null)
        {
            meterBar.SetMaxValue(maxMeter);
            meterBar.SetSliderValue(currentMeter);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of the Player's stats.
/// </summary>
public class PlayerStats : UnitStats
{
    private float comboTimer;
    private int highestCombo;
    private int comboHits;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("Get Player One.");
        GetMeter(1);
        GetComboCounter(1);
        SetTestWolf();
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
        }
        else
        {
            ResetComboTooLong();
        }
    }

    /// <summary>
    /// Add one hit to this Player's Combo.
    /// </summary>
    public void AddToCombo()
    {
        comboHits++;
        if (comboHits > highestCombo)
        {
            highestCombo = comboHits;
        }
        comboTimer = GetTimer();
        comboCounter.SetText(comboHits, comboTimer);
    }
    /// <summary>
    /// Reset the Combo is taking too long.
    /// </summary>
    public void ResetComboTooLong()
    {
        comboHits = 0;
    }
    /// <summary>
    /// Reset the Combo due the player getting hit.
    /// </summary>
    public void ResetComboHit()
    {
        comboHits = 0;
        comboTimer = 0;
        comboCounter.PlayerHit();
    }

    public void SetTestWolf()
    {
        meters.SetMetersWolf();
        statAttack = 5;
        statDefense = 2;
        statMeterGain = 2;
        maxHealth = 1000;
        maxMeter = 650;
        maxStun = 1000;
        currentHealth = maxHealth;
        currentMeter = maxMeter;
        meters.SetHealthBarCurrent(currentHealth);
        meters.SetMeterBarCurrent(currentMeter, false);
        GetComponentInChildren<ParticlePooler>().SetParticlesSpecialMovesWolf();
    }
    /// <summary>
    /// Get Timer for current Combo hit. The higher the combo, the less time to increase the combo hits.
    /// </summary>
    /// <returns></returns>
    private float GetTimer()
    {
        if (comboHits < 10)
        {
            return 5f;
        }
        else if ((comboHits >= 10) && (comboHits < 20))
        {
            return 4.5f;
        }
        else if ((comboHits >= 20) && (comboHits < 30))
        {
            return 4f;
        }
        else if ((comboHits >= 30) && (comboHits < 40))
        {
            return 3.5f;
        }
        else if ((comboHits >= 40) && (comboHits < 50))
        {
            return 3f;
        }
        else
        {
            return 2.5f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stats for the Playable Fox, name pending.
/// </summary>
public class FoxStats : UnitStats
{
    protected override void Awake()
    {
        base.Awake();
        maxHealth = 80;
        maxMeter = 100;
        statAttack = 2;
        statDefense = 2;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stats for the Playable Wolf, name pending.
/// </summary>
public class WolfStats : UnitStats
{

    protected override void Awake()
    {
        base.Awake();
        maxHealth = 1000;
        maxMeter = 650;
        maxStun = 400;
        statAttack = 3;
        statDefense = 1;
    }

    
}

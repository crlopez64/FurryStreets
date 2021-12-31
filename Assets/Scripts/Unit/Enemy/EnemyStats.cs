using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used for keepign track of enemy stats, including enemy drops when defeated.
/// </summary>
public class EnemyStats : UnitStats
{

    protected override void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        SetTest();
        
    }

}

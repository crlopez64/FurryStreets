using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for identifying a hitbox that will only trigger to any Units not on ground.
/// </summary>
public class HitboxAirborne : Hitbox
{
    private void Awake()
    {
        forAirborne = true;
    }
}

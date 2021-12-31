using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for having the player teleport to entrances via an Action.
/// </summary>
public class EntranceTeleport : LocationTeleport
{
    protected override void Start()
    {
        base.Start();
        mustActionButton = true;
    }
}

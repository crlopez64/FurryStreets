using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for having the Player teleport via open roads
/// </summary>
public class OpenRoadTeleport : LocationTeleport
{
    protected override void Start()
    {
        base.Start();
        mustActionButton = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that keeps track of all the HUDS for 4 players.
/// </summary>
public class HUDMetersGrid : MonoBehaviour
{
    private HUDMeters[] meters;

    private void Awake()
    {
        meters = GetComponentsInChildren<HUDMeters>();
    }
    private void Start()
    {
        
    }

    /// <summary>
    /// Get the HUD meter based on the player. First player should use "1."
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public HUDMeters GetMeter(byte player)
    {
        if (player == 0)
        {
            player = 1;
        }
        if (player > 4)
        {
            player = 4;
        }
        return meters[player - 1];
    }
}

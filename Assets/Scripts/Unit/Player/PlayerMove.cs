using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The script of only the Player's movement basic motor controls.
/// </summary>
public class PlayerMove : UnitMove
{

    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Start()
    {
        base.Start();
        CanFlip(true);
        CanMove(true);
        SetSpeed(7, 4);
        SetJumpHeight(100);
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of NPC Moving.
/// </summary>
public class NPCMove : UnitMove
{

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        SetSpeedSlow();
        CanMove(true);
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    /// <summary>
    /// Set NPC Walk speed to slow.
    /// </summary>
    public void SetSpeedSlow()
    {
        SetSpeed(1, 1);
    }
    /// <summary>
    /// Set NPC Walk to medium.
    /// </summary>
    public void SetSpeedMedium()
    {
        SetSpeed(2, 1);
    }
    /// <summary>
    /// Set NPC Walk to medium.
    /// </summary>
    public void SetSpeedFast()
    {
        SetSpeed(3, 2);
    }
}

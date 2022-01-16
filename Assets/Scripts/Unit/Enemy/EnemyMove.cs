using UnityEngine;

/// <summary>
/// Script in charge of Enemy's movement.
/// </summary>
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(UnitAnimationLayers))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMove : UnitMove
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        //SetMoveSmoothing(true);
        CanFlip(true);
        CanMove(true);
        SetSpeed(5, 3);
        SetJumpHeight(25);
        isEnemy = true;
    }
    protected override void Update()
    {
        base.Update();
        if (!unitAttack.IsAttacked())
        {
            Move(Vector2.zero);
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Grounded Player = Layer 6
 * Airborne Player = Layer 7
 * 
 */

/// <summary>
/// Script in charge of having a Unit move.
/// </summary>
public class UnitMove : MonoBehaviour
{
    private UnitAnimationLayers unitAnimationLayers;
    private Animator animator;
    private Rigidbody2D rb2D;
    private Vector3 initialGroundedPosition;
    private Vector2 velocityRef;
    private Vector2 velocity;
    private bool grounded;
    private bool canMove;
    private bool canFlip;
    private bool moveSmoothing;
    private int gravityScale;
    //TODO: Add hit stun timer.
    private float hitstunTimer;
    private float groundCheckTimer;

    protected UnitAttack unitAttack;
    protected bool isEnemy;
    protected uint horizontalSpeed;
    protected uint verticalSpeed;
    protected uint jumpHeight;

    protected virtual void Awake()
    {
        unitAnimationLayers = GetComponent<UnitAnimationLayers>();
        unitAttack = GetComponent<UnitAttack>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start()
    {
        grounded = true;
        initialGroundedPosition = transform.position;
        gravityScale = 6;
    }
    protected virtual void Update()
    {
        //Timers
        if (groundCheckTimer > 0)
        {
            groundCheckTimer -= Time.deltaTime;
        }
        //Direction Facing
        if ((!unitAttack.CurrentlyAttacking()) && (!unitAttack.IsAttacked()))
        {
            if (canFlip && grounded)
            {
                if (velocity.x > 0)
                {
                    Vector3 scaleTemp = transform.localScale;
                    scaleTemp.x = 1;
                    transform.localScale = scaleTemp;
                }
                else if (velocity.x < 0)
                {
                    Vector3 scaleTemp = transform.localScale;
                    scaleTemp.x = -1;
                    transform.localScale = scaleTemp;
                }
            }
        }

        //Animator
        //animator.SetBool("Stunned", Stunned());
        animator.SetBool("Grounded", grounded);
        animator.SetFloat("VelocityX", rb2D.velocity.x);
        animator.SetFloat("VelocityY", rb2D.velocity.y);
        animator.SetFloat("VelocityAll", rb2D.velocity.magnitude);
    }
    protected virtual void FixedUpdate()
    {
        //Landing back to ground after being in air
        if ((!grounded) && CanCheckGround())
        {
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            if (transform.position.y <= initialGroundedPosition.y)
            {
                grounded = true;
                rb2D.gravityScale = 0;
                gameObject.layer = isEnemy ? 7 : 6;
                initialGroundedPosition = new Vector3(transform.position.x, initialGroundedPosition.y, transform.position.z);
                transform.position = initialGroundedPosition;
                if (unitAttack.IsAttacked())
                {
                    velocity = Vector2.zero;
                    rb2D.velocity = velocity;
                }
            }
        }
        //Movement Physics
        if (grounded)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            unitAttack.ResetHitTypeRecord();
            if (unitAttack.IsAttacked() || unitAttack.CurrentlyAttacking())
            {
                velocity = Vector2.SmoothDamp(velocity, Vector2.zero, ref velocityRef, 0.15f);
                rb2D.velocity = velocity;
            }
            else
            {
                if (moveSmoothing)
                {
                    rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, velocity, ref velocityRef, 0.15f);
                }
                else
                {
                    rb2D.velocity = velocity;
                }
            }
        }
        else
        {
            //velocity.x = Mathf.Clamp(velocity.x, -30, 30);
            rb2D.velocity = new Vector2(velocity.x, rb2D.velocity.y);
        }
    }

    /// <summary>
    /// Flip the character's sprite.
    /// </summary>
    public void FlipSprite()
    {
        Vector3 scaleTemp = transform.localScale;
        scaleTemp.x *= -1;
        transform.localScale = scaleTemp;
    }
    /// <summary>
    /// Make the Unit move.
    /// </summary>
    public void Move(Vector2 directionalInput)
    {
        if ((!canMove) || unitAttack.CurrentlyAttacking() || unitAttack.CurrentlyGrabbing()
            || unitAttack.IsAttacked() || unitAttack.Stunned())
        {
            return;
        }
        directionalInput = new Vector2(Mathf.Clamp(directionalInput.x, -1, 1), Mathf.Clamp(directionalInput.y, -1, 1));
        velocity = new Vector2(horizontalSpeed * directionalInput.x, verticalSpeed * directionalInput.y);
    }
    /// <summary>
    /// Make the Unit stop moving completely if on the ground.
    /// </summary>
    public void StopMoving()
    {
        if (grounded)
        {
            velocity = Vector2.zero;
            rb2D.velocity = velocity;
        }
    }
    /// <summary>
    /// Set if the Unit can move or not.
    /// </summary>
    /// <param name="tOrF"></param>
    public void CanMove(bool tOrF)
    {
        canMove = tOrF;
    }
    /// <summary>
    /// Set if the Unit can flip sprites to the other direction.
    /// </summary>
    /// <param name="tOrF"></param>
    public void CanFlip(bool tOrF)
    {
        canFlip = tOrF;
    }
    /// <summary>
    /// Make the Unit jump.
    /// </summary>
    public void Jump(Vector2 directionalInput)
    {
        if (grounded)
        {
            velocity = new Vector2(Mathf.Clamp(directionalInput.x, -1, 1) * horizontalSpeed, jumpHeight);
            MakeJump(velocity, true);
        }
    }
    /// <summary>
    /// Make the Unit forcefully move when attacking.
    /// </summary>
    public void AttackMove()
    {
        if (grounded)
        {
            velocity = new Vector2(Mathf.Sign(transform.localScale.x) * Mathf.Abs(unitAttack.GetAttackToAnimate().GetUnitToMove().x),
                Mathf.Abs(unitAttack.GetAttackToAnimate().GetUnitToMove().y));
            rb2D.velocity = velocity;
        }
    }
    /// <summary>
    /// Make the Unit knockback until further notice.
    /// </summary>
    /// <param name="knockback"></param>
    public void Knockback(Vector3 attackerPosition, Vector2 knockback, byte hitType)
    {
        canMove = false;
        float direction = (transform.position.x >= attackerPosition.x) ? 1 : -1;
        if (Mathf.Sign(transform.localScale.x) != (-direction))
        {
            FlipSprite();
        }
        if (grounded)
        {
            //Make initial knockback
            if (hitType <= 2)
            {
                knockback.y = 0;
                velocity = new Vector2(direction * knockback.x, Mathf.Abs(knockback.y));
            }
            else
            {
                velocity = new Vector2(direction * knockback.x, Mathf.Abs(knockback.y));
                MakeJump(velocity, true);
            }
        }
        else
        {
            //Reduce the next knockback
            velocity = new Vector2(direction * knockback.x, Mathf.Abs(knockback.y) * 0.9f);
            MakeJump(velocity, false);
        }
    }
    /// <summary>
    /// Set if Movement should be smooth.
    /// </summary>
    /// <param name="tOrF"></param>
    public void SetMoveSmoothing(bool tOrF)
    {
        moveSmoothing = tOrF;
    }
    
    /// <summary>
    /// Is the Unit touching the ground?
    /// </summary>
    /// <returns></returns>
    public bool Grounded()
    {
        return grounded;
    }
    /// <summary>
    /// Is the Unit jumping up or popping up in the air?
    /// </summary>
    /// <returns></returns>
    public bool FlyingUp()
    {
        return (!grounded) && (rb2D.velocity.y > 0);
    }
    /// <summary>
    /// Return the X Speed of the Unit.
    /// </summary>
    /// <returns></returns>
    public float HorizontalSpeed()
    {
        return rb2D.velocity.x;
    }
    /// <summary>
    /// Set the personal Speeds of the Unit.
    /// </summary>
    /// <param name="horizontalSpeed"></param>
    /// <param name="verticalSpeed"></param>
    protected void SetSpeed(uint horizontalSpeed, uint verticalSpeed)
    {
        this.horizontalSpeed = horizontalSpeed;
        this.verticalSpeed = verticalSpeed;
    }
    /// <summary>
    /// Set the jump height of the Unit.
    /// </summary>
    /// <param name="jumpHeight"></param>
    protected void SetJumpHeight(uint jumpHeight)
    {
        this.jumpHeight = jumpHeight;
    }
    /// <summary>
    /// Helper method to make a Unit jump. Can be used for normal jumping and getting hit.
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="multiplier"></param>
    private void MakeJump(Vector2 velocity, bool wasGrounded)
    {
        if (wasGrounded)
        {
            initialGroundedPosition = transform.position;
        }
        rb2D.velocity = Vector2.zero;
        grounded = false;
        gameObject.layer = isEnemy ? 9 : 8;
        rb2D.gravityScale = gravityScale;
        rb2D.velocity = velocity;
        groundCheckTimer = 0.1f;
    }
    /// <summary>
    /// Can the Unit check if they are touching ground? This is so that any jump or knockback is possible.
    /// </summary>
    /// <returns></returns>
    private bool CanCheckGround()
    {
        return groundCheckTimer <= 0;
    }
}

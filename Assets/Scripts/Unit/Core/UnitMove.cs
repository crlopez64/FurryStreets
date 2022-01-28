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
    private Rigidbody2D rb2D;
    private Animator animator;
    private UnitShadow unitShadow;
    private UnitAnimationLayers unitAnimationLayers;
    private Vector3 initialGroundedPosition;
    private Vector2 velocity;
    private Vector2 velocityRef;
    private bool canMove;
    private bool canFlip;
    private bool grounded;
    private bool moveSmoothing;
    private byte doubleTaps;
    private byte lastDirectionTapped;
    private byte firstDirectionTapped;
    private int gravityScale;
    private float doubleTapTimer;
    private float groundCheckTimer;

    protected UnitAttack unitAttack;
    protected bool isEnemy;
    protected uint jumpHeight;
    protected uint verticalSpeed;
    protected uint horizontalSpeed;

    protected virtual void Awake()
    {
        unitAnimationLayers = GetComponent<UnitAnimationLayers>();
        unitAttack = GetComponent<UnitAttack>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        unitShadow = GetComponentInChildren<UnitShadow>();
    }
    protected virtual void Start()
    {
        grounded = true;
        initialGroundedPosition = transform.position;
        rb2D.drag = 15f;
        doubleTaps = 0;
        gravityScale = 12;
        lastDirectionTapped = 5;
        firstDirectionTapped = 0;
    }
    protected virtual void Update()
    {
        //Timers
        if (groundCheckTimer > 0)
        {
            groundCheckTimer -= Time.deltaTime;
        }
        if (doubleTapTimer > 0)
        {
            doubleTapTimer -= Time.deltaTime;
        }
        else
        {
            if (!IsRunning())
            {
                doubleTaps = 0;
                firstDirectionTapped = 0;
                if (rb2D.velocity != Vector2.zero)
                {
                    lastDirectionTapped = 5;
                }
            }
        }
        //Mess with Rigidbody drag for pop up combos
        //Also shadow things
        if (!grounded)
        {
            if (unitShadow != null)
            {
                unitShadow.TurnOffShadow();
            }
            if (rb2D.velocity.y > 2f)
            {
                rb2D.drag = 3f;
                gravityScale = 12;
                rb2D.gravityScale = gravityScale;
            }
            else if (rb2D.velocity.y <= 2f && rb2D.velocity.y >= -2f)
            {
                rb2D.drag = 3f;
                gravityScale = 4;
                rb2D.gravityScale = gravityScale;
            }
            else
            {
                rb2D.drag = 6f;
                gravityScale = 12;
                rb2D.gravityScale = gravityScale;
            }
        }
        else
        {
            if (unitShadow != null)
            {
                unitShadow.TurnOnShadow();
            }
            rb2D.drag = 15f;
        }
        //Direction Facing
        if (unitAttack != null)
        {
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
        }
        else
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

        //Animators
        if (animator != null)
        {
            animator.SetBool("Grounded", grounded);
            animator.SetBool("Running", IsRunning());
            animator.SetFloat("VelocityX", rb2D.velocity.x);
            animator.SetFloat("VelocityY", rb2D.velocity.y);
            animator.SetFloat("VelocityAll", rb2D.velocity.magnitude);
        }
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
                ParticleManager.Instance().SpawnLandingHitParticle(transform.position);
            }
        }
        //Movement Physics
        if (grounded)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
            if (unitAttack != null)
            {
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
    public void Move(Vector2 directionalInput, byte directionalByte)
    {
        if (unitAttack != null)
        {
            if (unitAttack.CurrentlyAttacking() || unitAttack.CurrentlyGrabbing() || unitAttack.IsAttacked()
                || unitAttack.Stunned())
            {
                return;
            }
            if (unitAttack.Blocking())
            {
                velocity = Vector2.zero;
                return;
            }
        }
        if (!canMove)
        {
            return;
        }
        if (directionalByte != lastDirectionTapped)
        {
            DoubleTap(directionalByte);
        }
        lastDirectionTapped = directionalByte;
        directionalInput = new Vector2(Mathf.Clamp(directionalInput.x, -1, 1), Mathf.Clamp(directionalInput.y, -1, 1));
        float currentHorizontalSpeed = IsRunning() ? horizontalSpeed * 1.8f : horizontalSpeed;
        float currentVerticalSpeed = IsRunning() ? verticalSpeed * 1.5f : verticalSpeed;
        velocity = new Vector2(currentHorizontalSpeed * directionalInput.x, currentVerticalSpeed * directionalInput.y);
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
    /// Make the Unit knockback until further notice.
    /// </summary>
    /// <param name="knockback"></param>
    public void Knockback(Vector3 attackerPosition, Attack incomingAttack, bool criticalStunned)
    {
        //Debug.Log("Knockback from: " + incomingAttack.GetName());
        canMove = false;
        float direction = (transform.position.x >= attackerPosition.x) ? 1 : -1;
        if (Mathf.Sign(transform.localScale.x) != (-direction))
        {
            FlipSprite();
        }
        if (grounded)
        {
            //Make initial knockback
            velocity = criticalStunned ? new Vector2(direction * 28f, 0) : new Vector2(direction * 2f, 0);
            if (incomingAttack.AttributeKnockbackFar())
            {
                velocity = new Vector2(direction * 60f, 15f);
                animator.SetTrigger("HitDistalKnockback");
                unitAnimationLayers.SetHitLayer();
                MakeJump(velocity, true);
                return;
            }
            if (incomingAttack.AttributeKnockback())
            {
                velocity = new Vector2(direction * 30f, 15f);
                animator.SetTrigger("HitDistalKnockback");
                unitAnimationLayers.SetHitLayer();
                MakeJump(velocity, true);
                return;
            }
            if (incomingAttack.AttributePopUp())
            {
                velocity = criticalStunned ? velocity = new Vector2(direction * 18f, 40f) : new Vector2(direction * 3f, 50f);
                animator.SetTrigger("HitAerialKnockback");
                unitAnimationLayers.SetHitLayer();
                MakeJump(velocity, true);
                return;
            }
            switch(incomingAttack.GetHitType())
            {
                case 1:
                    animator.SetTrigger("HitHigh");
                    unitAnimationLayers.SetHitLayer();
                    break;
                case 2:
                    animator.SetTrigger("HitLow");
                    unitAnimationLayers.SetHitLayer();
                    break;
                default:
                    animator.SetTrigger("HitHigh");
                    unitAnimationLayers.SetHitLayer();
                    break;
            }
        }
        else
        {
            //Reduce the next knockback
            if (incomingAttack.AttributeKnockback() || incomingAttack.AttributeKnockbackFar() || incomingAttack.AttributePopUp())
            {
                velocity = new Vector2(direction * 16f, 10f);
                MakeJump(velocity, false);
            }
            else
            {
                velocity = criticalStunned ? velocity = new Vector2(direction * 16f, 10f) : new Vector2(direction * 1f, 20f);
                MakeJump(velocity, false);
            }
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
    /// Spawn landing particles, if any.
    /// </summary>
    public void SpawnLandingParticles()
    {
        ParticleManager.Instance().SpawnLandingHitParticle(transform.position);
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
    /// Is this Unit running?
    /// </summary>
    /// <returns></returns>
    public bool IsRunning()
    {
        return doubleTaps >= 2;
    }
    /// <summary>
    /// Is the incoming Unit facing this Unit?
    /// </summary>
    /// <param name="incomingUnit"></param>
    public bool FacingUnit(UnitMove incomingUnit)
    {
        return Mathf.Sign(transform.localScale.x) == (-Mathf.Sign(incomingUnit.transform.localScale.x));
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
    private void DoubleTap(byte directionalByte)
    {
        if ((directionalByte == 5) || (directionalByte == 0) || (directionalByte > 9))
        {
            if (doubleTaps >= 2)
            {
                doubleTaps = 0;
                doubleTapTimer = 0f;
            }
            return;
        }
        if (doubleTaps == 0)
        {
            firstDirectionTapped = directionalByte;
            doubleTaps++;
            doubleTapTimer = 0.2f;
        }
        else
        {
            if (firstDirectionTapped != lastDirectionTapped)
            {
                if (SameBasicDirection(directionalByte))
                {
                    if (doubleTaps == 1)
                    {
                        doubleTaps++;
                    }
                }
                else
                {
                    doubleTaps = 0;
                    firstDirectionTapped = 0;
                }
            }
        }
    }
    /// <summary>
    /// Check if the same basic direction has been pressed. If strictly up or down, return false.
    /// </summary>
    /// <param name="directionalByte"></param>
    /// <returns></returns>
    private bool SameBasicDirection(byte directionalByte)
    {
        if ((directionalByte == 0) || (directionalByte == 2) || (directionalByte == 5)
            || (directionalByte == 8) || (directionalByte > 9))
        {
            return false;
        }
        //If stick pressed right (3, 6, or 9), count it
        if (directionalByte % 3 == 0)
        {
            return (firstDirectionTapped % 3) == 0;
        }
        //If stick pressed left (1, 4, or 7), count it
        return (firstDirectionTapped % 3) == 1;
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

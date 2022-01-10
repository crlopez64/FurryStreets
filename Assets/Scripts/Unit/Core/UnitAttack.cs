using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of Unit attacking.
/// </summary>
public class UnitAttack : MonoBehaviour
{
    private List<Collider2D> hitsRecorded;
    /*
     * Base class for attacking
     * ? How to do seduce while still alive?
     * Player Attack -> Can chain attacks
     * Enemy Attack -> Single attacks with maybe long animations
     * 
     */
    protected UnitStats unitStats;
    protected BoxCollider2D physicalCollider;
    protected Hitbox groundedHitbox;
    protected Hitbox airborneHitbox;
    protected UnitMove unitMove;
    protected UnitMove grabbedUnit;
    protected Attack[] airbornAttacks; //TODO: Add for Punch, Kick, and Special
    protected List<Attack> specialAttacks; //TODO: Neutral, Forward, Down, Up
    protected Attack rootAttack;
    /// <summary>
    /// The current attack to animate.
    /// </summary>
    protected Attack attackToAnimate;
    protected ParticlePooler particlePooler;
    protected UnitAnimationLayers unitAnimationLayers;
    protected Animator animator;
    protected bool isHit; //Use for UnitMove purposes
    protected bool attacking;
    protected bool stunArmor;
    protected bool attackStance; //If true, being idle or walking will have a different stance. Only visual.
    protected byte grabbingEnemyType; //0: Not grabbing, 1: Physical, 2: Escape
    protected byte grabbedByType; //0: Free, 1: Physical, 2: Escape
    /// <summary>
    /// The type of animation to play when hit. 0 = inactive, 1 = Minor, 2 = Major, 3 = Knockback Aerial, 4 = Knockback Distal
    /// </summary>
    protected byte hitType;
    /// <summary>
    /// If KB, record this hit type when hit by normal moves.
    /// </summary>
    protected byte hitTypeRecord;
    protected float stunTimer;

    public Transform unitGrabPosition;
    public TextAsset textMoveList;
    public LayerMask whoCanHit;

    protected virtual void Awake()
    {
        hitsRecorded = new List<Collider2D>(5);
        unitAnimationLayers = GetComponent<UnitAnimationLayers>();
        animator = GetComponent<Animator>();
        unitMove = GetComponent<UnitMove>();
        unitStats = GetComponent<UnitStats>();
        particlePooler = GetComponentInChildren<ParticlePooler>();
        physicalCollider = GetComponent<BoxCollider2D>();
        specialAttacks = new List<Attack>();
        Hitbox[] hitboxes = GetComponentsInChildren<Hitbox>();
        foreach (Hitbox hitbox in hitboxes)
        {
            if (hitbox.ForAirborne())
            {
                airborneHitbox = hitbox;
            }
            else
            {
                groundedHitbox = hitbox;
            }
        }
    }
    protected virtual void Start()
    {
        attackStance = true;
        attackToAnimate = null;
    }
    protected virtual void Update()
    {
        //Stun Things and Timer
        if ((!stunArmor) && (stunTimer > 0f))
        {
            stunTimer -= Time.deltaTime;
        }
        if ((stunTimer <= 0f) && unitStats.Stunned())
        {
            //Reset stun
            unitMove.CanMove(true);
            StopStun();
            unitAnimationLayers.SetMovementLayer();
        }

        //Animator
        animator.SetBool("IsHit", isHit);
        animator.SetBool("Stunned", Stunned());
        animator.SetBool("Attacking", attacking);
        animator.SetBool("AttackStance", attackStance);
        animator.SetBool("StayDowned", unitStats.StaminaEmpty());
        animator.SetInteger("HitType", hitType);
        animator.SetInteger("Grabbed", grabbedByType);
        animator.SetInteger("GrabbingEnemy", grabbingEnemyType);
        animator.SetInteger("AttackAnim", (attackToAnimate == null) ? 0 : attackToAnimate.GetAnimationID());
    }
    public void OnDrawGizmos()
    {
        if (attackToAnimate != null)
        {
            if (attackToAnimate != rootAttack)
            {
                Gizmos.color = new Color(0, 1, 0, 0.5f);
                Gizmos.DrawCube(groundedHitbox.transform.position, attackToAnimate.GetHitboxDimensions());
                Gizmos.color = new Color(0, 1, 0.5f, 0.5f);
                Gizmos.DrawCube(airborneHitbox.transform.position, new Vector2(2f, 1.5f));
            }
            else
            {
                if (groundedHitbox != null)
                {
                    Gizmos.DrawCube(groundedHitbox.transform.position, Vector3.zero);
                }
                if (airborneHitbox != null)
                {
                    Gizmos.DrawCube(airborneHitbox.transform.position, Vector3.zero);
                }
            }
        }
        
    }

    /// <summary>
    /// Set if the Unit is prepping to attack or not.
    /// </summary>
    /// <param name="tOrF"></param>
    public void SetAttackStance(bool tOrF)
    {
        attackStance = tOrF;
    }
    /// <summary>
    /// Set if the Unit is now attacking.
    /// </summary>
    public void NowAttacking()
    {
        attacking = true;
    }
    /// <summary>
    /// Animation callback layer at the beginning of a hit animation.
    /// </summary>
    public void TurnOffIsHit()
    {
        isHit = false;
    }
    /// <summary>
    /// Animation callback layer.
    /// </summary>
    public void TurnOffHitLayer()
    {
        isHit = false;
        hitType = 0;
        grabbedByType = 0;
        grabbingEnemyType = 0;
        if (stunArmor)
        {
            SetStunArmor(false);
            unitMove.CanMove(false);
            unitMove.StopMoving();
            unitAnimationLayers.SetStunnedLayer();
        }
        else if (unitStats.StaminaEmpty())
        {
            unitMove.CanMove(false);
            unitMove.StopMoving();
            unitAnimationLayers.SetHitLayer();
        }
        else
        {
            unitMove.CanMove(true);
            unitAnimationLayers.SetMovementLayer();
        }
    }
    /// <summary>
    /// Turn on Critical Stun for this Unit.
    /// </summary>
    public void SetStun()
    {
        stunArmor = true;
        stunTimer = 5f;
    }
    /// <summary>
    /// Set the Stun Armor for this Unit.
    /// </summary>
    /// <param name="tOrF"></param>
    public void SetStunArmor(bool tOrF)
    {
        stunArmor = tOrF;
    }
    /// <summary>
    /// Turn on or off the physical collider of this Unit.
    /// </summary>
    /// <param name="tOrF"></param>
    public void SetPhysicalCollider(bool tOrF)
    {
        physicalCollider.enabled = tOrF;
    }
    /// <summary>
    /// Set that the Unit has been grabbed.
    /// </summary>
    public void Grabbed()
    {
        grabbedByType = 1;
        if (Stunned())
        {
            StopStun();
        }
    }
    /// <summary>
    /// Get released from grab.
    /// </summary>
    public void GrabReleased()
    {
        grabbedByType = 0;
    }
    /// <summary>
    /// Set that the Unit has escaped from the grab.
    /// </summary>
    public void GrabEscape()
    {
        grabbedByType = 3;
    }
    /// <summary>
    /// Make an active frame activating the hitbox.
    /// </summary>
    public void ActiveFrame()
    {
        hitsRecorded.Clear();
        Collider2D[] hits = Physics2D.OverlapBoxAll(groundedHitbox.transform.position, attackToAnimate.GetHitboxDimensions(), 0f, whoCanHit);
        foreach(Collider2D hit in hits)
        {
            if (hit.GetComponentInParent<UnitAttack>() != null)
            {
                //If self, ignore
                if (hit.GetComponentInParent<UnitAttack>() == this)
                {
                    continue;
                }
                if (!hit.GetComponentInParent<UnitAttack>().StunArmor())
                {
                    if ((hitsRecorded.Count == 0) || (!hitsRecorded.Contains(hit)))
                    {
                        hitsRecorded.Add(hit);
                    }
                }
            }
        }
        Collider2D[] otherHits = Physics2D.OverlapBoxAll(airborneHitbox.transform.position, new Vector2(2f, 1.5f), 0f, whoCanHit);
        foreach(Collider2D hit in otherHits)
        {
            if (hit.GetComponentInParent<UnitAttack>() != null)
            {
                //If unit is grounded, ignore
                if (hit.GetComponentInParent<UnitMove>().Grounded())
                {
                    continue;
                }
                //If self, ignore
                if (hit.GetComponentInParent<UnitAttack>() == this)
                {
                    continue;
                }
                if (!hit.GetComponentInParent<UnitAttack>().StunArmor())
                {
                    if ((!hit.GetComponentInParent<UnitMove>().Grounded()) && (!hit.GetComponentInParent<UnitMove>().FlyingUp()))
                    {
                        if ((hitsRecorded.Count == 0) || (!hitsRecorded.Contains(hit)))
                        {
                            hitsRecorded.Add(hit);
                        }
                    }
                }
            }
        }
        foreach(Collider2D hit in hitsRecorded)
        {
            if (hit.GetComponentInParent<UnitMove>().Grounded() ||
                ((transform.position.y < hit.transform.position.y) && (Mathf.Abs(transform.position.y - hit.transform.position.y) <= 5f) &&
                (!hit.GetComponentInParent<UnitMove>().Grounded())))
            {
                hit.GetComponentInParent<UnitAttack>().TakeHit(transform, attackToAnimate);
                //If an enemy, add combo counter to player
                //Else if a player, reset their combo
                if (hit.GetComponentInParent<EnemyAttack>() != null)
                {
                    if (unitStats.GetComponent<PlayerStats>() != null)
                    {
                        unitStats.GetComponent<PlayerStats>().AddToCombo();
                    }
                }
                else if (hit.GetComponentInParent<PlayerAttack>() != null)
                {
                    if (hit.GetComponent<PlayerStats>() != null)
                    {
                        hit.GetComponent<PlayerStats>().ResetComboHit();
                    }
                }
            }
            if (hit.GetComponentInParent<UnitAttack>() != null)
            {
                Debug.LogWarning("NOTE: This item does not have a Unit Attack! " + hit.name);
                hit.GetComponentInParent<UnitAttack>().ResetAttacking();
            }
            hit.GetComponentInParent<UnitAnimationLayers>().SetHitLayer();
            if (particlePooler != null)
            {
                particlePooler.SpawnParticle(0, (Vector2)groundedHitbox.transform.position + Random.insideUnitCircle * 0.5f);
            }
        }
    }
    /// <summary>
    /// Get the closest enemy within grab range.
    /// </summary>
    public void ActiveFrameGrab()
    {
        //Debug.Log("Attempt to grab");
        Collider2D[] hits = Physics2D.OverlapBoxAll(groundedHitbox.transform.position, attackToAnimate.GetHitboxDimensions(), 0f, whoCanHit);
        Collider2D closest = null;
        foreach (Collider2D hit in hits)
        {
            //if self, ignore
            if (hit.GetComponentInParent<PlayerAttack>() == this)
            {
                continue;
            }
            if (closest == null)
            {
                closest = hit;
                continue;
            }
            if (Vector2.Distance(transform.position, hit.transform.position) < Vector2.Distance(transform.position, hit.transform.position))
            {
                closest = hit;
                continue;
            }
        }
        if (closest != null)
        {
            if (closest.GetComponentInParent<UnitAttack>().Stunned())
            {
                Grab(closest);
            }
            else
            {
                //If not stunned, check for chance of getting grabbed success
                if (closest.GetComponentInParent<UnitStats>() != null)
                {
                    if (closest.GetComponentInParent<UnitStats>().CanGrab())
                    {
                        Grab(closest);
                    }
                    else
                    {
                        GrabFailed(closest);
                    }
                }
                else
                {
                    Debug.LogWarning("Does not have Unit Stats!!");
                    GrabFailed(closest);
                }
            }
        }
    }
    /// <summary>
    /// Spawn a special move particle.
    /// </summary>
    /// <param name="whichSpecial"></param>
    public void SpawnParticleSpecial(int whichSpecial)
    {
        particlePooler.SpawnParticleSpecial((byte)whichSpecial, particlePooler.transform.position, transform.localScale.x);
    }
    /// <summary>
    /// Have the Unit escape from the grab position.
    /// </summary>
    public void EnemyEscapeGrab()
    {
        grabbedUnit.CanMove(true);
        grabbedUnit.GetComponent<UnitAttack>().SetPhysicalCollider(true);
        grabbedUnit.transform.parent = transform.parent;
        grabbedUnit = null;
        grabbingEnemyType = 3;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
    /// <summary>
    /// Stop the Grab failed pushback animation.
    /// </summary>
    public void GrabFailedEndAnim()
    {
        attacking = false;
        unitMove.StopMoving();
        unitMove.CanFlip(true);
        unitMove.CanMove(true);
        grabbingEnemyType = 0;
        unitAnimationLayers.SetMovementLayer();
    }
    /// <summary>
    /// Show that this Unit has escaped from a grab.
    /// </summary>
    public void EscapeGrab()
    {
        grabbedByType = 3;
        unitAnimationLayers.SetHitLayer();
    }
    /// <summary>
    /// Cancel the grabbed by animation.
    /// </summary>
    public void CancelGrabbedBy()
    {
        unitMove.CanMove(true);
        SetPhysicalCollider(true);
        grabbedByType = 0;
        grabbingEnemyType = 0;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        unitAnimationLayers.SetMovementLayer();
    }
    /// <summary>
    /// Flip the Grabbed Enemy's sprite.
    /// </summary>
    public void FlipGrabbedEnemy()
    {
        grabbedUnit.FlipSprite();
    }
    /// <summary>
    /// Move the enemy to the specified position.
    /// </summary>
    public void MoveGrabbedEnemy()
    {
        grabbedUnit.transform.position = unitGrabPosition.position;
    }
    /// <summary>
    /// Move the enemy to the specified position as well as flip their sprite.
    /// </summary>
    public void MoveGrabbedEnemyFlip()
    {
        grabbedUnit.FlipSprite();
        grabbedUnit.transform.position = unitGrabPosition.position;
    }
    /// <summary>
    /// Throw a grabbed Enemy.
    /// </summary>
    public void ThrowGrabbedEnemy()
    {
        Debug.Log("Throw enemy");
        FlipGrabbedEnemy();
        grabbedUnit.transform.parent = transform.parent;
        grabbedUnit.GetComponent<UnitAttack>().SetPhysicalCollider(true);
        grabbedUnit.GetComponent<UnitAttack>().GrabReleased();
        grabbedUnit.GetComponent<UnitAttack>().TakeHit(transform, attackToAnimate);
        grabbingEnemyType = 0;
        grabbedUnit = null;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
    /// <summary>
    /// Take some incoming damage. If too much stun, stun the unit.
    /// </summary>
    public void TakeHit(Transform attackerPosition, Attack attack)
    {
        isHit = true;
        attacking = false;
        hitTypeRecord = (hitType >= 3) ? hitType : (byte)4;
        hitType = (!unitMove.Grounded()) ? hitTypeRecord : attack.GetHitType();
        if (Stunned())
        {
            StopStun();
            unitStats.ResetStun();
        }
        if (unitStats == null)
        {
            Debug.LogWarning("Warning: No Unit Stats detected.");
        }
        else
        {
            if (unitStats.TakeDamage(attack, GetComponent<UnitStats>()))
            {
                Debug.LogWarning("Stunned!!");
                SetStun();
                Vector2 newVelocity = attack.GetKnockback();
                newVelocity.x = attack.GetKnockback().x + ((hitType <= 2) ? 20f : 5f);
                unitMove.Knockback(attackerPosition.position, newVelocity, hitType);
            }
            else
            {
                unitMove.Knockback(attackerPosition.position, attack.GetKnockback(), hitType);
            }
            if (unitStats.StaminaEmpty())
            {
                animator.SetTrigger("Defeated");
                unitStats.ResetStun();
                if (GetComponent<EnemyAttack>() != null)
                {
                    GetComponent<EnemyAttack>().SetDespawnTimer();
                }
            }
        }
        unitMove.CanMove(false);
    }
    /// <summary>
    /// Stop recording the Hit Type Record.
    /// </summary>
    public void ResetHitTypeRecord()
    {
        hitTypeRecord = 0;
    }
    /// <summary>
    /// Stop the Unit's stun.
    /// </summary>
    public void StopStun()
    {
        stunArmor = false;
        stunTimer = 0f;
        unitStats.ResetStun();
    }
    /// <summary>
    /// Is the Unit currently attacking?
    /// </summary>
    /// <returns></returns>
    public bool CurrentlyAttacking()
    {
        return attacking;
    }
    /// <summary>
    /// Is the Unit grabbing someone?
    /// </summary>
    /// <returns></returns>
    public bool CurrentlyGrabbing()
    {
        return grabbingEnemyType > 0;
    }
    /// <summary>
    /// Has the Unit been attacked?
    /// </summary>
    /// <returns></returns>
    public bool IsAttacked()
    {
        return hitType > 0;
    }
    /// <summary>
    /// Does this Unit have Stun Armor?
    /// </summary>
    /// <returns></returns>
    public bool StunArmor()
    {
        return stunArmor;
    }
    /// <summary>
    /// Is the Unit stunned?
    /// </summary>
    /// <returns></returns>
    public bool Stunned()
    {
        return stunTimer > 0f;
    }
    /// <summary>
    /// Get the current attack animating.
    /// </summary>
    /// <returns></returns>
    public Attack GetAttackToAnimate()
    {
        return attackToAnimate;
    }
    /// <summary>
    /// Return the Root Attack for ground attacks.
    /// </summary>
    /// <returns></returns>
    public Attack RootAttack()
    {
        return rootAttack;
    }
    /// <summary>
    /// Reset all attacking variables to allow to attack from the beginning again.
    /// </summary>
    public virtual void ResetAttacking()
    {
        //Empty
    }
    /// <summary>
    /// Play the next attack animation.
    /// </summary>
    public virtual void PlayNextAttack()
    {
        //Empty
    }
    /// <summary>
    /// Create the Unit's attacks.
    /// </summary>
    protected virtual void CreateAttacks()
    {
        //Empty
    }
    /// <summary>
    /// Create the Unit's movelist.
    /// </summary>
    protected virtual void SetUpMoveList()
    {
        //Empty
    }

    /// <summary>
    /// Grab the Enemy
    /// </summary>
    /// <param name="closest"></param>
    protected void Grab(Collider2D closest)
    {
        //Debug.Log("Grabbed someone! " + closest.name);
        if (GetComponent<PlayerAttack>() != null)
        {
            GetComponent<PlayerAttack>().SetGrabTimer(closest.GetComponentInParent<UnitStats>().GetGrabTimer());
        }
        grabbingEnemyType = 1;
        grabbedUnit = closest.GetComponentInParent<UnitMove>();
        if (Mathf.Sign(transform.localScale.x) == Mathf.Sign(grabbedUnit.transform.localScale.x))
        {
            grabbedUnit.FlipSprite();
        }
        grabbedUnit.GetComponent<UnitStats>().ResetStun();
        grabbedUnit.GetComponent<UnitAttack>().SetStunArmor(false);
        grabbedUnit.GetComponentInParent<UnitAttack>().StopStun();
        grabbedUnit.GetComponent<UnitAttack>().Grabbed();
        grabbedUnit.GetComponent<UnitAttack>().SetPhysicalCollider(false);
        grabbedUnit.GetComponentInParent<UnitAnimationLayers>().SetHitLayer();
        grabbedUnit.CanMove(false);
        grabbedUnit.transform.parent = transform;
        grabbedUnit.transform.position = unitGrabPosition.position;
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
    /// <summary>
    /// Have the grabbed unit cancel out the grab.
    /// </summary>
    /// <param name="closest"></param>
    protected void GrabFailed(Collider2D closest)
    {
        Debug.Log("Grab failed");
        grabbedUnit = closest.gameObject.GetComponentInParent<UnitMove>();
        grabbedUnit.GetComponent<UnitAttack>().EscapeGrab();
        grabbedUnit = null;
        grabbingEnemyType = 3;
        unitMove.CanFlip(false);
        unitAnimationLayers.SetHitLayer();
        unitMove.Knockback(closest.transform.position, new Vector2(7.0f, 0), 1);
    }
    /// <summary>
    /// Have the grabbed unit cancel out of the grab.
    /// </summary>
    protected void GrabFailed()
    {
        Debug.Log("Grab failed after holding too long.");
        if (grabbedUnit == null)
        {
            Debug.LogWarning("Did not find grabbed unit.");
            return;
        }
        grabbedUnit.GetComponent<UnitAttack>().EscapeGrab();
        grabbedUnit.CanMove(true);
        grabbedUnit.GetComponent<UnitAttack>().SetPhysicalCollider(true);
        grabbedUnit.transform.parent = transform.parent;
        grabbedUnit = null;
        grabbingEnemyType = 3;
        unitMove.CanFlip(false);
        unitAnimationLayers.SetHitLayer();
        unitMove.Knockback(grabbedUnit.transform.position, new Vector2(7.0f, 0), 1);
    }
}

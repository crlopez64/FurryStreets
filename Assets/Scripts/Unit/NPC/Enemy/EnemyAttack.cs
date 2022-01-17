using UnityEngine;

/// <summary>
/// Script in charge of the Enemy's Attacks.
/// </summary>
public class EnemyAttack : UnitAttack
{
    private float despawnTimer;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        CreateAttacks();
        attackToAnimate = rootAttack;
        despawnTimer = 0;
    }
    protected override void Update()
    {
        base.Update();
        //Turn off Enemy gameobject after a set amount of time.
        if (unitStats.StaminaEmpty())
        {
            if (despawnTimer > 0)
            {
                despawnTimer -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Make the Enemy attack.
    /// </summary>
    public void MakeAttack(byte attackOption)
    {
        //BASE CASE: If hit, do not buffer in attacks
        if (IsAttacked())
        {
            return;
        }
        Debug.Log("Enemy attacking with: Option " + attackOption + "!");
        attacking = true;
        attackToAnimate = attackToAnimate.GetNextAttack(attackOption);
        unitAnimationLayers.SetAttackLayer();
        unitMove.StopMoving();
    }
    /// <summary>
    /// Set the despawn timer.
    /// </summary>
    public void SetDespawnTimer()
    {
        despawnTimer = 5f;
    }
    /// <summary>
    /// Reset all attacking variables to allow to attack from the beginning again.
    /// </summary>
    public override void ResetAttacking()
    {
        attacking = false;
        attackToAnimate = rootAttack;
    }
    /// <summary>
    /// Within a self-contained attack that has multiple hits, play the next attack.
    /// </summary>
    public override void PlayNextAttack()
    {
        if (attackToAnimate.HasOptions())
        {
            attackToAnimate = attackToAnimate.GetNextAttack(0);
        }
        else
        {
            unitAnimationLayers.SetMovementLayer();
            ResetAttacking();
        }
    }
    /// <summary>
    /// Create the Enemy's attack
    /// </summary>
    protected override void CreateAttacks()
    {
        //Use a similar scheme as the Player txt move list
        //For every line, each chain is self contained in the attack
        rootAttack = new Attack("Starting Null", 5, "noAttack", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null);
        if (textMoveList == null)
        {
            Debug.LogWarning("NOTE: Unit does not have a MoveList to reference.");
            return;
        }
        SetUpMoveList();
        StopAddingAttacks(rootAttack);
    }
    /// <summary>
    /// Create the Enemy's movelist.
    /// </summary>
    protected override void SetUpMoveList()
    {
        string[] lines = textMoveList.text.Split('\n');
        foreach (string line in lines)
        {
            Attack currentAttackInString = rootAttack;
            string[] linePrep = line.Split('=');
            string[] attackString = linePrep[1].Split(';');
            for (int i = 0; i < attackString.Length; i++)
            {
                bool isFinalUniqueAttack = i == attackString.Length - 1;
                Attack newAttack = InitializeAttack(linePrep[0], attackString[i], isFinalUniqueAttack);
                currentAttackInString.AddAttack(newAttack);
                if (i < (attackString.Length - 1))
                {
                    currentAttackInString = newAttack;
                }
            }
        }
    }
    /// <summary>
    /// Stop adding attacks for all the moves in the list.
    /// </summary>
    /// <param name="currentAttack"></param>
    private void StopAddingAttacks(Attack currentAttack)
    {
        if (currentAttack.HasOptions())
        {
            foreach (Attack attack in currentAttack.GetNextAttacks())
            {
                StopAddingAttacks(attack);
            }
        }
        currentAttack.StopAddingAttacks();
    }
}

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
        Debug.Log("Enemy attacking with option: " + attackOption + "!");
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
        rootAttack = new Attack("Starting Null", false, false, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false);
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
        //Debug.Log("Creating the movelist...");
        string[] lines = textMoveList.text.Split('\n');
        foreach (string line in lines)
        {
            Attack currentAttackInString = rootAttack;
            string[] linePrep = line.Split('=');
            //Branch the attack strings from the root
            string[] attackString = linePrep[1].Split(';');

            //Debug.Log("Count: " + attackString.Length);
            for (int i = 0; i < attackString.Length; i++)
            {
                string[] attackData = attackString[i].Split(',');
                bool isFinalUniqueAttack = i == attackString.Length - 1;
                Attack newAttack = new Attack(linePrep[0], bool.Parse(attackData[0]), bool.Parse(attackData[1]),
                    byte.Parse(attackData[2]), byte.Parse(attackData[3]),
                    int.Parse(attackData[4]), int.Parse(attackData[5]), int.Parse(attackData[6]),
                    float.Parse(attackData[7]), float.Parse(attackData[8]), byte.Parse(attackData[9]),
                    float.Parse(attackData[10]), float.Parse(attackData[11]), float.Parse(attackData[12]), float.Parse(attackData[13]),
                    isFinalUniqueAttack);
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

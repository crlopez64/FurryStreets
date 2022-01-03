using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of Player attacking.
/// </summary>
public class PlayerAttack : UnitAttack
{
    //TODO: Cancel normal moves to Special moves
    private Queue<int> attacksBuffered;  //The next attacks buffered in.
    private Attack attackTree;      //The entire movelist while on the ground.
    private Attack attackToBuffer;  //The current attack for the attack buffer.
    private bool canPlayNextAttack;
    private bool currentlyPlayingSpecial; //If true, cannot cancel Special into another Special.
    private float grabTimer;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        attacksBuffered = new Queue<int>(5);
        CreateAttacks();
        grabTimer = 0f;
        attackToAnimate = rootAttack;
    }
    protected override void Update()
    {
        base.Update();
        if (canPlayNextAttack)
        {
            if (HasAttacksBuffered())
            {
                PlayNextAttack();
            }
        }
        if (grabTimer > 0)
        {
            grabTimer -= Time.deltaTime;
        }
        else
        {
            if (grabbedUnit != null)
            {
                GrabFailed();
            }
        }
    }
    /// <summary>
    /// Reset all attacking variables to allow to attack from the beginning again.
    /// </summary>
    public override void ResetAttacking()
    {
        attacking = false;
        currentlyPlayingSpecial = false;
        attackToBuffer = rootAttack;
        attackToAnimate = rootAttack;
        attacksBuffered.Clear();
    }
    /// <summary>
    /// Make the player attack.
    /// </summary>
    /// <param name="directionalInput"></param>
    /// <param name="attackInput"></param>
    public void MakeAttack(byte directionalInput, byte attackInput)
    {
        //BASE CASE: If attack input includes Special, consider this first
        if (((attackInput & 0x4) >> 2) == 0x1)
        {
            Debug.Log("Pressed Special move.");
            currentlyPlayingSpecial = true;
        }

        //BASE CASE: If hit or on an ender attack, and does not have Special Attack Input, do not buffer in attacks
        if (IsAttacked() ||
            ((!CurrentlyGrabbing()) && (!attackToBuffer.HasOptions())))
        {
            return;
        }

        //BASE CASE: If was grabbing, only consider attack input.
        if (CurrentlyGrabbing())
        {
            for(int i = 0; i < attackToBuffer.GetNextAttacks().Count; i++)
            {
                if (attackToBuffer.GetNextAttack(i).RequiredAttack() == attackInput)
                {
                    attackToAnimate = attackToAnimate.GetNextAttack(i);
                    attackToBuffer = attackToBuffer.GetNextAttack(i);
                    unitMove.StopMoving();
                    return;
                }
            }
            //Debug.Log("Did not find attack...");
            return;
        }

        //Check if current attack is valid to buffer and animate
        //Check if directional input AND attack input matches
        //If not, redo the check without the directional input
        List<Attack> nextInString = attackToBuffer.GetNextAttacks();
        for (int i = 0; i < 2; i++)
        {
            //Base Case: If the initial check found nothing, remove the directional input entirely
            if (i == 1)
            {
                //Debug.Log("Was incorrect. Check again with Neutral");
                directionalInput = 5;
            }
            for(int j = 0; j < nextInString.Count; j++)
            {
                if (nextInString[j].RequiredDirection() != directionalInput)
                {
                    continue;
                }
                if (nextInString[j].RequiredAttack() == attackInput)
                {
                    //Base Case: If not enough meter, cannot use the move.
                    if (nextInString[j].HasMeterCost())
                    {
                        if (!nextInString[j].CanUseMove(unitStats.CurrentMeter()))
                        {
                            Debug.Log("Do not have enough meter to use!");
                            continue;
                        }
                    }
                    if (attacking)
                    {
                        //If already attacking, buffer the attack into the queue
                        //Debug.Log("Correct attack: Enqueuing");
                        attacksBuffered.Enqueue(j);
                        attackToBuffer = attackToBuffer.GetNextAttack(j);
                    }
                    else
                    {
                        //If not, play the attack immediately
                        //Debug.Log("Correct attack: Initial attack");
                        attacking = true;
                        attackToAnimate = attackToAnimate.GetNextAttack(j);
                        attackToBuffer = attackToBuffer.GetNextAttack(j);
                        unitAnimationLayers.SetAttackLayer();
                        unitMove.StopMoving();
                    }
                    return;
                }
            }
        }
        //Debug.Log("Incorrect attack");
    }
    public void PlayNextAttackAnimator()
    {
        canPlayNextAttack = true;
    }
    public void BackToIdle()
    {
        canPlayNextAttack = false;
        unitAnimationLayers.SetMovementLayer();
        ResetAttacking();
    }
    /// <summary>
    /// Play the next attack animation.
    /// </summary>
    public override void PlayNextAttack()
    {
        if (!IsAttacked())
        {
            if (attacksBuffered.Count > 0) 
            {
                canPlayNextAttack = false;
                attackToAnimate = attackToAnimate.GetNextAttack(attacksBuffered.Dequeue());
                //Debug.LogWarning("AttackID: " + attackToAnimate.GetAnimationID());
            }
            else
            {
                canPlayNextAttack = false;
                unitAnimationLayers.SetMovementLayer();
                ResetAttacking();
            }
        }
    }
    /// <summary>
    /// Clear out the Player's attacks.
    /// </summary>
    public void ClearBufferedAttacks()
    {
        attacksBuffered.Clear();
    }
    /// <summary>
    /// If grabbing an enemy, set grab timer.
    /// </summary>
    /// <param name="newTimer"></param>
    public void SetGrabTimer(float newTimer)
    {
        grabTimer = newTimer;
    }
    /// <summary>
    /// Zero out the Grab timer.
    /// </summary>
    public void ZeroGrabTimer()
    {
        grabTimer = 0f;
    }
    /// <summary>
    /// Does the Player have anyh= more attacks to play out?
    /// </summary>
    /// <returns></returns>
    public bool HasAttacksBuffered()
    {
        return attacksBuffered.Count > 0;
    }
    public Attack GetRootAttack()
    {
        return rootAttack;
    }
    /// <summary>
    /// Create the Player's basic Attack Tree.
    /// </summary>
    protected override void CreateAttacks()
    {
        attackTree = new Attack("Starting Null", false, false, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false);
        rootAttack = attackTree;
        attackToBuffer = attackTree;
        if (textMoveList == null)
        {
            Debug.LogWarning("NOTE: Unit does not have a MoveList to reference.");
            return;
        }
        SetUpMoveList();
        StopAddingAttacks(rootAttack);
    }
    /// <summary>
    /// Create the Player's movelist.
    /// </summary>
    protected override void SetUpMoveList()
    {
        string[] lines = textMoveList.text.Split('\n');
        foreach(string line in lines)
        {
            Attack currentAttackInString = rootAttack;
            string[] linePrep = line.Split('=');
            string[] lineBranching = linePrep[0].Split('.');
            
            if (lineBranching.Length == 1)
            {
                //Branch the attack strings from the root
                string[] attackString = linePrep[1].Split(';');

                //Debug.Log("Count: " + attackString.Length);
                for(int i = 0; i < attackString.Length; i++)
                {
                    string moveName = (i != attackString.Length - 1) ? linePrep[0] + " Partial" : linePrep[0];
                    string[] attackData = attackString[i].Split(',');
                    bool isFinalUniqueAttack = i == attackString.Length - 1;
                    Attack newAttack = new Attack(moveName, bool.Parse(attackData[0]), bool.Parse(attackData[1]),
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
            else
            {
                //Split from the next n attacks of the last known attack string branching from the root
                //1. (from a 3 Attack String): Deviate the original route from the first attack (P,P,P to P,K,-)
                //2. (from a 3 Attack String): Deviate the original route from the second attack (P,P,P to P,P,K)
                Attack whereToDeviate = rootAttack.GetNextAttack(rootAttack.GetNextAttacks().Count - 1);
                for(int i = 0; i < int.Parse(lineBranching[0]); i++)
                {
                    whereToDeviate = whereToDeviate.GetNextAttack(0);
                }
                string[] attackString = linePrep[1].Split(';');
                for (int i = 0; i < attackString.Length; i++)
                {
                    string moveName = (i != attackString.Length - 1) ? lineBranching[1] + " Partial" : lineBranching[1];
                    string[] attackData = attackString[i].Split(',');
                    bool isFinalUniqueAttack = i == attackString.Length - 1;
                    Attack newAttack = new Attack(moveName, bool.Parse(attackData[0]), bool.Parse(attackData[1]),
                        byte.Parse(attackData[2]), byte.Parse(attackData[3]),
                        int.Parse(attackData[4]), int.Parse(attackData[5]), int.Parse(attackData[6]),
                        float.Parse(attackData[7]), float.Parse(attackData[8]), byte.Parse(attackData[9]),
                        float.Parse(attackData[10]), float.Parse(attackData[11]), float.Parse(attackData[12]), float.Parse(attackData[13]),
                        isFinalUniqueAttack);
                    whereToDeviate.AddAttack(newAttack);
                    if (i == 0)
                    {
                        whereToDeviate = newAttack;
                    }
                }
            }
        }
        //Debug.Log("Move List created.");
    }
    /// <summary>
    /// Stop adding attacks for all the moves in the list.
    /// </summary>
    /// <param name="currentAttack"></param>
    private void StopAddingAttacks(Attack currentAttack)
    {
        if (currentAttack.HasOptions())
        {
            foreach(Attack attack in currentAttack.GetNextAttacks())
            {
                StopAddingAttacks(attack);
            }
        }
        currentAttack.StopAddingAttacks();
    }
}

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
    private Attack specialBuffered; //The special attack buffered.
    private bool canPlayNextAttack;
    private bool playingSpecialMove;
    private float grabTimer;

    public bool specialMoveBuffered;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        attacksBuffered = new Queue<int>(5);
        CreateAttacks();
        grabTimer = 0f;
        attackToAnimate = rootAttack;
    }
    protected override void Update()
    {
        base.Update();
        specialMoveBuffered = (specialBuffered != null);
        if (canPlayNextAttack)
        {
            if (HasAttacksBuffered() || (specialBuffered != null))
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
        attackToBuffer = rootAttack;
        attackToAnimate = rootAttack;
        playingSpecialMove = false;
        specialBuffered = null;
        attacksBuffered.Clear();
    }
    /// <summary>
    /// Animator playing Special move.
    /// </summary>
    public void PlayingSpecialMove()
    {
        playingSpecialMove = true;
    }
    /// <summary>
    /// Make the player attack.
    /// </summary>
    /// <param name="directionalInput"></param>
    /// <param name="attackInput"></param>
    public void MakeAttack(byte directionalInput, byte attackInput)
    {
        //BASE CASE: If Special was buffered in, stop doing anything.
        if (specialBuffered != null)
        {
            return;
        }
        //BASE CASE: If attack input includes Special, consider this first. OK if last attack buffered was Ender.
        if (((attackInput & 0x4) >> 2) == 0x1)
        {
            if (specialAttacks.Count == 0)
            {
                Debug.LogError("ERROR: No Special moves!!");
                return;
            }
            if ((directionalInput == 3) || (directionalInput == 9))
            {
                directionalInput = 6;
            }
            if ((directionalInput == 1) || (directionalInput == 7))
            {
                directionalInput = 4;
            }
            switch (directionalInput)
            {
                case 2: //Down Special
                    if (specialAttacks[2].CanUseMove(unitStats.CurrentMeter()))
                    {
                        specialBuffered = specialAttacks[2];
                    }
                    else
                    {
                        unitStats.NotEnoughMeter();
                    }
                    break;
                case 4: //Forward Special
                    if (specialAttacks[1].CanUseMove(unitStats.CurrentMeter()))
                    {
                        specialBuffered = specialAttacks[1];
                    }
                    else
                    {
                        unitStats.NotEnoughMeter();
                    }
                    break;
                case 5: //Neutral Special
                    if (specialAttacks[0].CanUseMove(unitStats.CurrentMeter()))
                    {
                        specialBuffered = specialAttacks[0];
                    }
                    else
                    {
                        unitStats.NotEnoughMeter();
                    }
                    break;
                case 6: //Forward Special
                    if (specialAttacks[1].CanUseMove(unitStats.CurrentMeter()))
                    {
                        specialBuffered = specialAttacks[1];
                    }
                    else
                    {
                        unitStats.NotEnoughMeter();
                    }
                    break;
                case 8: //Up Special
                    if (specialAttacks[3].CanUseMove(unitStats.CurrentMeter()))
                    {
                        specialBuffered = specialAttacks[3];
                    }
                    else
                    {
                        unitStats.NotEnoughMeter();
                    }
                    break;
                default: //Neutral Special
                    if (specialAttacks[0].CanUseMove(unitStats.CurrentMeter()))
                    {
                        specialBuffered = specialAttacks[0];
                    }
                    else
                    {
                        unitStats.NotEnoughMeter();
                    }
                    break;
            }
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
                        SetAttackStance();
                        ResetHits();
                        attackToAnimate = attackToAnimate.GetNextAttack(j);
                        attackToBuffer = attackToBuffer.GetNextAttack(j);
                        if (attackToAnimate.HasMeterCost())
                        {
                            if (attackToAnimate.CanUseMove(unitStats.CurrentMeter()))
                            {
                                unitStats.MeterBurn(attackToAnimate.MeterCost());
                            }
                            else
                            {
                                unitStats.NotEnoughMeter();
                            }
                        }
                        //TODO: Move the Flipping to somewhere else?
                        if (attackToAnimate.AttributeFlip())
                        {
                            unitMove.FlipSprite();
                        }
                        unitAnimationLayers.SetAttackLayer();
                        unitMove.StopMoving();
                    }
                    //If attack enqueued was a Special, declare it.
                    if (attackToBuffer.RequiredAttack() == 4)
                    {
                        specialBuffered = attackToBuffer;
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
        ResetHits();
        if (!IsAttacked())
        {
            if (attacksBuffered.Count > 0) 
            {
                SetAttackStance();
                canPlayNextAttack = false;
                attackToAnimate = attackToAnimate.GetNextAttack(attacksBuffered.Dequeue());
                if (attackToAnimate.HasMeterCost())
                {
                    unitStats.MeterBurn(attackToAnimate.MeterCost());
                }
                //TODO: Move the Flipping to somewhere else?
                if (attackToAnimate.AttributeFlip())
                {
                    unitMove.FlipSprite();
                }
            }
            else
            {
                //If nothing else is buffered, check if a special move was buffered.
                if (specialBuffered != null)
                {
                    canPlayNextAttack = false;
                    attackToAnimate = specialBuffered;
                    if (attackToAnimate.HasMeterCost())
                    {
                        SetAttackStance();
                        unitStats.MeterBurn(attackToAnimate.MeterCost());
                    }
                    //TODO: Move the Flipping to somewhere else?
                    if (attackToAnimate.AttributeFlip())
                    {
                        unitMove.FlipSprite();
                    }
                    playingSpecialMove = true;
                }
                else
                {
                    canPlayNextAttack = false;
                    unitAnimationLayers.SetMovementLayer();
                    ResetAttacking();
                }
                
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
        attackTree = new Attack("Starting Null", 5, "noAttack", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, null);
        rootAttack = attackTree;
        attackToBuffer = attackTree;
        specialAttacks = new List<Attack>();
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

                for(int i = 0; i < attackString.Length; i++)
                {
                    string attackName = (i != attackString.Length - 1) ? linePrep[0] + " Partial" : linePrep[0];
                    bool isFinalUniqueAttack = i == attackString.Length - 1;
                    Attack newAttack = InitializeAttack(attackName, attackString[i], isFinalUniqueAttack);
                    currentAttackInString.AddAttack(newAttack);
                    if (i < (attackString.Length - 1))
                    {
                        currentAttackInString = newAttack;
                    }
                    //If a Special move, move it to its own list. It's fine if it branches from Root.
                    if (newAttack.RequiredAttack() == 4)
                    {
                        newAttack.StopAddingAttacks();
                        specialAttacks.Add(newAttack);
                    }
                }
            }
            else
            {
                //Split from the next n attacks of the last known attack string branching from the root
                //(Move); (Move); (Move)
                //*.(Move): Deviate from last attack, starting from Root.
                Attack whereToDeviate = rootAttack;
                //Find the pathway
                for (int i = 0; i < (lineBranching.Length - 1); i++)
                {
                    //If star, branch from the most recent attack in the list.
                    if (lineBranching[i][0] == '*')
                    {
                        whereToDeviate = whereToDeviate.GetNextAttack(whereToDeviate.Length() - 1);
                        continue;
                    }
                    if (int.TryParse(lineBranching[i], out int intParsed))
                    {
                        whereToDeviate = whereToDeviate.GetNextAttack(intParsed);
                    }
                }

                //Place in the attacks
                string[] attackString = linePrep[1].Split(';');
                for (int i = 0; i < attackString.Length; i++)
                {
                    string attackName = (i != attackString.Length - 1) ? lineBranching[lineBranching.Length - 1] + " Partial":
                        lineBranching[lineBranching.Length - 1];
                    bool isFinalUniqueAttack = i == attackString.Length - 1;
                    Attack newAttack = InitializeAttack(attackName, attackString[i], isFinalUniqueAttack);
                    whereToDeviate.AddAttack(newAttack);
                    if (i == 0)
                    {
                        whereToDeviate = newAttack;
                    }
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
            foreach(Attack attack in currentAttack.GetNextAttacks())
            {
                StopAddingAttacks(attack);
            }
        }
        currentAttack.StopAddingAttacks();
    }
}

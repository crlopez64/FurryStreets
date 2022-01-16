using UnityEngine;
using System.Collections;
using Panda;

/// <summary>
/// Script in charge of methods for valid AI commands.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    //TODO: Change "Player" to "CurrentFocus" or something
    private Vector3 vectorToFocus;
    private PlayerMove currentFocus;
    private EnemyMove enemyMove;
    private EnemyStats enemyStats;
    private EnemyAttack enemyAttack;
    private bool haveToDetour;
    private bool retreatFromFocus;
    private bool pauseOnHorizontal;
    private bool moveDiagonalCloseIn;
    private bool moveDiagonalRetreat;
    private byte detourPath; //For each bit, 0 = free, 1 = obstacle in the way
    private byte directionToMove;
    private float canHitTimer;
    private float pauseMoveTimer;
    private float redetermineHabits;
    private float redeterminePauseOnMove;

    public LayerMask whoToAttack;
    public LayerMask thingsInTheWay;
    [Range(0, 5)]
    public byte retreatOnGettingHit;
    [Range(0, 5)]
    public byte closeInHabits;
    [Range(0, 5)]
    public byte retreatHabits;
    [Range(1f, 10f)]
    public float idealDistanceFromPlayer;
    [Range(1.06f, 2f)]
    public float idealGrabDistanceFromPlayer;

    private void Awake()
    {
        currentFocus = FindObjectOfType<PlayerMove>();
        enemyStats = GetComponent<EnemyStats>();
        enemyMove = GetComponent<EnemyMove>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        //Timers
        if (redeterminePauseOnMove > 0f)
        {
            redeterminePauseOnMove -= Time.deltaTime;
        }
        if (pauseMoveTimer > 0f)
        {
            pauseMoveTimer -= Time.deltaTime;
        }
        if (canHitTimer > 0f)
        {
            canHitTimer -= Time.deltaTime;
        }
        if (redetermineHabits > 0f)
        {
            redetermineHabits -= Time.deltaTime;
        }

        //Character flip facing?
        //If not getting attacked, Flip sprite accordingly

        //Get Vector from enemy to player
        vectorToFocus = (currentFocus.transform.position - transform.position).normalized;
    }

    /// <summary>
    /// Check if Enemy will retreat from its Focus or not. If RetreatHabits = 0, Enemy will continue toward Focus; if =5, Enemy will always repel.
    /// </summary>
    public void RetreatPotentialFromAttack()
    {
        if (retreatOnGettingHit == 0)
        {
            retreatFromFocus = false;
        }
        else
        {
            retreatFromFocus = Random.Range(0, 100) <= (retreatOnGettingHit * 20);
        }
    }
    /// <summary>
    /// Force the Enemy to change its focus to the given Player.
    /// </summary>
    /// <param name="attackingPlayer"></param>
    public void ForceFocus(PlayerMove attackingPlayer)
    {
        currentFocus = attackingPlayer;
    }
    [Task]
    public void GetDirectRoute()
    {
        directionToMove = GetRadianDirection(GetVectorToFocus());
        Task.current.Complete(true);
    }
    [Task]
    public void FlipSprite()
    {
        enemyMove.FlipSprite();
        Task.current.Complete(true);
    }
    [Task]
    public void DeterminePauseMove()
    {
        //if (redeterminePauseOnMove <= 0f)
        //{
        //    if (Random.Range(0, 9) <= 7)
        //    {
        //        pauseMoveTimer = Random.Range(0.2f, 1f);
        //        redeterminePauseOnMove = pauseMoveTimer;
        //    }
        //    else
        //    {
        //        redeterminePauseOnMove = 3f;
        //    }
        //}
        Task.current.Complete(true);
    }
    [Task]
    public void StopMove()
    {
        directionToMove = 8;
        Move();
    }
    [Task]
    public void MoveHorizontalAxis()
    {
        if (retreatFromFocus)
        {
            directionToMove = (byte)(IsRightOfPlayer() ? 0 : 4);
        }
        else
        {
            directionToMove = (byte)(IsRightOfPlayer() ? 4 : 0);
        }
        Move();
    }
    [Task]
    public void MoveUp()
    {
        RedetermineHabits();
        if (retreatFromFocus)
        {
            if (moveDiagonalRetreat)
            {
                directionToMove = (byte)(IsRightOfPlayer() ? 1 : 3);
            }
            else
            {
                directionToMove = 2;
            }
        }
        else
        {
            if (moveDiagonalCloseIn)
            {
                directionToMove = (byte)(IsRightOfPlayer() ? 3 : 1);
            }
            else
            {
                directionToMove = 2;
            }
        }
        Move();
    }
    [Task]
    public void MoveDown()
    {
        RedetermineHabits();
        if (retreatFromFocus)
        {
            if (moveDiagonalRetreat)
            {
                directionToMove = (byte)(IsRightOfPlayer() ? 7 : 5);
            }
            else
            {
                directionToMove = 6;
            }
        }
        else
        {
            if (moveDiagonalCloseIn)
            {
                directionToMove = (byte)(IsRightOfPlayer() ? 5 : 7);
            }
            else
            {
                directionToMove = 6;
            }
        }
        Move();
    }
    
    [Task]
    public void MakeAttack1()
    {
        //Debug.Log("Make attack one.");
        enemyAttack.MakeAttack(0);
        canHitTimer = 3f;
        Task.current.Complete(false);
    }
    [Task]
    public void MakeAttack2()
    {
        Debug.Log("Make attack two.");
        canHitTimer = 4f;
        Task.current.Complete(true);
    }
    [Task]
    public void MakeAttack3()
    {
        Debug.Log("Make attack three.");
        canHitTimer = 5f;
        Task.current.Complete(true);
    }
    [Task]
    public bool IsRightOfPlayer()
    {
        return transform.position.x >= currentFocus.transform.position.x;
    }
    [Task]
    public bool PausedOnMove()
    {
        return pauseMoveTimer > 0f;
    }
    [Task]
    public bool IsAbovePlayer()
    {
        if (currentFocus == null)
        {
            Debug.LogWarning("NOTE: Enemy does not have a Focus!!");
            return false;
        }
        return (transform.position.y - currentFocus.transform.position.y) > 0f;
    }
    [Task]
    public bool IsBelowPlayer()
    {
        if (currentFocus == null)
        {
            Debug.LogWarning("NOTE: Enemy does not have a Focus!!");
            return false;
        }
        return (transform.position.y - currentFocus.transform.position.y) <= 0f;
    }
    [Task]
    public bool WithinDistance()
    {
        return Mathf.Abs(transform.position.x - currentFocus.transform.position.x) <= idealDistanceFromPlayer;
    }
    [Task]
    public bool WithinHorizontalAxis()
    {
        return Mathf.Abs(transform.position.y - currentFocus.transform.position.y) <= 1f;
    }
    [Task]
    public bool CanAttack()
    {
        return canHitTimer <= 0f;
    }
    [Task]
    public bool KnockedOut()
    {
        return enemyStats.StaminaEmpty();
    }

    private void Move()
    {
        switch (directionToMove)
        {
            case 0:
                enemyMove.Move(new Vector2(1, 0));
                break;
            case 1:
                enemyMove.Move(new Vector2(1, 1));
                break;
            case 2:
                enemyMove.Move(new Vector2(0, 1));
                break;
            case 3:
                enemyMove.Move(new Vector2(-1, 1), true);
                break;
            case 4:
                enemyMove.Move(new Vector2(-1, 0));
                break;
            case 5:
                enemyMove.Move(new Vector2(-1, -1));
                break;
            case 6:
                enemyMove.Move(new Vector2(-1, 0));
                break;
            case 7:
                enemyMove.Move(new Vector2(1, -1));
                break;
            default:
                enemyMove.Move(Vector2.zero);
                break;
        }
        Task.current.Complete(true);
    }
    /// <summary>
    /// Redetermine how the Enemy will move around.
    /// </summary>
    private void RedetermineHabits()
    {
        if (redetermineHabits <= 0f)
        {
            DetermineCloseInMovePattern();
            DetermineRetreatMovePattern();
            redetermineHabits = 1f;
        }
    }
    private void CheckDetourPaths()
    {
        for (int i = 0; i < 8; i++)
        {
            //Check each direction to see if can move to if needing to
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, GetUnitCircleLine((byte)i), 4, thingsInTheWay);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit == this)
                {
                    continue;
                }
                if (hit.collider.gameObject.layer == 7 || hit.collider.gameObject.layer == 9)
                {
                    detourPath |= (byte)(0x1 << i);
                    continue;
                }
            }
            detourPath &= (byte)~(0x1 << i);
        }
    }
    /// <summary>
    /// Check if Enemy will close in via a Diagonal only pattern.
    /// If Close In-Habits = 0, Enemy will only Square move; if =4, Enemy will only Diamond Move.
    /// If Enemy is on the same Horizontal Axis as its Focus, Square move only.
    /// </summary>
    private void DetermineCloseInMovePattern()
    {
        if (closeInHabits == 0)
        {
            moveDiagonalCloseIn = false;
        }
        else
        {
            moveDiagonalCloseIn = Random.Range(0, 100) <= (closeInHabits * 25);
        }
    }
    /// <summary>
    /// Check if Enemy will retreat via a Diagonal only pattern.
    /// If Close In-Habits = 0, Enemy will only Square move; if =4, Enemy will only Diamond Move.
    /// If Enemy is on the same Horizontal Axis as its Focus, Square move only.
    /// </summary>
    private void DetermineRetreatMovePattern()
    {
        if (retreatHabits == 0)
        {
            moveDiagonalRetreat = false;
        }
        else
        {
            moveDiagonalRetreat = Random.Range(0, 100) <= (retreatHabits * 25);
        }
    }
    /// <summary>
    /// Is the specified path path open?
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    private bool DetourPathOpen(byte section)
    {
        return (((0x1 << section) & detourPath) >> section) == 0x1;
    }
    private byte GetRadianDirection(Vector2 line)
    {
        if (line == Vector2.zero)
        {
            return 5;
        }
        float angle = Mathf.Atan2(line.y, line.x);
        byte eighthAngle = (byte)(Mathf.Round((8 * angle) / (2 * Mathf.PI) + 8) % 8);
        return eighthAngle;
    }
    private byte GetTrueDirection(byte radianDirection)
    {
        if (radianDirection > 9)
        {
            return 5;
        }
        switch (radianDirection)
        {
            case 0: //Right
                return 6;
            case 1: //Up-Right
                return 9;
            case 2: //Up
                return 8;
            case 3: //Up-Left
                return 7;
            case 4: //Left
                return 4;
            case 5: //Down-Left
                return 1;
            case 6: //Down
                return 2;
            case 7: //Down-Right
                return 3;
            default: //Neutral
                return 5;
        }
    }
    /// <summary>
    /// Get the Vector2 within the unit circle.
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    private Vector2 GetUnitCircleLine(byte section)
    {
        if (section > 7)
        {
            section = 0;
        }
        float sinAngle = Mathf.Sin((45 * section) * Mathf.Deg2Rad);
        float cosAngle = Mathf.Cos((45 * section) * Mathf.Deg2Rad);
        return new Vector2(cosAngle, sinAngle);
    }
    /// <summary>
    /// Return a Vector toward a direct path to the current Focus, obstacle or not.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetVectorToFocus()
    {
        return (currentFocus.transform.position - transform.position).normalized;
    }
}

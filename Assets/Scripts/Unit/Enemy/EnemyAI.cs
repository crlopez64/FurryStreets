using UnityEngine;
using System.Collections;
using Panda;

/// <summary>
/// Script in charge of methods for valid AI commands.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    private Vector3 vectorToPlayer;
    private PlayerMove player;
    private EnemyMove enemyMove;
    private EnemyStats enemyStats;
    private EnemyAttack enemyAttack;
    private bool haveToDetour;
    private byte directionToMove;
    private byte detourPath; //For each bit, 0 = free, 1 = obstacle in the way
    private float moveTimer;
    private float canHitTimer;

    public LayerMask whoToAttack;
    public LayerMask thingsInTheWay;
    [Range(1f, 10f)]
    public float idealDistanceFromPlayer;
    [Range(1.06f, 2f)]
    public float idealGrabDistanceFromPlayer;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>();
        enemyStats = GetComponent<EnemyStats>();
        enemyMove = GetComponent<EnemyMove>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        //Timers
        if (moveTimer > 0f)
        {
            moveTimer -= Time.deltaTime;
        }
        if (canHitTimer > 0f)
        {
            canHitTimer -= Time.deltaTime;
        }

        //Character flip facing?

        //Get Vector from enemy to player
        vectorToPlayer = (player.transform.position - transform.position).normalized;
        //Check if there's anything between this Unit and the Player
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,
            vectorToPlayer, Vector2.Distance(transform.position, player.transform.position), thingsInTheWay);
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject.layer == 7 || hit.collider.gameObject.layer == 9)
            {
                haveToDetour = true;
                break;
            }
            haveToDetour = false;
        }
        if (haveToDetour)
        {
            //Use any other direction to move
        }
        else
        {
            //Move to player location
            directionToMove = GetRadianDirection(vectorToPlayer);

        }
    }
    [Task]
    public void FlipSprite()
    {
        enemyMove.FlipSprite();
        Task.current.Complete(true);
    }
    [Task]
    public void Move()
    {
        switch(directionToMove)
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
                enemyMove.Move(new Vector2(-1, 1));
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
    [Task]
    public void ResetMoveTimer()
    {
        moveTimer = Random.Range(1f, 3f);
        Task.current.Complete(true);
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
        return transform.position.x >= player.transform.position.x;
    }
    [Task]
    public bool CanStillMove()
    {
        return moveTimer > 0f;
    }
    [Task]
    public bool IsAbovePlayer()
    {
        if (player != null)
        {
            return (transform.position.y - player.transform.position.y) > 0f;
        }
        else
        {
            return false;
        }
    }
    [Task]
    public bool IsBelowPlayer()
    {
        if (player != null)
        {
            return (transform.position.y - player.transform.position.y) <= 0f;
        }
        else
        {
            return false;
        }
    }
    [Task]
    public bool WithinDistance()
    {
        return Mathf.Abs(transform.position.x - player.transform.position.x) <= idealDistanceFromPlayer;
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
    /// Is the specified path path open?
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    private bool DetourPathOpen(byte section)
    {
        return (((0x1 << section) & detourPath) >> section) == 0x1;
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
}

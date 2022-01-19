using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

/// <summary>
/// Script in charge of methods for valid Non-Enemy AI commands.
/// </summary>
[RequireComponent(typeof(NPCMove))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(UnitAnimationLayers))]
public class NotEnemyAI : MonoBehaviour
{
    private NPCMove npcMove;
    private List<Vector3> pathways;
    private bool continuePath;
    private byte directionToMove;
    private int currentPathwayIndex;

    public List<Transform> walkLocations;

    private void Awake()
    {
        npcMove = GetComponent<NPCMove>();
    }
    private void Start()
    {
        SetPathway();
    }

    private void Update()
    {
        
    }

    public void ContinuePathway()
    {
        continuePath = true;
    }
    [Task]
    public void StopMoving()
    {
        npcMove.StopMoving();
        Task.current.Succeed();
    }
    [Task]
    public void GetDirectRoute()
    {
        directionToMove = GetRadianDirection(GetVectorToNextPosition());
        Move();
        //Task.current.Succeed();
    }
    [Task]
    public void GetNextDestination()
    {
        currentPathwayIndex++;
        if (currentPathwayIndex >= pathways.Count)
        {
            currentPathwayIndex = 0;
        }
        Task.current.Succeed();
    }
    [Task]
    public bool HasPathway()
    {
        return pathways.Count > 1;
    }
    [Task]
    public bool ReachedDestination()
    {
        if (pathways.Count <= 0)
        {
            return true;
        }
        if ((transform.position.x <= (pathways[currentPathwayIndex].x + 0.3f)) && (transform.position.x >= (pathways[currentPathwayIndex].x - 0.3f)))
        {
            if ((transform.position.y <= (pathways[currentPathwayIndex].y + 0.3f)) && (transform.position.y >= (pathways[currentPathwayIndex].y - 0.3f)))
            {
                Debug.Log("Within destination");
                return true;
            }
        }
        return false;
    }

    private void Move()
    {
        switch (directionToMove)
        {
            case 0:
                npcMove.Move(new Vector2(1, 0));
                break;
            case 1:
                npcMove.Move(new Vector2(1, 1));
                break;
            case 2:
                npcMove.Move(new Vector2(0, 1));
                break;
            case 3:
                npcMove.Move(new Vector2(-1, 1));
                break;
            case 4:
                npcMove.Move(new Vector2(-1, 0));
                break;
            case 5:
                npcMove.Move(new Vector2(-1, -1));
                break;
            case 6:
                npcMove.Move(new Vector2(-1, 0));
                break;
            case 7:
                npcMove.Move(new Vector2(1, -1));
                break;
            default:
                npcMove.Move(Vector2.zero);
                break;
        }
        Task.current.Succeed();
    }
    /// <summary>
    /// Set up the paths for this AI and delete the gameObjects for this path.
    /// </summary>
    private void SetPathway()
    {
        currentPathwayIndex = 0;
        pathways = new List<Vector3>();
        pathways.Add(transform.position);
        DestinationMove[] dsetinations = GetComponentsInChildren<DestinationMove>();
        if (dsetinations.Length <= 0)
        {
            return;
        }
        foreach(DestinationMove walkLocation in dsetinations)
        {
            Vector3 position = new Vector3(walkLocation.transform.position.x, walkLocation.transform.position.y, walkLocation.transform.position.z);
            pathways.Add(position);
            Destroy(walkLocation.gameObject);
        }
        pathways.TrimExcess();
    }
    /// <summary>
    /// Return the radian direction.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Return a Vector toward a direct path to the current Focus, obstacle or not.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetVectorToNextPosition()
    {
        return (pathways[currentPathwayIndex] - transform.position).normalized;
    }
}

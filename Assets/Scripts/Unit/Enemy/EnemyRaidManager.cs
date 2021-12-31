using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of making Enemy raids.
/// </summary>
public class EnemyRaidManager : MonoBehaviour
{
    private EnemyRaidLocation enemyRaidLocation;
    private Queue<GameObject> nextRaiders;
    private List<GameObject> defeated;
    private BoxCollider2D[] raidBorders;
    private byte currentCount;

    public byte maxCount;
    public Vector2 cameraPosition;
    public List<GameObject> raiders;

    private void Awake()
    {
        defeated = new List<GameObject>(raiders.Count);
        nextRaiders = new Queue<GameObject>(raiders.Count);
        raidBorders = GetComponentsInChildren<BoxCollider2D>();
    }
    private void Start()
    {
        currentCount = 0;
        SetBorders(false);
    }

    /// <summary>
    /// Move the Raid arena and start the raid.
    /// </summary>
    public void StartRaid(EnemyRaidLocation enemyRaidLocation)
    {
        defeated.Clear();
        transform.position = enemyRaidLocation.transform.position;
        this.enemyRaidLocation = enemyRaidLocation;
        SetBorders(true);
        for(int i = 0; i < raiders.Count; i++)
        {
            nextRaiders.Enqueue(raiders[i]);
        }
        for(int i = 0; i < maxCount; i++)
        {
            Debug.Log("Spawn next raider.");
        }
        //Move camera
        currentCount = maxCount;
    }
    
    /// <summary>
    /// Reduce the current raider count at play.
    /// </summary>
    public void ReduceCurrentCount()
    {
        currentCount--;
    }
    /// <summary>
    /// Add the next raider into the queue. If no raiders left, then end the raid.
    /// </summary>
    /// <param name="enemy"></param>
    public void AddNextRaider(GameObject enemy)
    {
        if (nextRaiders.Count > 0)
        {
            if (currentCount < maxCount)
            {
                if (raiders.Contains(enemy))
                {
                    defeated.Add(enemy);
                    Debug.Log("Spawn next raider.");
                    currentCount++;
                }
            }
        }
        else
        {
            Debug.Log("End the raid");
            enemyRaidLocation.SetRaidComplete(true);
            enemyRaidLocation = null;
            SetBorders(false);
        }
    }

    /// <summary>
    /// Set the borders on or off.
    /// </summary>
    /// <param name="tOrF"></param>
    private void SetBorders(bool tOrF)
    {
        foreach(BoxCollider2D border in raidBorders)
        {
            border.enabled = tOrF;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of activating and starting a raid.
/// </summary>
public class EnemyRaidLocation : MonoBehaviour
{
    private bool raidComplete;
    private EnemyRaidManager enemyRaidManager;

    private void Awake()
    {
        enemyRaidManager = FindObjectOfType<EnemyRaidManager>();
    }
    private void Start()
    {
        raidComplete = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyRaidManager.StartRaid(this);
        }
    }

    /// <summary>
    /// Set if this Raid has been complete or not.
    /// </summary>
    /// <param name="tOrF"></param>
    public void SetRaidComplete(bool tOrF)
    {
        raidComplete = tOrF;
    }
    /// <summary>
    /// Return if this Raid has been completed or not.
    /// </summary>
    /// <returns></returns>
    public bool RaidHasBeenComplete()
    {
        return raidComplete;
    }
}

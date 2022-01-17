using UnityEngine;

/// <summary>
/// Script in charge of seeing if the Player is actually too close.
/// </summary>
public class EnemyEyesightTooClose : MonoBehaviour
{
    private bool playerOnSight;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerMove>() != null)
        {
            playerOnSight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerMove>() != null)
        {
            playerOnSight = false;
        }
    }

    /// <summary>
    /// Is the Player on the Enemy's sight?
    /// </summary>
    /// <returns></returns>
    public bool PlayerOnSight()
    {
        return playerOnSight;
    }
}

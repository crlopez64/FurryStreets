using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIHelpers : MonoBehaviour
{
    public BoxCollider2D eyesight;
    public BoxCollider2D eyesightTooClose;

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of having Units touch the backside of buildings to make them behind them.
/// </summary>
public class BuildingMasking : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer >= 6) && (collision.gameObject.layer <= 9))
        {
            collision.GetComponentInParent<SpriteRenderer>().sortingLayerName = "Background";
            collision.GetComponentInParent<SpriteRenderer>().sortingOrder = 3;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer >= 6) && (collision.gameObject.layer <= 9))
        {
            collision.GetComponentInParent<SpriteRenderer>().sortingLayerName = "Units";
            collision.GetComponentInParent<SpriteRenderer>().sortingOrder = 0;
        }
    }
}

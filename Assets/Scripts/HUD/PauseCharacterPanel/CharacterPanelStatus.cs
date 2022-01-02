using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of showing stat effects that Player may have.
/// </summary>
public class CharacterPanelStatus : MonoBehaviour
{
    //TODO: If Player has status effects, turn on.

    private GridLayout gridLayout;

    private void Awake()
    {
        gridLayout = GetComponent<GridLayout>();
    }


}

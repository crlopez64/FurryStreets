using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of the character portrait.
/// </summary>
public class CharacterPanelPortrait : MonoBehaviour
{
    private Image portrait;

    public void Awake()
    {
        portrait = GetComponent<Image>();
    }

    /// <summary>
    /// Swap the character portrait.
    /// </summary>
    /// <param name="sprite"></param>
    public void SwapPortrait(Sprite sprite)
    {
        portrait.sprite = sprite;
    }
}

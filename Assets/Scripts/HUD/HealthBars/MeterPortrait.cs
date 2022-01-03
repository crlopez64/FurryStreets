using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of showing the player potrait.
/// </summary>
public class MeterPortrait : MonoBehaviour
{
    private Image portrait;

    private void Awake()
    {
        portrait = GetComponent<Image>();
    }

    /// <summary>
    /// Adjust the character portrait based on the character chosen. Mateo(Wolf) = 0, Fox(Fox) = 1
    /// </summary>
    public void SetPortrait(byte characterChosen)
    {
        switch (characterChosen)
        {
            case 0:
                portrait.sprite = Resources.Load<Sprite>("HUD/Portraits_Icon/Portrait_Wolf");
                break;
            case 1:
                portrait.sprite = Resources.Load<Sprite>("HUD/Portraits_Icon/Portrait_Fox");
                break;
            default:
                break;
        }
    }
}

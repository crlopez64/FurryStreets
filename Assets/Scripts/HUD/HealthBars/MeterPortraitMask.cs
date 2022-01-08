using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of portrait mask.
/// </summary>
public class MeterPortraitMask : MonoBehaviour
{
    private Image mask;
    private MeterPortrait portrait;

    private void Awake()
    {
        mask = GetComponent<Image>();
        portrait = GetComponentInChildren<MeterPortrait>();
    }

    /// <summary>
    /// Adjust the character portrait based on the character chosen. Mateo(Wolf) = 0, Fox(Fox) = 1
    /// </summary>
    public void SetPortrait(byte characterChosen)
    {
        switch (characterChosen)
        {
            case 0:
                mask.sprite = Resources.Load<Sprite>("HUD/Portraits_Mask/PortraitMaskWolf");
                portrait.SetPortrait(characterChosen);
                break;
            case 1:
                mask.sprite = Resources.Load<Sprite>("HUD/Portraits_Mask/PortraitMaskFox");
                portrait.SetPortrait(characterChosen);
                break;
            default:
                break;
        }
    }
}

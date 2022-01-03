using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in charge of the HUD frame and making the Health/Meter looks nice.
/// </summary>
public class MeterFrame : MonoBehaviour
{
    private Image frame;

    private void Awake()
    {
        frame = GetComponent<Image>();
    }

    /// <summary>
    /// Adjust the health and meter frame based on the character chosen. Mateo(Wolf) = 0, Fox(Fox) = 1
    /// </summary>
    public void SetMeterFrame(byte characterChosen)
    {
        switch (characterChosen)
        {
            case 0:
                frame.sprite = Resources.Load<Sprite>("HUD/HealthBars/HealthMeterWolf/HealthMeterFrameWolf");
                break;
            case 1:
                frame.sprite = Resources.Load<Sprite>("HUD/HealthBars/HealthMeterFox/HealthMeterFrameFox");
                break;
            default:
                break;
        }
    }
}

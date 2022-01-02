using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of visually showing Meter values.
/// </summary>
public class CharacterPanelMeter : MonoBehaviour
{
    private CharacterPanelCurrentValue currentText;
    private CharacterPanelMaxValue maxText;

    private void Awake()
    {
        currentText = GetComponentInChildren<CharacterPanelCurrentValue>();
        maxText = GetComponentInChildren<CharacterPanelMaxValue>();
    }

    /// <summary>
    /// Set the current stat values.
    /// </summary>
    /// <param name="currentValue"></param>
    /// <param name="maxValue"></param>
    public void SetStat(int currentValue, int maxValue)
    {
        currentText.SetCurrentHealthValue(currentValue);
        maxText.SetMaxValue(maxValue);
    }
}

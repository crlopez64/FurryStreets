using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script in charge of keeping track of the Player Character Panel.
/// </summary>
public class PauseMenuMainCharacterPanel : MonoBehaviour
{
    private CharacterPanelName characterName;
    private CharacterPanelPortrait portrait;
    private CharacterPanelHealth health;
    private CharacterPanelMeter meter;

    private void Awake()
    {
        characterName = GetComponentInChildren<CharacterPanelName>();
        portrait = GetComponentInChildren<CharacterPanelPortrait>();
        health = GetComponentInChildren<CharacterPanelHealth>();
        meter = GetComponentInChildren<CharacterPanelMeter>();
    }
    private void Start()
    {
        
    }

    /// <summary>
    /// Empty the panel.
    /// </summary>
    public void EmptyPanel()
    {
        characterName.SetName("");
        portrait.SwapPortrait(null);
        health.SetStat(0, 0);
        meter.SetStat(0, 0);
        gameObject.SetActive(false);
    }

}

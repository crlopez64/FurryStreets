using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script in charge of the Main Pause Menu in the game.
/// </summary>
public class PauseMenuMain : MonoBehaviour
{
    public TextMeshProUGUI staminaCurrent;
    public TextMeshProUGUI staminaMax;
    public TextMeshProUGUI meterCurrent;
    public TextMeshProUGUI meterMax;
    public TextMeshProUGUI money;
    public TextMeshProUGUI novi;

    /// <summary>
    /// Update the main pause menu and its values.
    /// </summary>
    /// <param name="player"></param>
    public void UpdatePauseMain(PlayerStats player)
    {
        staminaCurrent.text = player.CurrentHealth().ToString();
        staminaMax.text = player.MaxHealth().ToString();
        meterCurrent.text = player.CurrentMeter().ToString();
        meterMax.text = player.MaxMeter().ToString();
        money.text = "-1";
        novi.text = "-1";
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script that turns on and keep on the Enemy HUD when active. Hitting enemy resets the timer before this HUD goes away.
/// </summary>
public class EnemyHUD : MonoBehaviour
{
    private TextMeshProUGUI fountainPen;
    private Image portrait;
    private float canvasTimer;

    private void Awake()
    {
        portrait = GetComponentInChildren<Image>();
        fountainPen = GetComponentInChildren<TextMeshProUGUI>();
    }
    void Start()
    {
        TurnOffHUD();
        canvasTimer = 0f;
    }
    void Update()
    {
        if (canvasTimer > 0)
        {
            canvasTimer -= Time.deltaTime;
        }
        else
        {
            TurnOffHUD();
        }
    }

    /// <summary>
    /// Turn on the canvas graphics.
    /// </summary>
    public void TurnOnHUD(UnitStats unitStats)
    {
        //get portait sprites from enemy
        portrait.enabled = true;
        Debug.Log("Stamina: " + unitStats.CurrentHealth() + "/" + unitStats.MaxHealth());
        canvasTimer = 5f;
    }
    /// <summary>
    /// Turn off the HUD.
    /// </summary>
    public void TurnOffHUD()
    {
        portrait.enabled = false;
        portrait.sprite = null;
        fountainPen.text = "";
    }
}

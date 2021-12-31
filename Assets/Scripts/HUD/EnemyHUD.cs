using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script that turns on and keep on the Enemy HUD when active. Hitting enemy resets the timer before this HUD goes away.
/// </summary>
public class EnemyHUD : MonoBehaviour
{
    private TextMeshProUGUI fountainPen;
    private MeterBarHealth meterBarHealth;
    private Image portrait;
    private float canvasTimer;

    private void Awake()
    {
        portrait = GetComponentInChildren<Image>();
        meterBarHealth = GetComponentInChildren<MeterBarHealth>();
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
        meterBarHealth.gameObject.SetActive(true);
        meterBarHealth.SetMaxValue(unitStats.MaxHealth());
        meterBarHealth.SetSliderValue(unitStats.CurrentHealth());
        canvasTimer = 5f;
    }
    /// <summary>
    /// Turn off the HUD.
    /// </summary>
    public void TurnOffHUD()
    {
        portrait.enabled = false;
        portrait.sprite = null;
        meterBarHealth.gameObject.SetActive(false);
        fountainPen.text = "";
    }
}

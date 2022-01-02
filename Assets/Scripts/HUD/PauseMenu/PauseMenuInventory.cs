using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script in charge of the Inventory visual in the Pause Menu.
/// </summary>
public class PauseMenuInventory : MonoBehaviour
{
    private PauseMenuInventoryButton[] buttons;

    public TextMeshProUGUI description;
    public GameObject firstItem;

    private void Awake()
    {
        buttons = GetComponentsInChildren<PauseMenuInventoryButton>();
    }

    /// <summary>
    /// Set up the buttons and align them with hte current inventory.
    /// </summary>
    /// <param name="currentInventory"></param>
    public void SetUpInventory(List<Item> currentInventory)
    {
        foreach(PauseMenuInventoryButton button in buttons)
        {
            button.SetButtonEmpty();
            button.gameObject.SetActive(false);
        }
        if ((currentInventory == null) || (currentInventory.Count == 0))
        {
            Debug.LogWarning("HOLD ON: There is no inventory set.");
            buttons[0].SetButtonBlank();
            buttons[0].gameObject.SetActive(true);
            return;
        }
        int index = 0;
        foreach(Item item in currentInventory)
        {
            buttons[index].SetButtonVisual(item);
            buttons[index].gameObject.SetActive(true);
            index++;
        }
    }
}

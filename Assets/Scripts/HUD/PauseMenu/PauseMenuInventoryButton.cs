using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Script in charge of the buttons in the Inventory HUD.
/// </summary>
public class PauseMenuInventoryButton : PauseMenuButton, IPointerEnterHandler, ISelectHandler
{
    private string moveName;
    private string description;
    private Image itemSprite;
    private TextMeshProUGUI itemName;

    private void Awake()
    {
        itemSprite = GetComponentInChildren<Image>();
        itemName = GetComponentInChildren<TextMeshProUGUI>();
    }
    /// <summary>
    /// Clear out a button of its contents.
    /// </summary>
    public void SetButtonEmpty()
    {
        moveName = "";
        description = "";
        itemName.text = "";
        itemSprite.sprite = null;
        itemSprite.enabled = true;
    }
    /// <summary>
    /// Set the Item Visual for the button.
    /// </summary>
    /// <param name="spritePathName"></param>
    /// <param name="name"></param>
    public void SetButtonVisual(Item item)
    {
        moveName = item.GetName();
        description = item.GetDescription();
        itemSprite.sprite = Resources.Load<Sprite>(item.GetSpritePath());
        itemSprite.type = Image.Type.Simple;
        itemSprite.preserveAspect = true;
        itemName.text = moveName;
        itemSprite.enabled = true;
    }
    /// <summary>
    /// Set an invisible button.
    /// </summary>
    public void SetButtonBlank()
    {
        moveName = "";
        description = "No Inventory...";
        itemName.text = "";
        itemSprite.sprite = null;
        itemSprite.enabled = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (GetComponent<Button>().IsInteractable())
        {
            GetComponentInParent<PauseMenuInventory>().description.text = description;
        }
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (GetComponent<Button>().IsInteractable())
        {
            GetComponentInParent<PauseMenuInventory>().description.text = description;
        }
    }
}

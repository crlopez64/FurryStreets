using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Script used for making functioning buttons for the Pause Menu.
/// </summary>
public class PauseMenuButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler, ICancelHandler
{
    private RectTransform rectTransform;
    private TextMeshProUGUI fountainPen;
    private float nextDimension;
    private bool isSelected;
    private float velocityRef;

    public bool mainPauseMenuButton;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        fountainPen = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        isSelected = false;
        nextDimension = 120;
    }
    private void Update()
    {
        if (mainPauseMenuButton)
        {
            if (!isSelected)
            {
                rectTransform.sizeDelta = new Vector2(120, 120);
                fountainPen.fontSize = 24;
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(200, 200);
                fountainPen.fontSize = 38;
            }
        }
        
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>().IsInteractable())
        {
            if (!EventSystem.current.alreadySelecting)
            {
                Debug.Log("Moved button");
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        }
    }
    public virtual void OnSelect(BaseEventData eventData)
    {
        if (GetComponent<Button>().IsInteractable())
        {
            isSelected = true;
        }
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (GetComponent<Button>().IsInteractable())
        {
            isSelected = false;
            GetComponent<Selectable>().OnPointerExit(null);
        }
    }
    public void OnCancel(BaseEventData eventData)
    {
        if (mainPauseMenuButton)
        {
            GameManager.Instance.PauseGame();
        }
        else
        {
            GetComponentInParent<PauseMenuPanels>().ButtonMainPanel();
        }
    }
}

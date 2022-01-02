using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Script in charge of the panel placements.
/// </summary>
public class PauseMenuPanels : MonoBehaviour
{
    private Vector2 titleTargetPosition;
    private Vector2 bodyTargetPosition;
    private Vector2 controlsTargetPosition;
    private Vector2 refVelocity0;
    private Vector2 refVelocity1;
    private Vector2 refVelocity2;

    public Transform bodyOnScreenPosition;
    public Transform bodyOffScreenPosition;
    public Transform titleOnScreenPosition;
    public Transform titleOffScreenPosition;
    public Transform controlsOnScreen;
    public Transform controlsOffScreen;
    public Image titlePanel;
    public Image bodyPanel;
    public Image controlsPanel;
    public GameObject mainPanel;
    public GameObject inventoryPanel;
    public GameObject movelistPanel;
    public GameObject firstButton;

    private void Awake()
    {
        titleTargetPosition = titleOffScreenPosition.position;
        bodyTargetPosition = bodyOffScreenPosition.position;
        controlsTargetPosition = controlsOffScreen.position;
    }
    private void Update()
    {
        titlePanel.transform.position = Vector2.SmoothDamp(titlePanel.transform.position, titleTargetPosition,
            ref refVelocity0, 0.2f, Mathf.Infinity, Time.fixedDeltaTime * 0.2f);
        bodyPanel.transform.position = Vector2.SmoothDamp(bodyPanel.transform.position, bodyTargetPosition,
            ref refVelocity1, 0.2f, Mathf.Infinity, Time.fixedDeltaTime * 0.2f);
        controlsPanel.transform.position = Vector2.SmoothDamp(controlsPanel.transform.position, controlsTargetPosition,
            ref refVelocity2, 0.2f, Mathf.Infinity, Time.fixedDeltaTime * 0.2f);
    }

    public void TransitionPanels(bool pauseOn)
    {
        titleTargetPosition = pauseOn ? titleOnScreenPosition.position : titleOffScreenPosition.position;
        bodyTargetPosition = pauseOn ? bodyOnScreenPosition.position : bodyOffScreenPosition.position;
        controlsTargetPosition = pauseOn ? controlsOnScreen.position : controlsOffScreen.position;
    }
    
    public void ButtonMainPanel()
    {
        SetTitle("Paused Game");
        SetControls(true);
        mainPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        movelistPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
    public void ButtonInventoryPanel()
    {
        SetTitle("Inventory");
        SetControls(true);
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        movelistPanel.SetActive(false);
        GetComponentInChildren<PauseMenuInventory>().SetUpInventory(GameManager.Instance.GetComponent<Inventory>().GetCurrentInventory());
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GetComponentInChildren<PauseMenuInventory>().firstItem);
    }
    public void ButtonMoveListPanel()
    {
        SetTitle("Move List");
        SetControls(false);
        mainPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        movelistPanel.SetActive(true);
        GetComponentInChildren<PauseMenuMoveList>().ShowMoveList(GameManager.Instance.Player().GetComponent<PlayerAttack>().RootAttack());
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GetComponentInChildren<PauseMenuMoveList>().firstMove);
    }

    /// <summary>
    /// Set the title panel.
    /// </summary>
    /// <param name="text"></param>
    private void SetTitle(string text)
    {
        titlePanel.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
    /// <summary>
    /// Set the controls panel.
    /// </summary>
    /// <param name="canConfirm"></param>
    private void SetControls(bool canConfirm)
    {
        controlsPanel.GetComponentInChildren<TextMeshProUGUI>().text = canConfirm ? "[A] Select\n[B] Back" : "[B] Back";
    }
}

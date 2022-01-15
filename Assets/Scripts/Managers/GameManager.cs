using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// The script that is the Game Manager.
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private PlayerMove player;
    private Camera gameCamera;
    private bool pauseGame;

    public Canvas playerHUD;
    public Canvas pauseMenu;
    public Canvas blackFade;
    public Canvas dialogue;
    public Canvas currency;

    private void Awake()
    {
        if (playerHUD == null || pauseMenu == null || blackFade == null || dialogue == null)
        {
            Debug.LogError("ERROR: Not all canvases set on the Manager.");
        }
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        player = FindObjectOfType<PlayerMove>();
        Debug.Log("Disabling mouse");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Time.timeScale = 0.5f;
    }
    private void Start()
    {
        playerHUD.gameObject.SetActive(true);
        dialogue.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        blackFade.gameObject.SetActive(false);
        currency.GetComponentInChildren<HUDCurrencyHolder>().TurnOnVisuals();
        currency.gameObject.SetActive(true);
    }

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// The game camera for the scene.
    /// </summary>
    /// <param name="gameCamera"></param>
    public void SetCamera(Camera gameCamera)
    {
        this.gameCamera = gameCamera;
    }
    public void SetCameraClamps(LocationBinds locationBinds)
    {
        gameCamera.GetComponent<CameraFollow>().SetCameraClamps(locationBinds);
    }
    /// <summary>
    /// Turn on the black panel and fade in.
    /// </summary>
    public void BlackFadeOn()
    {
        blackFade.gameObject.SetActive(true);
        blackFade.GetComponent<HUDBlackPanel>().WipeOutPanel();
        blackFade.GetComponent<HUDBlackPanel>().FadeIn();
    }
    /// <summary>
    /// If the black panel is on, fade out.
    /// </summary>
    public void BlackFadeOff()
    {
        if (blackFade.gameObject.activeInHierarchy)
        {
            blackFade.GetComponent<HUDBlackPanel>().FadeOut();
        }
    }
    public void PauseGame()
    {
        pauseGame = !pauseGame; 
        playerHUD.gameObject.SetActive(!pauseGame);
        dialogue.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
        if (pauseGame)
        {
            pauseMenu.GetComponent<PauseMenuPanels>().ButtonMainPanel();
        }
        StopAllCoroutines();
        StartCoroutine(PauseGameTurnOffPause());
        blackFade.gameObject.SetActive(false);
        if (!pauseGame)
        {
            currency.GetComponentInChildren<HUDCurrencyHolder>().TurnOnVisuals();
        }
        currency.gameObject.SetActive(!pauseGame);
        pauseMenu.GetComponent<PauseMenuPanels>().TransitionPanels(pauseGame);
        EventSystem.current.SetSelectedGameObject(pauseMenu.GetComponent<PauseMenuPanels>().firstButton);
        Time.timeScale = (!pauseGame) ? 1f : 0f;
    }
    /// <summary>
    /// Is the game currently paused?
    /// </summary>
    /// <returns></returns>
    public bool GamePaused()
    {
        return pauseGame;
    }
    /// <summary>
    /// Get the player reference.
    /// </summary>
    /// <returns></returns>
    public PlayerMove Player()
    {
        return player;
    }

    private IEnumerator PauseGameTurnOffPause()
    {
        yield return new WaitForSeconds(1.5f);
        pauseMenu.GetComponent<PauseMenuPanels>().ButtonMainPanel();
        pauseMenu.gameObject.SetActive(pauseGame);
    }
}

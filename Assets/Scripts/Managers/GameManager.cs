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
    private DialogueManager dialogueManager;
    private PlayerMove player;
    private Camera gameCamera;
    private bool pauseGame;
    private bool dialoguing;
    private byte currentChapter;
    private byte currentChapterStep;

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
        Debug.Log("Disabling mouse");
        dialogueManager = GetComponent<DialogueManager>();
        player = FindObjectOfType<PlayerMove>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Time.timeScale = 0.5f;
    }
    private void Start()
    {
        playerHUD.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        blackFade.gameObject.SetActive(false);
        currency.GetComponentInChildren<HUDCurrencyHolder>().TurnOnVisuals();
        currency.gameObject.SetActive(true);
        dialogueManager.TurnOffDialogue();
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
    /// <summary>
    /// Set camera clamps.
    /// </summary>
    /// <param name="locationBinds"></param>
    public void SetCameraClamps(LocationBinds locationBinds)
    {
        gameCamera.GetComponent<CameraFollow>().SetCameraClamps(locationBinds);
    }
    /// <summary>
    /// Advance Chapter. This will move the Chapter step to zero.
    /// </summary>
    public void AdvanceChapter()
    {
        currentChapter++;
        currentChapterStep = 0;
    }
    /// <summary>
    /// Advance the Chapter step.
    /// </summary>
    public void AdvanceChapterStep()
    {
        currentChapterStep++;
    }
    /// <summary>
    /// Place back the current Chapter and Chapter Step.
    /// </summary>
    /// <param name="chapter"></param>
    /// <param name="chapterStep"></param>
    public void SetProgress(byte chapter, byte chapterStep)
    {
        currentChapter = chapter;
        currentChapterStep = chapterStep;
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
    /// <summary>
    /// Set if the player is talking to anyone or no.
    /// </summary>
    /// <param name="tOrF"></param>
    public void SetDialoguing(bool tOrF)
    {
        dialoguing = tOrF;
    }
    /// <summary>
    /// Set string for NPC Folder.
    /// </summary>
    /// <param name="npcFolder"></param>
    public void SetNPCFolder(string npcFolder)
    {
        dialogueManager.SetNPCHolder(npcFolder);
    }
    /// <summary>
    /// Start the Dialogue.
    /// </summary>
    /// <param name="currentPath"></param>
    public void StartDialogue(string currentPath)
    {
        dialogueManager.StartDialogue(currentPath);
    }
    /// <summary>
    /// Advance the dialogue. If no other dialogue can occur, turn the game back on.
    /// </summary>
    public void AdvanceText()
    {
        dialogueManager.AdvanceText();
    }
    /// <summary>
    /// Pause the game and open up Pause menu.
    /// </summary>
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
    /// Is the player currently talking with someone?
    /// </summary>
    /// <returns></returns>
    public bool Dialoguing()
    {
        return dialoguing;
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

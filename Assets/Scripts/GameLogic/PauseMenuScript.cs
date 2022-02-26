using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    private bool paused;

    GameObject pauseCanvas;
    GameObject mainPauseMenu;
    GameObject optionsMenu;
    GameObject controllerSettingsMenu;
    GameObject rebindPlayer1;
    GameObject rebindPlayer2;

    GameObject playerInputManager;

    PlayerInput playerInput;
    PlayerControlActions input;
    InputDevice lastInteract;
    
    void Awake()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
        pauseCanvas = transform.GetChild(0).gameObject;
        mainPauseMenu = pauseCanvas.transform.Find("MainPauseMenu").gameObject;
        optionsMenu = pauseCanvas.transform.Find("OptionsMenu").gameObject;
        controllerSettingsMenu = pauseCanvas.transform.Find("ControllerSettingsMenu").gameObject;
        rebindPlayer1 = controllerSettingsMenu.transform.Find("Rebind1").gameObject;
        rebindPlayer2 = controllerSettingsMenu.transform.Find("Rebind2").gameObject;

        playerInputManager = transform.parent.Find("Players").gameObject;

        paused = false;
        Time.timeScale = 1f;
        pauseCanvas.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            InputUser.PerformPairingWithDevice(
                Gamepad.all[i],
                playerInput.user
            );
        }
        

        InputUser.PerformPairingWithDevice(
            Keyboard.current,
            playerInput.user
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            Pause();
            if (paused)
            {
                pauseCanvas.SetActive(true);
                mainPauseMenu.SetActive(true);
            }
            else
            {
                mainPauseMenu.SetActive(false);
                optionsMenu.SetActive(false);
                controllerSettingsMenu.SetActive(false);
                pauseCanvas.SetActive(false);
            }
        }
    }

    public void EnablePlayerInput()
    {
        playerInput.ActivateInput();
    }

    public void DisablePlayerInput()
    {
        playerInput.DeactivateInput();
    }

    public void Pause()
    {
        if (paused)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
        paused = !paused;
        // Debug.Log(paused);
    }

    public bool IsPaused()
    {
        return paused;
    }

    public void InteractDetection(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
            lastInteract = ctx.control.device;
        if (ctx.canceled)
            lastInteract = null;
    }

    IEnumerator waitForInput(string player)
    {
        InputDevice rebindingDevice = null;
        currentlyRebinding(player);
        while (rebindingDevice == null && paused)
        {
            if (lastInteract != null)
            {
                rebindingDevice = lastInteract;
            }
            yield return null;
        }
        if (paused)
        {
            playerInputManager.GetComponent<InputManager>().RebindPlayerControls(player, rebindingDevice);
        }
        finishedRebinding();
        yield return rebindingDevice;
    }

    private void playerControlText()
    {
        GameObject player1text = controllerSettingsMenu.transform.Find("Player1Controller").gameObject;
        GameObject player2text = controllerSettingsMenu.transform.Find("Player2Controller").gameObject;

        if (ControlManager.player1Gamepad)
        {
            // add controller name
            player1text.GetComponent<Text>().text = "Player 1: " +
            Gamepad.all[ControlManager.player1GamepadIndex].name;
        }
        else
        {
            player1text.GetComponent<Text>().text = "Player 1: Keyboard";
        }

        if (ControlManager.player2Gamepad)
        {
            player2text.GetComponent<Text>().text = "Player 2: " +
            Gamepad.all[ControlManager.player2GamepadIndex].name;
        }
        else
        {
            player2text.GetComponent<Text>().text = "Player 2: Keyboard";
        }
    }

    // MainPauseMenu
    public void Resume()
    {
        Pause();
        pauseCanvas.SetActive(paused);
    }

    public void MainMenu()
    {
        Pause();
        transform.parent.Find("SceneLoader").GetComponent<SceneLoader>().LoadScene("Title Scene");
    }

    public void Options()
    {
        // hiding MainPauseMenu and showing OptionsMenu
        mainPauseMenu.SetActive(false);
        Transform craftingAssistant = optionsMenu.transform.Find("CraftingAssistantToggle");
        craftingAssistant.GetChild(0).gameObject.GetComponent<Text>().text = "Crafting Assistant - ";
        if (PauseMenuSettings.CraftingAssistantToggle)
            craftingAssistant.GetChild(0).gameObject.GetComponent<Text>().text = craftingAssistant.GetChild(0).gameObject.GetComponent<Text>().text + "On";
        else
            craftingAssistant.GetChild(0).gameObject.GetComponent<Text>().text = craftingAssistant.GetChild(0).gameObject.GetComponent<Text>().text + "Off";
        optionsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    // OptionsMenu
    public void CraftingAssistantToggle()
    {
        Transform craftingAssistant = optionsMenu.transform.Find("CraftingAssistantToggle");
        if (PauseMenuSettings.CraftingAssistantToggle)
        {
            craftingAssistant.GetChild(0).gameObject.GetComponent<Text>().text = "Crafting Assistant - Off";
            PauseMenuSettings.CraftingAssistantToggle = false;
        }
        else
        {
            craftingAssistant.GetChild(0).gameObject.GetComponent<Text>().text = "Crafting Assistant - On";
            PauseMenuSettings.CraftingAssistantToggle = true;
            
        }
    }

    public void ControllerSettings()
    {
        optionsMenu.SetActive(false);
        controllerSettingsMenu.SetActive(true);

        playerControlText();
    }
    public void OptionsBack()
    {
        // opposite of Options()
        mainPauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    // ControllerSettingsMenu
    public void Rebind1()
    {
        StartCoroutine(waitForInput("Player 1"));
    }

    public void Rebind2()
    {
        StartCoroutine(waitForInput("Player 2"));
    }

    private void currentlyRebinding(string player)
    {
        if (player == "Player 1")
        {
            rebindPlayer1.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Press interact...";
            rebindPlayer2.SetActive(false);
        }

        if (player == "Player 2")
        {
            rebindPlayer2.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Press interact...";
            rebindPlayer1.SetActive(false);
        }
    }

    private void finishedRebinding()
    {
        rebindPlayer1.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Rebind";
        rebindPlayer2.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Rebind";

        rebindPlayer1.SetActive(true);
        rebindPlayer2.SetActive(true);

        playerControlText();
    }

    public void ControllerSettingsBack()
    {
        controllerSettingsMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
}

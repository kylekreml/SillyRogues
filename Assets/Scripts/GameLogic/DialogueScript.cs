using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class DialogueScript : MonoBehaviour
{
    [SerializeField]
    float time = 0;
    [SerializeField]
    float timeBetweenDialogue = .3f;
    [SerializeField]
    bool beforeLevel = false;
    bool dialogueStarted = false;

    PauseMenuScript pause;
    PlayerInput playerInput;
    InputManager players;

    int totalDialogues;
    [SerializeField]
    int dialogueIndex = 1;

    void Awake()
    {
        pause = transform.parent.parent.parent.Find("PauseMenu").GetComponent<PauseMenuScript>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        players = transform.parent.parent.parent.Find("Players").GetComponent<InputManager>();

        totalDialogues = transform.childCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ControlManager.player1Gamepad)
            InputUser.PerformPairingWithDevice(
                Gamepad.all[ControlManager.player1GamepadIndex],
                playerInput.user
            );
        if (ControlManager.player2Gamepad)
            InputUser.PerformPairingWithDevice(
                Gamepad.all[ControlManager.player2GamepadIndex],
                playerInput.user
            );

        InputUser.PerformPairingWithDevice(
            Keyboard.current,
            playerInput.user
        );

        if (beforeLevel)
        {
            StartDialogue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueStarted)
            time += Time.unscaledDeltaTime;
        
        if (dialogueIndex == totalDialogues)
        {
            dialogueIndex++;
            CompleteDialogue();
        }
    }

    public void StartDialogue()
    {
        dialogueStarted = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(dialogueIndex).gameObject.SetActive(true);

        players.DisablePlayerInput();
        pause.DisablePlayerInput();
        pause.Pause();
    }

    void CompleteDialogue()
    {
        transform.GetChild(0).gameObject.SetActive(false);

        pause.Pause();
        players.EnablePlayerInput();
        pause.EnablePlayerInput();

        if (!beforeLevel)
        {
            transform.parent.parent.parent.Find("GameManager").GetComponent<GameManager>().StartNextLevel();
        }
    }

    public void Continue(InputAction.CallbackContext ctx)
    {
        if (dialogueIndex < totalDialogues && dialogueStarted && time > timeBetweenDialogue && ctx.started)
        {
            transform.GetChild(dialogueIndex).gameObject.SetActive(false);

            dialogueIndex++;
            time = 0;

            if (dialogueIndex < totalDialogues)
            {
                transform.GetChild(dialogueIndex).gameObject.SetActive(true);
            }
        }
    }
}

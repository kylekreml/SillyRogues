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
    float timeBetweenDialogue = 3;

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

        // for (int i = 0; i < InputSystem.devices.Count; i++)
        // {
        //     InputUser.PerformPairingWithDevice(
        //         InputSystem.devices[i],
        //         playerInput.user
        //     );
        // }
        // Debug.Log(string.Join("\n",Gamepad.all));
        // for (int i = 0; i < Gamepad.all.Count; i++)
        // {
        //     InputUser.PerformPairingWithDevice(
        //         Gamepad.all[i],
        //         playerInput.user
        //     );
        // }
        // Debug.Log(string.Join("\n",playerInput.devices));

        transform.GetChild(dialogueIndex).gameObject.SetActive(true);
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
        players.DisablePlayerInput();

        pause.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.unscaledDeltaTime;
        if (dialogueIndex == totalDialogues)
        {
            dialogueIndex++;
            Complete();
        }
    }

    void Complete()
    {
        Debug.Log("finished");
        pause.Pause();
        players.EnablePlayerInput();
    }

    public void Continue(InputAction.CallbackContext ctx)
    {
        // Time.realtimeSinceStartup >= time
        if (dialogueIndex < totalDialogues && time > timeBetweenDialogue && ctx.started)
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

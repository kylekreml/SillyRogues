using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InputManager : MonoBehaviour
{
    PlayerControlActions input;
    PlayerInput player1;
    PlayerInput player2;

    void Awake()
    {
        input = new PlayerControlActions();
        player1 = transform.Find("Player 1").GetComponent<PlayerInput>();
        player2 = transform.Find("Player 2").GetComponent<PlayerInput>();

        // Case where there are no controllers assigned
        if (ControlManager.numberOfGamepads == -1)
        {
            if (Gamepad.all.Count > 0)
            {
                ControlManager.player1GamepadIndex = 0;
                ControlManager.player1Gamepad = true;
                InputUser.PerformPairingWithDevice(
                    Gamepad.all[ControlManager.player1GamepadIndex],
                    player1.user, 
                    InputUserPairingOptions.UnpairCurrentDevicesFromUser
                );
                player1.SwitchCurrentControlScheme(player1.devices[0]);
            }

            // Debug.Log(string.Join("\n", Gamepad.all));
            ControlManager.numberOfGamepads = Gamepad.all.Count;
            if (Gamepad.all.Count > 1)
            {
                ControlManager.player2GamepadIndex = 1;
                ControlManager.player2Gamepad = true;
                InputUser.PerformPairingWithDevice(
                    Gamepad.all[ControlManager.player2GamepadIndex],
                    player2.user, 
                    InputUserPairingOptions.UnpairCurrentDevicesFromUser
                );
                player2.SwitchCurrentControlScheme(player2.devices[0]);
            }

            if (!ControlManager.player1Gamepad)
            {
                player1.SwitchCurrentControlScheme("Keyboard1");
            }

            if (!ControlManager.player2Gamepad)
            {
                player2.SwitchCurrentControlScheme("Keyboard2");
            }
        }

        // Case where there are controllers assigned
        else
        {
            if (ControlManager.player1Gamepad)
            {
                InputUser.PerformPairingWithDevice(
                    Gamepad.all[ControlManager.player1GamepadIndex],
                    player1.user,
                    InputUserPairingOptions.UnpairCurrentDevicesFromUser
                );
                player1.SwitchCurrentControlScheme("Gamepad");
            }
            else
            {
                player1.SwitchCurrentControlScheme("Keyboard1");
            }

            if (ControlManager.player2Gamepad)
            {
                InputUser.PerformPairingWithDevice(
                    Gamepad.all[ControlManager.player2GamepadIndex],
                    player2.user,
                    InputUserPairingOptions.UnpairCurrentDevicesFromUser
                );
                player2.SwitchCurrentControlScheme("Gamepad");
            }
            else
            {
                player2.SwitchCurrentControlScheme("Keyboard2");
            }
        }
    }

    // TODO: add public functions for the pause menu to be able to switch controls
    public void RebindPlayerControls(string player, InputDevice device)
    {
        if (player == "Player 1")
        {
            InputUser.PerformPairingWithDevice(
                device,
                player1.user, 
                InputUserPairingOptions.UnpairCurrentDevicesFromUser
            );
            player1.SwitchCurrentControlScheme(player1.devices[0]);

            if (player1.currentControlScheme == "Gamepad")
            {
                for (int i = 0; i < Gamepad.all.Count; i++)
                {
                    if (device == Gamepad.all[i])
                    {
                        ControlManager.player1GamepadIndex = i;
                        break;
                    }
                }
                ControlManager.player1Gamepad = true;

                if (device == player2.devices[0])
                {
                    InputUser.PerformPairingWithDevice(
                        Keyboard.current,
                        player2.user, 
                        InputUserPairingOptions.UnpairCurrentDevicesFromUser
                    );
                    player2.SwitchCurrentControlScheme(player2.devices[0]);
                    ControlManager.player2Gamepad = false;
                }
            }
        }
        else
        {
            InputUser.PerformPairingWithDevice(
                device,
                player2.user, 
                InputUserPairingOptions.UnpairCurrentDevicesFromUser
            );
            player2.SwitchCurrentControlScheme(player2.devices[0]);

            if (player2.currentControlScheme == "Gamepad")
            {
                for (int i = 0; i < Gamepad.all.Count; i++)
                {
                    if (device == Gamepad.all[i])
                    {
                        ControlManager.player2GamepadIndex = i;
                        break;
                    }
                }
                ControlManager.player2Gamepad = true;

                if (device == player1.devices[0])
                {
                    InputUser.PerformPairingWithDevice(
                        Keyboard.current,
                        player1.user, 
                        InputUserPairingOptions.UnpairCurrentDevicesFromUser
                    );
                    player1.SwitchCurrentControlScheme(player1.devices[0]);
                    ControlManager.player1Gamepad = false;
                }
            }
        }
    }

    public void EnablePlayerInput()
    {
        player1.ActivateInput();
        player2.ActivateInput();
    }

    public void DisablePlayerInput()
    {
        player1.DeactivateInput();
        player2.DeactivateInput();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }
}

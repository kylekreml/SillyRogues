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
                    player1.user
                );
            }

            // Debug.Log(string.Join("\n", Gamepad.all));
            ControlManager.numberOfGamepads = Gamepad.all.Count;
            if (Gamepad.all.Count > 1)
            {
                ControlManager.player2GamepadIndex = 1;
                ControlManager.player2Gamepad = true;
                InputUser.PerformPairingWithDevice(
                    Gamepad.all[ControlManager.player2GamepadIndex],
                    player2.user
                );
            }
        }

        // Case where there are controllers assigned
        else
        {
            if (ControlManager.player1Gamepad)
            {
                InputUser.PerformPairingWithDevice(
                    Gamepad.all[ControlManager.player1GamepadIndex],
                    player1.user
                );
            }

            if (ControlManager.player2Gamepad)
            {
                InputUser.PerformPairingWithDevice(
                    Gamepad.all[ControlManager.player2GamepadIndex],
                    player2.user
                );
            }
        }

        InputUser.PerformPairingWithDevice(
            Keyboard.current,
            player1.user
        );
        InputUser.PerformPairingWithDevice(
            Keyboard.current,
            player2.user
        );
    }

    // TODO: fix it
    public void RebindPlayerControls(string player, InputDevice device)
    {
        if (player == "Player 1")
        {
            InputUser.PerformPairingWithDevice(
                device,
                player1.user,
                InputUserPairingOptions.UnpairCurrentDevicesFromUser
            );
            InputUser.PerformPairingWithDevice(
                Keyboard.current,
                player1.user
            );

            if (player1.devices.Count > 1)
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
                
                for (int i = 0; i < player2.devices.Count; i++)
                {
                    if (device == player2.devices[i])
                    {
                        InputUser.PerformPairingWithDevice(
                            Keyboard.current,
                            player2.user, 
                            InputUserPairingOptions.UnpairCurrentDevicesFromUser
                        );
                        ControlManager.player2Gamepad = false;
                    }
                }
            }

            if (device == Keyboard.current)
            {
                InputUser.PerformPairingWithDevice(
                    Keyboard.current,
                    player1.user, 
                    InputUserPairingOptions.UnpairCurrentDevicesFromUser
                );
                ControlManager.player1Gamepad = false;
            }
        }
        else
        {
            InputUser.PerformPairingWithDevice(
                device,
                player2.user,
                InputUserPairingOptions.UnpairCurrentDevicesFromUser
            );
            InputUser.PerformPairingWithDevice(
                Keyboard.current,
                player2.user
            );

            if (player2.devices.Count > 1)
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
                
                for (int i = 0; i < player1.devices.Count; i++)
                {
                    if (device == player1.devices[i])
                    {
                        InputUser.PerformPairingWithDevice(
                            Keyboard.current,
                            player1.user, 
                            InputUserPairingOptions.UnpairCurrentDevicesFromUser
                        );
                        ControlManager.player1Gamepad = false;
                    }
                }
            }

            if (device == Keyboard.current)
            {
                InputUser.PerformPairingWithDevice(
                    Keyboard.current,
                    player2.user, 
                    InputUserPairingOptions.UnpairCurrentDevicesFromUser
                );
                ControlManager.player2Gamepad = false;
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

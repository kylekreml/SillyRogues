using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class KillMe : MonoBehaviour
{
    public float time = 5;
    public PauseMenuScript pause;
    Slider slider;
    PlayerInput playerInput;

    void Awake()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();

        InputUser.PerformPairingWithDevice(Gamepad.all[0], playerInput.user);
        InputUser.PerformPairingWithDevice(Gamepad.all[1], playerInput.user);
        playerInput.SwitchCurrentControlScheme(Gamepad.all[0]);
    }

    // Start is called before the first frame update
    void Start()
    {
        pause.Pause();
        slider = this.transform.Find("time").gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = Time.realtimeSinceStartup / time;
        if (Time.realtimeSinceStartup >= time)
        {
            // if(Input.anyKeyDown)
            // {
            //     pause.Pause();
            //     Destroy(gameObject);
            // }
        }
    }

    public void deleteIt(InputAction.CallbackContext ctx)
    {
        if (Time.realtimeSinceStartup >= time && ctx.started)
        {
            //Cannot use get any key since the new input system doesn't have it and is
            //possibly going to be added in the next version/update of the input system
            pause.Pause();
            Destroy(gameObject);
        }
    }
}

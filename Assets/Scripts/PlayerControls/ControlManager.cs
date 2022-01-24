using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ControlManager
{
    public static bool player1Gamepad {get; set;} = false;
    public static bool player2Gamepad {get; set;} = false;
    public static int numberOfGamepads {get; set;} = -1;
    public static int player1GamepadIndex {get; set;}
    public static int player2GamepadIndex {get; set;}
}

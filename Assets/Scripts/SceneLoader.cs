using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public void doExitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
    public void LoadScene(string sceneName)
    {
        Debug.Log("LOAD SCENE");
        SceneManager.LoadScene(sceneName);
    }

    public void debugButton()
    {
        Debug.Log("PRESSED");
    }

}

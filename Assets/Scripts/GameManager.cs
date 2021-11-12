using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    private static GameManager instance;

    [SerializeField]
    private int levelGold;
    [SerializeField]
    private string nextScene;
    [SerializeField]
    private bool debugChange = false;

    private GameManager()
    {
        // Initialize GameManager
        levelGold = 0;
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    void Update()
    {
        DebugSceneChange();
    }
    
    public void Pause(bool paused)
    {
        //TODO: figure out how to pause
    }

    public void ChangeGold(int gold)
    {
        levelGold += gold;
        if (levelGold <= 0)
        {
            GameOver();
        }
    }

    public int GetGold()
    {
        return levelGold;
    }

    public void SetGold(int gold)
    {
        levelGold = gold;
    }

    private void GameOver()
    {
        //TODO: gameover stuff here
        Debug.Log("GameOver not implemented :)");
    }

    public void ChangeScene(string changeScene)
    {
        Debug.Log("Change to " + changeScene);
        SceneManager.LoadScene(changeScene);
    }

    public void SetNextScene(string ns)
    {
        nextScene = ns;
    }

    public void DebugSceneChange()
    {
        if (debugChange)
            ChangeScene(nextScene);
        debugChange = false;
    }
}

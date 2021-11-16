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
    private int spawnersLeft;
    [SerializeField]
    private bool levelComplete;
    [SerializeField]
    private string nextScene;
    [SerializeField]
    private bool debugChange = true;

    private GameManager()
    {
        // Initialize GameManager
        levelGold = 0;
        spawnersLeft = 0;
        levelComplete = false;
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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(instance);
    }

    void Update()
    {
        if (levelGold > 0)
        {
            if (levelComplete && debugChange)
            {
                //Do finished level stuff
                //also reset values
                levelGold = 0;
                levelComplete = false;
                ChangeScene(nextScene);
            }
            else if (spawnersLeft == 0)
            {
                levelComplete = true;
            }
        }
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

    public void ChangeSpawnersLeft(int spawner)
    {
        spawnersLeft += spawner;
    }

    private void GameOver()
    {
        //TODO: gameover stuff here
        ChangeScene("Game Over");
    }

    public void ChangeScene(string changeScene)
    {
        SceneManager.LoadScene(changeScene);
    }

    public void SetNextScene(string ns)
    {
        nextScene = ns;
    }
}

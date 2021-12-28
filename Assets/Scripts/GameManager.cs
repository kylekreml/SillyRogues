using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    private static GameManager instance;

    [SerializeField]
    private int levelGold = 10;
    [SerializeField]
    private int spawnersLeft;
    [SerializeField]
    private bool levelComplete;
    [SerializeField]
    private string nextScene;
    [SerializeField]
    public GameObject hp;
    private void Start()
    {
        // Initialize GameManager
        levelGold = 10;
        spawnersLeft = 0;
        levelComplete = false;
    }

    void Update()
    {
        if (levelGold > 0)
        {
            if (levelComplete)
            {
                //Do finished level stuff
                hp.SetActive(false);
                StartCoroutine(SceneDelay());
            }
            else if (spawnersLeft == 0)
            {
                levelComplete = true;
            }
        }
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
        transform.parent.Find("SceneLoader").GetComponent<SceneLoader>().LoadScene("Game Over");
    }

    public void SetNextScene(string ns)
    {
        nextScene = ns;
    }

    IEnumerator SceneDelay()
    {
        yield return new WaitForSeconds(2f);
        transform.parent.Find("SceneLoader").GetComponent<SceneLoader>().LoadScene(nextScene);
    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

}

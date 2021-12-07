using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    private bool paused;
    // Start is called before the first frame update
    void Awake()
    {
        paused = false;
        Time.timeScale = 1f;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            transform.GetChild(0).gameObject.SetActive(paused);
        }
    }

    public void Pause()
    {
        if (paused)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
        paused = !paused;
    }

    public bool IsPaused()
    {
        return paused;
    }

    public void MainMenu()
    {
        //GameManager.Instance.Pause(false);
        Pause();
        GameManager.Instance.ChangeScene("Title Scene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

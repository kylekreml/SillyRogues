using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    private bool paused;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        transform.GetChild(0).gameObject.SetActive(paused);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        GameManager.Instance.Pause(true);
        paused = !paused;
        transform.GetChild(0).gameObject.SetActive(paused);
    }

    public void MainMenu()
    {
        #GameManager.Instance.Pause(false);
        GameManager.Instance.ChangeScene("Title Scene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

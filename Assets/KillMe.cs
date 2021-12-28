using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillMe : MonoBehaviour
{
    public float time = 5;
    public PauseMenuScript pause;
    Slider slider;
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
            if(Input.anyKeyDown)
            {
                pause.Pause();
                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TitleButton()
    {
        Debug.Log("button");
        // Changed how GameManager works, so this doesn't work
        // GameManager.Instance.ChangeScene("Tutorial 1");
    }
}

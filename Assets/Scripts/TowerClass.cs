using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerClass : MonoBehaviour
{


    public bool disabled = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enableTower()
    {
        disabled = false;
    }

    public void disableTower()
    {
        disabled = true;
    }

    public bool returnStatus()
    {
        return disabled;
    }
}

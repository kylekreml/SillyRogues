using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerClass : MonoBehaviour
{


    public bool disabled = false;
    public bool buffed = false;
    //public int tier = 1;
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

    public void buffTower()
    {
        buffed = true;
    }

    public void debuffTower()
    {
        buffed = false;
    }

}

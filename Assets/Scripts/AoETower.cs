using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoETower : TowerClass
{
    public float attackSpeed = 1;
    public GameObject blade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!this.disabled)
        {
            Rotate();
            this.transform.Find("pointer").GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            this.transform.Find("pointer").GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    void Rotate()
    {
        gameObject.transform.Rotate(0, 0, attackSpeed * 10);
    }
    
}

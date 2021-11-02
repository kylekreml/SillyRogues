using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusTowerBehavior : TowerClass
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.disabled)
        {
            this.transform.Find("Aoe Radius").GetComponent<SpriteRenderer>().enabled = false;
            this.transform.Find("Aoe Radius").GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
            this.transform.Find("Aoe Radius").GetComponent<SpriteRenderer>().enabled = true;
            this.transform.Find("Aoe Radius").GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}

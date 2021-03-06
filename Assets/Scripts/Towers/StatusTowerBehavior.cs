using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusTowerBehavior : TowerClass
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlaceCheck : MonoBehaviour
{
    private List<Collider2D> obstructions = new List<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Tower")
        {
            if(!obstructions.Contains(collider))
            {
                obstructions.Add(collider);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Tower")
        {
            if(obstructions.Contains(collider))
            {
                obstructions.Remove(collider);
            }
        }
    }

    public List<Collider2D> Obstructed()
    {
        return obstructions;
    }
}

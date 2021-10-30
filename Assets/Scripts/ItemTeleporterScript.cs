using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTeleporterScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO: need a way to remove the item from the player
    //IDEA: make a separate function that is public in PlayerMovement
    //      the function will still have the held variable, but will also make the held object a child of the player
    //      can easily remove parent(player) from held by setting parent to null
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Tower" || collider.gameObject.tag == "Resource")
        {

        }
    }
}

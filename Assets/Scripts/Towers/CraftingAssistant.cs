using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingAssistant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disable()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Enable()
    {

    }
}

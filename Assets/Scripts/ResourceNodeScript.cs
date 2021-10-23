using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeScript : ResourceScript
{
    [SerializeField]
    private Resource resourceType;
    [SerializeField]
    private int interactsLeft;
    [SerializeField]
    private int interactsNeeded;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private GameObject resource;
    private enum Directions{up, right, down, left};
    [SerializeField]
    Directions direction;
    [SerializeField]
    private float spawnDistance;
    // Start is called before the first frame update
    void Start()
    {
        interactsLeft = interactsNeeded;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interactResourceNode()
    {
        interactsLeft--;

        if (interactsLeft <= 0)
        {
            interactsLeft = interactsNeeded;
            spriteRenderer.color = Color.green;
            GameObject r = Instantiate(resource);
            r.GetComponent<ResourceScript>().SetType(resourceType);
            
            switch(direction)
            {
                case Directions.up:
                    r.transform.position = transform.position + new Vector3(0, spawnDistance, 0);
                    break;
                case Directions.right:
                    r.transform.position = transform.position + new Vector3(spawnDistance, 0, 0);
                    break;
                case Directions.down:
                    r.transform.position = transform.position + new Vector3(0, -spawnDistance, 0);
                    break;
                case Directions.left:
                    r.transform.position = transform.position + new Vector3(-spawnDistance, 0, 0);
                    break;
            }
        }
        else
        {
            //TEMPORARY SPRITE CHANGE
            if (interactsLeft <= 1)
            {
                spriteRenderer.color = Color.red;
            }
            else if (interactsLeft <= (interactsNeeded/2))
            {
                spriteRenderer.color = Color.yellow;
            }
        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceNodeScript : ResourceScript
{
    [SerializeField]
    private Resource resourceType;
    [SerializeField]
    private int interactsLeft;
    [SerializeField]
    private int interactsNeeded;
    [SerializeField]
    private float interactTimeLeft;
    [SerializeField]
    private float interactTimeNeeded;
    private Slider interactBar;
    private bool isHeld;
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
        interactTimeLeft = interactTimeNeeded;
        interactBar = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        interactBar.maxValue = interactTimeNeeded;
        interactBar.value = interactTimeLeft;
        isHeld = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[(int)resourceType];
        //spriteRenderer.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHeld)
            interactTimeLeft -= Time.deltaTime;
            interactBar.value = interactTimeLeft;

        handleInteractTime();
    }

    // Temporary disabled to test holding node
    public void interactResourceNode()
    {
        interactsLeft--;
    }

    public void interactTimeResourceNode(bool held)
    {
        isHeld = held;
    }

    private void handleInteractTime()
    {
        if (interactTimeLeft <= 0)
        {
            interactTimeLeft = interactTimeNeeded;
            // var temp = spriteRenderer.color;
            // temp.a = 1f;
            // spriteRenderer.color = temp;
            GameObject r = Instantiate(resource);
            r.GetComponent<ResourceScript>().SetResourceType(resourceType);

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
        // else
        // {
        //     //TEMPORARY SPRITE CHANGE
        //     if (interactTimeLeft/interactsNeeded <= .1f)
        //     {
        //         var temp = spriteRenderer.color;
        //         temp.a = 0.3f;
        //         spriteRenderer.color = temp;
        //     }
        //     else if (interactTimeLeft/interactsNeeded <= .5f)
        //     {
        //         var temp = spriteRenderer.color;
        //         temp.a = 0.7f;
        //         spriteRenderer.color = temp;
        //     }
        // }

        // spam interact logic
        // if (interactsLeft <= 0)
        // {
        //     interactsLeft = interactsNeeded;
        //     var temp = spriteRenderer.color;
        //     temp.a = 1f;
        //     spriteRenderer.color = temp;
        //     GameObject r = Instantiate(resource);
        //     r.GetComponent<ResourceScript>().SetResourceType(resourceType);
            
        //     switch(direction)
        //     {
        //         case Directions.up:
        //             r.transform.position = transform.position + new Vector3(0, spawnDistance, 0);
        //             break;
        //         case Directions.right:
        //             r.transform.position = transform.position + new Vector3(spawnDistance, 0, 0);
        //             break;
        //         case Directions.down:
        //             r.transform.position = transform.position + new Vector3(0, -spawnDistance, 0);
        //             break;
        //         case Directions.left:
        //             r.transform.position = transform.position + new Vector3(-spawnDistance, 0, 0);
        //             break;
        //     }
        // }
        // else
        // {
        //     //TEMPORARY SPRITE CHANGE
        //     if (interactsLeft <= 1)
        //     {
        //         var temp = spriteRenderer.color;
        //         temp.a = 0.3f;
        //         spriteRenderer.color = temp;
        //     }
        //     else if (interactsLeft <= (interactsNeeded/2))
        //     {
        //         var temp = spriteRenderer.color;
        //         temp.a = 0.7f;
        //         spriteRenderer.color = temp;
        //     }
        // }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemTeleporterScript : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundMap;

    [SerializeField]
    private Tilemap colMap;
    public AudioSource soundEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        teleportItem();
    }

    //TODO: need a way to remove the item from the player
    //IDEA: make a separate function that is public in PlayerMovement
    //      the function will still have the held variable, but will also make the held object a child of the player
    //      can easily remove parent(player) from held by setting parent to null
    // void OnTriggerStay2D(Collider2D collider)
    // {
    //     if (collider.gameObject.tag == "Tower" || collider.gameObject.tag == "Resource")
    //     {

    //     }
    // }

    //Using TestGridSpawn.canSpawnTower() instead, need to see if collider hits before
    //Not detecting towers
    private void teleportItem()
    {
        Vector3Int gridPosition = groundMap.WorldToCell(transform.position + new Vector3(0,1,0));
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y), new Vector2(0.5f, 0.5f),0f);

        foreach(Collider2D obj in overlaps)
        {
            if (obj.tag == "Tower")
            {
                if (gameObject.name == "Teleport1")
                {
                    obj.transform.position = transform.parent.GetChild(1).GetChild(0).position;
                }
                else
                {
                    obj.transform.position = transform.parent.GetChild(0).GetChild(0).position;
                }
                soundEffect.Play();
            }
            else if (obj.tag == "Resource" && obj.gameObject.GetComponent<ResourceScript>().GetResourceType() != Resource.Node)
            {
                if (gameObject.name == "Teleport1")
                {
                    obj.transform.position = transform.parent.GetChild(1).GetChild(0).position;
                }
                else
                {
                    obj.transform.position = transform.parent.GetChild(0).GetChild(0).position;
                }
                soundEffect.Play();
            }

        }
    }
}

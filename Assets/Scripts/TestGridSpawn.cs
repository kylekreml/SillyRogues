using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestGridSpawn : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundMap;

    [SerializeField]
    private Tilemap colMap;

    private bool canSpawn;

    public char playerNumber = (char)1;
    public GameObject tower;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float InteractInput = Input.GetAxis("Interact" + playerNumber);
        if (InteractInput != 0)
        {
            canSpawn = true;
            Vector3Int gridPosition = groundMap.WorldToCell(transform.position + new Vector3(0, 1, 0));
            Collider2D[] overlaps = Physics2D.OverlapBoxAll(new Vector2(gridPosition.x, gridPosition.y), new Vector2(0.5f, 0.5f), 0f);
            foreach (Collider2D o in overlaps)
            {
                Debug.Log(o.tag);
                if (o.tag == "Tower")
                {
                    canSpawn = false;
                }
            }
            if (canSpawn)
            {
                spawn();
            }
        }
    }
    private bool canSpawnTower()
    {
        Vector3Int gridPosition = groundMap.WorldToCell(transform.position + new Vector3(0, 1, 0));
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(new Vector2(gridPosition.x, gridPosition.y), new Vector2(0.5f, 0.5f),0f);
        foreach(Collider2D o in overlaps)
        {
            if(o.tag == "Tower")
            {
                return false;
            }
        }
        return true;
    }

    private void spawn()
    {
        Debug.Log("Spawned");
        Instantiate(tower, groundMap.WorldToCell(transform.position + new Vector3(0, 1, 0)),Quaternion.identity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCreationScript : MonoBehaviour
{
    public GameObject[] towers;
    public Transform spawnpoint;

    public GameObject resource1;
    public GameObject resource2;

    // Start is called before the first frame update
    void Start()
    {
        spawnpoint = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(resource1 != null && resource2 != null)
        {
            createTower(towerRecipes());
        }
    }

    int towerRecipes()
    {
        if ((resource1.name == "wood" && resource2.name == "metal") || (resource2.name == "metal" && resource1.name == "wood"))
            return 0;
        else if ((resource1.name == "wood" && resource2.name == "wood") || (resource2.name == "wood" && resource1.name == "wood"))
            return 1;
        else if ((resource1.name == "metal" && resource2.name == "metal") || (resource2.name == "metal" && resource1.name == "metal"))
            return 2;
        return -1;
    }

    void createTower(int tower)
    {
        if (tower == -1)
        {
            GameObject r1 = Instantiate(resource1);
            GameObject r2 = Instantiate(resource2);
            r1.transform.position = spawnpoint.position;
            r2.transform.position = spawnpoint.position;
            resource1 = null;
            resource2 = null;
        }
        else
        {
            GameObject t = Instantiate(towers[tower]);
            t.transform.position = spawnpoint.position;
            resource1 = null;
            resource2 = null;
        }
    }
}

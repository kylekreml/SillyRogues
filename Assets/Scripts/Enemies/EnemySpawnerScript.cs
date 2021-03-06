using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject route;
    public GameObject destination;
    public float spawnDelay;
    public float spawnCooldown;

    public GameObject spawnChild;

    // Start is called before the first frame update
    void Start()
    {
        if (destination == null)
            destination = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnDelay > 0)
            spawnDelay -= Time.deltaTime;

        if(spawnDelay <= 0)
        {
            GameObject child = Instantiate(spawnChild);
            child.GetComponent<BasicEnemyScript>().SetRoute(route);
            child.GetComponent<BasicEnemyScript>().SetDestination(destination);
            child.transform.position = transform.position;
            spawnDelay = spawnCooldown;
        }
    }
}

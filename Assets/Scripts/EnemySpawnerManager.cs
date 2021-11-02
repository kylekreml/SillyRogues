using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnGroup
    {
        public float spawnTime; //seconds into the level.
        public float spawnDelay;
        [HideInInspector] public float curSpawnDelay;
        public GameObject enemy;
        public int numberOfEnemies;
        public Transform spawnPoint;
    }

    public List<SpawnGroup> groups;
    public GameObject route;
    public GameObject destination;
    public float timeSinceStart;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceStart = 0;
        if (destination == null)
            destination = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime;
        for (int i = 0; i < groups.Count; i++)
        {
            SpawnGroup g = groups[i];
            if (g.spawnTime > timeSinceStart || g.numberOfEnemies == 0)
                continue;
            if (g.curSpawnDelay > 0)
                g.curSpawnDelay -= Time.deltaTime;

            if (g.curSpawnDelay <= 0)
            {
                GameObject child = Instantiate(g.enemy);
                child.GetComponent<BasicEnemyScript>().SetRoute(route);
                child.GetComponent<BasicEnemyScript>().SetDestination(destination);
                child.transform.position = g.spawnPoint.position;
                g.curSpawnDelay = g.spawnDelay;
                g.numberOfEnemies -= 1;
            }
        }
    }
}
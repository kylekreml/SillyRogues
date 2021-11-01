using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnGroup
    {
        public float spawnTime;
        public float spawnDelay;
        public GameObject enemy;
        public int numberOfEnemies;
    }

    public List<GameObject> spawners;
    public List<SpawnGroup> groups;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

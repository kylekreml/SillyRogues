using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomResourceSpawner : MonoBehaviour
{
    [SerializeField]
    private float minX = -13;
    [SerializeField]
    private float minY = -7;
    [SerializeField]
    private float maxX = 13;
    [SerializeField]
    private float maxY = 7;
    private Vector2 randomPos;
    private float x;
    private float y;
    //wood < stone < metal < crystal. It's a random engine that chooses based on where the random generator falls
    [SerializeField]
    private float woodSpawnRate = 0.4f;
    [SerializeField]
    private float stoneSpawnRate = 0.7f;
    [SerializeField]
    private float metalSpawnRate = 0.9f;
    private float chance;
    [SerializeField]
    private GameObject woodPrefab;
    [SerializeField]
    private GameObject stonePrefab;
    [SerializeField]
    private GameObject metalPrefab;
    [SerializeField]
    private GameObject crystalPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnResources", 3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void SpawnResources()
    {
        x = UnityEngine.Random.Range(minX, maxX);
        y = UnityEngine.Random.Range(minY, maxY);
        randomPos = new Vector2(x, y);
        while ((Physics.CheckSphere(randomPos, 1)))
        {
            x = UnityEngine.Random.Range(minX, maxX);
            y = UnityEngine.Random.Range(minY, maxY);
            randomPos = new Vector2(x, y);
        }
        chance = UnityEngine.Random.Range(0f,1f);
        if (chance <= woodSpawnRate)
        {
            Instantiate(woodPrefab, randomPos, Quaternion.identity);
        }
        else if (chance <= stoneSpawnRate)
        {
            Instantiate(stonePrefab, randomPos, Quaternion.identity);
        }
        else if (chance <= metalSpawnRate)
        {
            Instantiate(metalPrefab, randomPos, Quaternion.identity);
        }
        else
        {
            Instantiate(crystalPrefab, randomPos, Quaternion.identity);
        }
    }
}

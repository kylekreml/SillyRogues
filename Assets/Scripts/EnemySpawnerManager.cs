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
        public GameObject enemy;
        public int numberOfEnemies;
        public Transform spawnPoint;
        public GameObject route;
        public GameObject destination;
    }

    public List<SpawnGroup> groups;
    public float timeSinceStart;
    private int wavesLeft;
    private int enemiesLeft;
    private bool doneSpawning;
    [SerializeField] private float warningTime;
    public AudioSource wave;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceStart = 0;
        wavesLeft = groups.Count;
        enemiesLeft = 0;
        doneSpawning = false;
        gameManager = transform.parent.GetComponent<GameManager>();
        gameManager.ChangeSpawnersLeft(1);
        for (int i = 0; i < groups.Count; i++)
        {
            SpawnGroup g = groups[i];
            if (g.destination == null) g.destination = gameObject;
            StartCoroutine(SpawnWave(g));
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime; //left because possibly could use later?

        if (!doneSpawning)
        {
            if (wavesLeft == 0 && enemiesLeft <= 0)
            {
                doneSpawning = true;
                gameManager.ChangeSpawnersLeft(-1);
            }
        }
    }

    IEnumerator SpawnWave(SpawnGroup g)
    {
        enemiesLeft += g.numberOfEnemies;
        SpriteRenderer sr = g.spawnPoint.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer enemyRenderer = g.enemy.GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(g.spawnTime - warningTime);
        if (sr != null)
        {
            sr.sprite = enemyRenderer.sprite;
            wave.Play();
            Color newColor = enemyRenderer.color;
            newColor.a = .8f;
            sr.color = newColor;
            g.spawnPoint.GetChild(0).GetChild(0).GetComponent<TextMesh>().text = "x " + g.numberOfEnemies;
            sr.enabled = true;
            g.spawnPoint.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        }
        yield return new WaitForSeconds(warningTime);
        if (sr != null) 
        {
            sr.enabled = false;
            g.spawnPoint.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
        for (int i = 0; i < g.numberOfEnemies; i++)
        {
            GameObject child = Instantiate(g.enemy);
            child.GetComponent<BasicEnemyScript>().SetRoute(g.route);
            child.GetComponent<BasicEnemyScript>().SetDestination(g.destination);
            child.GetComponent<BasicEnemyScript>().SetSpawnerScript(this);
            child.GetComponent<BasicEnemyScript>().SetGameManager(gameManager);
            child.transform.position = g.spawnPoint.position;
            yield return new WaitForSeconds(g.spawnDelay);
        }
        wavesLeft--;
    }

    public void RemovedEnemy()
    {
        enemiesLeft--;
    }
}

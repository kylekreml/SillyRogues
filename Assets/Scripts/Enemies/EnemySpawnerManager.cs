using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private List<GameObject> conveyorItems;
    [SerializeField] private GameObject conveyorTile;
    [SerializeField] private GameObject conveyor;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceStart = 0;
        wavesLeft = groups.Count;
        enemiesLeft = 0;
        doneSpawning = false;
        gameManager = transform.parent.GetComponent<GameManager>();
        gameManager.ChangeSpawnersLeft(1);
        conveyorItems = new List<GameObject>();
        for (int i = 0; i < groups.Count; i++)
        {
            SpawnGroup g = groups[i];
            if (g.destination == null) g.destination = gameObject;
            //Creates objects that move along the conveyor
            GameObject newItem = Instantiate(conveyorTile);
            newItem.transform.SetParent(conveyor.transform, false);
            newItem.GetComponent<Image>().sprite = g.enemy.GetComponent<SpriteRenderer>().sprite;
            newItem.GetComponent<Image>().color = g.enemy.GetComponent<SpriteRenderer>().color;
            newItem.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            newItem.transform.Translate(0, -g.spawnTime/0.025f, 0);
            //newItem.transform.GetChild(0).GetComponent<Text>().text = "x " + g.numberOfEnemies;
            conveyorItems.Add(newItem);
            
            StartCoroutine(SpawnWave(g));
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime; //left because possibly could use later?

        updateConveyor();

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

    private void updateConveyor()
    {
        for (int g = 0; g < groups.Count; g++)
        {
            conveyorItems[g].transform.localPosition = new Vector3((timeSinceStart-groups[g].spawnTime)/0.025f, 0f, 0f);
            if (timeSinceStart - groups[g].spawnTime >= 0)
            {
                conveyorItems[g].GetComponent<Image>().enabled = false;
            }
        }
    }
}

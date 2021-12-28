using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyScript : MonoBehaviour
{
    public float health = 5;
    private EnemySpawnerManager spawnerScript;
    private float speed;
    public float defaultSpeed = 3;

    public string type;
    public Slider healthSlider;

    [SerializeField]
    private GameObject destination;
    [SerializeField]
    private GameObject route;
    private Animator animator;
    private Transform[] waypoints;
    private int currentWaypoint;
    private Transform walkTowards;
    private int previousWaypoint;

    [SerializeField]
    private GameObject loot;
    private bool holdingLoot = false;
    private bool removeable = true;
    private SpriteRenderer enemySprite;

    private float timer = 0; 

    public HashSet<GameObject> towers = new HashSet<GameObject>();
    private GameManager gameManager;


    //for walking to exit destination when done with waypoints
    private bool walkToExit = false;
    private bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        enemySprite = this.GetComponent<SpriteRenderer>();
        speed = defaultSpeed;
        if (waypoints == null)
        {
            waypoints = new Transform[route.transform.childCount];
            for (int i = 0; i < route.transform.childCount; i++)
            {
                waypoints[i] = route.transform.GetChild(i);
            }
        }

        healthSlider = this.gameObject.transform.Find("enemyCanvas").GetChild(0).GetComponent<Slider>();
        healthSlider.maxValue = health;
        healthSlider.value = health;
        animator = this.GetComponent<Animator>();
        animator.SetFloat("Speed", speed);
    }

    // Update is called once per frame
    void Update()
    {
        //check if frozen due to line tower
        if (timer <= 0 && !dead)
        {
            walk();
        }
        else
        {
            timer -= Time.deltaTime;
        }

        
        //destroys this instance when it reaches the destination
        if (transform.position == destination.transform.position && walkToExit)
        {
            gameManager.ChangeGold(-1);
            spawnerScript.RemovedEnemy();
            Destroy(gameObject);
        }
    }

    void walk()
    {
        if (walkTowards == null)
            walkTowards = waypoints[currentWaypoint];
        Vector2 direction = Vector2.MoveTowards(transform.position, walkTowards.position, speed * Time.deltaTime);
        animator.SetFloat("Horizontal", transform.position.x - direction.x);
        animator.SetFloat("Vertical", transform.position.y - direction.y);
        transform.position = Vector2.MoveTowards(transform.position, walkTowards.position, speed * Time.deltaTime);

        if (transform.position == walkTowards.position)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                walkToExit = true;
                walkTowards = destination.transform;
            }
            else
            {
                previousWaypoint = currentWaypoint;
                walkTowards = waypoints[currentWaypoint];
            }
        }
    }

    public void damage(float amount, GameObject shooter)
    {
        health = health - amount;
        healthSlider.value = health;
        //Debug.Log(shooter.name);
        towers.Add(shooter);
        if (health <= 0 && removeable)
        {
            animator.SetBool("Dead", true);
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            removeable = false;
            spawnerScript.RemovedEnemy();
            if (holdingLoot)
            {
                holdingLoot = false;
                GameObject l = Instantiate(loot);
                l.transform.position = transform.position;
            }
            foreach (GameObject tower in towers)
            {
                tower.GetComponent<TowerClass>().giveXp();
                //Debug.Log(tower);
            }
            StartCoroutine(deathAnimation());
        }
    }

    public void changeSpeed(float newSpeed)
    {
        //Call this function to change speed of enemy movement
        speed = newSpeed;
    }

    public float getSpeed()
    {
        //Call this funciton to get the speed of the enemy to do movement calculations
        return defaultSpeed;
    }

    public void resetSpeed()
    {
        speed = defaultSpeed;
    }

    void OnTriggerEnter2D(Collider2D collide)
    {
        // Debug.Log(collide.gameObject.tag);
        if (holdingLoot == false && collide.gameObject.tag == "Loot")
        {
            Destroy(collide.gameObject);
            holdingLoot = true;
        }
    }

    //Called by TreasureCart. Checks if enemy is holding loot.
    //If not holding loot, give loot and return true. Else return false
    public bool giveLoot()
    {
        if (holdingLoot == false)
        {
            holdingLoot = true;
            return true;
        }
        return false;
    }

    //Called by EnemySpawner. Setter for the route the enemy walks
    public void SetRoute(GameObject r)
    {
        route = r;
        if (waypoints == null)
        {
            waypoints = new Transform[route.transform.childCount];
            for (int i = 0; i < route.transform.childCount; i++)
            {
                waypoints[i] = route.transform.GetChild(i);
            }
        }
    }

    //Called by EnemySpawner. Setter for the last waypoint before it can delete itself
    public void SetDestination(GameObject d)
    {
        destination = d;
    }

    //Called by EnemySpawner. Setter to find the script and keep count of enemies left in the scene
    public void SetSpawnerScript(EnemySpawnerManager sScript)
    {
        spawnerScript = sScript;
    }

    //Calculates distance between waypoints and returns a float of the remaining distance
    public float RemainingDistance()
    {
        int cwaypoint = currentWaypoint;

        float ret = 0f;
        if (cwaypoint < waypoints.Length)
        {
            ret = Vector3.Distance(transform.position, waypoints[cwaypoint].position);

            cwaypoint++;
            
            while (cwaypoint < waypoints.Length)
            {
                ret += Vector3.Distance(waypoints[cwaypoint-1].position, waypoints[cwaypoint].position);
                cwaypoint++;
            }

            ret += Vector3.Distance(waypoints[cwaypoint-1].position, destination.transform.position);
        }
        else
            ret += Vector3.Distance(transform.position, destination.transform.position);
        
        return ret;
    }


    public void freeze(float duration)
    {
        timer = duration;
    }

    public void SetGameManager(GameManager gm)
    {
        gameManager = gm;
    }

    IEnumerator deathAnimation()
    {
        dead = true;
        Color eColor = enemySprite.color;
        float temp;
        for(int i = 0; i< 10; i++)
        {
            temp = eColor.a - 0.1f;
            eColor.a = temp;
            enemySprite.color = eColor;
            yield return new WaitForSeconds(.1f);
        }
        Destroy(gameObject);
    }
}

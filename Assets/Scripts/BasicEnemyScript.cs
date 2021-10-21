using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour
{
    public float health = 5;
    private float speed;
    //not sure if this is how i'm doing it yet
    public float defaultSpeed = 3;

    public string type;

    [SerializeField]
    private GameObject destination;
    [SerializeField]
    private GameObject route;
    private Transform[] waypoints;
    private int currentWaypoint;
    private Transform walkTowards;
    private int previousWaypoint;

    [SerializeField]
    private GameObject loot;
    private bool holdingLoot = false;

    //for walking to exit destination when done with waypoints
    private bool walkToExit = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = defaultSpeed;
        waypoints = new Transform[route.transform.childCount];
        for (int i = 0; i < route.transform.childCount; i++)
        {
            waypoints[i] = route.transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        walk();
        
        //destroys this instance when it reaches the destination
        if (transform.position == destination.transform.position && walkToExit)
        {
            Destroy(gameObject);
        }
    }

    void walk()
    {
        if (walkTowards == null)
            walkTowards = waypoints[currentWaypoint];

        transform.position = Vector2.MoveTowards(transform.position, walkTowards.position, speed * Time.deltaTime);

        //Rotation of enemy
        Vector3 vectorToTarget = walkTowards.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
        Debug.DrawRay(transform.position, transform.up, Color.red);

        if (transform.position == walkTowards.position)
        {
            currentWaypoint++;
            //TODO: closest enemy walks to dropped loot
            //REMINDER OF LOOT PICKUP PROCESS
            //WAYPOINT WILL TELL CLOSEST ENEMY THERE IS LOOT AND REDIRECT THEM
            //OR DYING ENEMY TELLS CLOSEST ENEMY
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

    public void damage(float amount)
    {
        health = health - amount;
        if (health <= 0)
        {
            if(holdingLoot)
            {
                GameObject l = Instantiate(loot);
                l.transform.position = transform.position;
            }
            Destroy(gameObject);
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
        return speed;
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

    public bool giveLoot()
    {
        if (holdingLoot == false)
        {
            holdingLoot = true;
            return true;
        }
        return false;
    }

    public void SetRoute(GameObject r)
    {
        route = r;
    }

    public void SetDestination(GameObject d)
    {
        destination = d;
    }

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
}

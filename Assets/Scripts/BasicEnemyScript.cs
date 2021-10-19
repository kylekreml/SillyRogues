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

    public GameObject spawnpoint;
    public GameObject route;
    public Transform[] waypoints;
    public int currentWaypoint;
    public Transform walkTowards;
    public int previousWaypoint;

    //for movement walkToLoot towards loot pile or spawn
    public bool walkToLoot = true;
    //for if kill when in spawn
    public bool hasLoot = false;

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
        if (currentWaypoint < this.waypoints.Length)
        {
            if (walkTowards == null)
                walkTowards = waypoints[currentWaypoint];
            walk();
        }

        //TEMPORARY KILL
        if (transform.position == spawnpoint.transform.position && hasLoot)
        {
            Destroy(gameObject);
        }
    }

    void walk()
    {

        transform.position = Vector2.MoveTowards(transform.position, walkTowards.position, speed * Time.deltaTime);

        Vector3 vectorToTarget = walkTowards.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        // transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        transform.rotation = q;
        Debug.DrawRay(transform.position, transform.up, Color.red);

        if (transform.position == walkTowards.position)
        {
            //will need to change for loot and return to spawn
            //REMINDER OF LOOT PICKUP PROCESS
            //WAYPOINT WILL TELL CLOSEST ENEMY THERE IS LOOT AND REDIRECT THEM
            if (!(currentWaypoint < this.waypoints.Length - 1))
            {
                walkToLoot = !walkToLoot;
                //TEMPORARY KILL
                hasLoot = true;
            }

            if (walkToLoot)
                currentWaypoint++;
            else if (currentWaypoint > -1)
                currentWaypoint--;

            if (currentWaypoint > -1)
                walkTowards = waypoints[currentWaypoint];
            else
                walkTowards = spawnpoint.transform;
        }


     


    }
    public void damage(float amount)
    {
        health = health - amount;
        if (health <= 0)
        {
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour
{
    public int health;
    public float speed;
    //not sure if this is how i'm doing it yet
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
        waypoints = new Transform[route.transform.childCount];
        for(int i = 0; i < route.transform.childCount; i++)
        {
            waypoints[i] = route.transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWaypoint < this.waypoints.Length)
        {
            if(walkTowards == null)
                walkTowards = waypoints[currentWaypoint];
            walk();
        }

        //TEMPORARY KILL
        if(transform.position == spawnpoint.transform.position && hasLoot)
        {
            Debug.Log("home");
            Destroy(gameObject);
        }
    }

    void walk()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, walkTowards.position, speed*Time.deltaTime);

        if(transform.position == walkTowards.position)
        {
            //will need to change for loot and return to spawn
            //REMINDER OF LOOT PICKUP PROCESS
            //WAYPOINT WILL TELL CLOSEST ENEMY THERE IS LOOT AND REDIRECT THEM
            if(!(currentWaypoint < this.waypoints.Length-1))
            {
                walkToLoot = !walkToLoot;
                //TEMPORARY KILL
                hasLoot = true;
            }

            if(walkToLoot)
                currentWaypoint++;
            else if(currentWaypoint > -1)
                currentWaypoint--;
            
            if(currentWaypoint > -1)
                walkTowards = waypoints[currentWaypoint];
            else
                walkTowards = spawnpoint.transform;
        }
    }

    void damage(string damageType, int amount)
    {
        
    }
}

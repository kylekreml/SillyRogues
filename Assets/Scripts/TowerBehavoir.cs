using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavoir : MonoBehaviour
{

    public float bulletRespawn = 2f;

    private float timer;

    public GameObject bullet;

    public int radius;

    public GameObject target = null;

    float targetDistance = 1000000;


    // Start is called before the first frame update
    void Start()
    {
        timer = bulletRespawn;
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (timer <= 0)
            {
                Vector3 vectorToTarget = collision.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                //transform.rotation = Quaternion.Slerp(transform.rotation, q, 10f);
                transform.rotation = q;


                GameObject newBullet = Instantiate(bullet, this.gameObject.transform.GetChild(0).position, Quaternion.identity);
                newBullet.GetComponent<ProjectileBehavoir>().setTarget(collision.gameObject);
                timer = bulletRespawn;
            }
            else
                timer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (timer <= 0)
            {
                Vector3 vectorToTarget = collision.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                //transform.rotation = Quaternion.Slerp(transform.rotation, q, 10f);
                transform.rotation = q;

                GameObject newBullet = Instantiate(bullet, this.gameObject.transform.GetChild(0).position, Quaternion.identity);
                newBullet.GetComponent<ProjectileBehavoir>().setTarget(collision.gameObject);
                timer = bulletRespawn;
            }
            else
                timer -= Time.deltaTime;
        }
    }
    */


    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {

            if (target == null)
            {
                //only used on first enemy encountered
                Collider2D[] objects = Physics2D.OverlapCircleAll(this.transform.position, radius);

                if (objects.Length != 0)
                {
                    for (int i = 0; i < objects.Length; i++)
                    {
                        if (objects[i].gameObject.CompareTag("Enemy") && objects[i].gameObject.GetComponent<BasicEnemyScript>().RemainingDistance() < targetDistance)
                        {
                            target = objects[i].gameObject;
                            targetDistance = objects[i].gameObject.GetComponent<BasicEnemyScript>().RemainingDistance();
                        }
                    }
                }

                if (target)
                {
                    Vector3 vectorToTarget = target.transform.position - transform.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = q;
                    GameObject newBullet = Instantiate(bullet, this.gameObject.transform.GetChild(0).position, Quaternion.identity);
                    newBullet.GetComponent<ProjectileBehavoir>().setTarget(target);
                    timer = bulletRespawn;

                }

            }

            else
            { //if timer is <= 0 AND target != null
                if (Vector3.Distance(target.transform.position, this.transform.position) < radius)
                //enemy out of range
                { 

                        Vector3 vectorToTarget = target.transform.position - transform.position;
                        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                        transform.rotation = q;
                        GameObject newBullet = Instantiate(bullet, this.gameObject.transform.GetChild(0).position, Quaternion.identity);
                        newBullet.GetComponent<ProjectileBehavoir>().setTarget(target);
                        timer = bulletRespawn;
                    
                }
                else
                { //target out of range
                    target = null;
                    targetDistance = 1000000;

                    Collider2D[] objects = Physics2D.OverlapCircleAll(this.transform.position, radius);

                    if (objects.Length != 0)
                    {
                        for (int i = 0; i < objects.Length; i++)
                        {
                            if (objects[i].gameObject.CompareTag("Enemy") && objects[i].gameObject.GetComponent<BasicEnemyScript>().RemainingDistance() < targetDistance)
                            {
                                target = objects[i].gameObject;
                                targetDistance = objects[i].gameObject.GetComponent<BasicEnemyScript>().RemainingDistance();
                            }
                        }
                    }

                    if (target)
                    {
                        Vector3 vectorToTarget = target.transform.position - transform.position;
                        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                        transform.rotation = q;
                        GameObject newBullet = Instantiate(bullet, this.gameObject.transform.GetChild(0).position, Quaternion.identity);
                        newBullet.GetComponent<ProjectileBehavoir>().setTarget(target);
                        timer = bulletRespawn;

                    }
                }
            }


        }
        else 
        {
            timer -= Time.deltaTime;
        }







    }
}

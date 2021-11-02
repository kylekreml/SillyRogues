using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerBehavior : TowerClass
{

    public float bulletRespawn = 2f;

    private float timer;

    public GameObject bullet;

    public int radius;

    GameObject target = null;

    float targetDistance = 1000000;

    public LineRenderer circleRenderer;

    public float buffMultiplier = .90f;

    // Start is called before the first frame update
    void Start()
    {
        timer = bulletRespawn;
    }


    // Update is called once per frame
    void Update()
    {
        if (!this.disabled) {
            if (timer <= 0)
            {
                target = null;
                targetDistance = 1000000;
                Collider2D[] objects = Physics2D.OverlapCircleAll(this.transform.position, radius);
                if (objects.Length != 0)
                {
                    setTarget(objects);
                    if (target != null)
                    {
                        shootTarget();
                    }
                }

            }
            else
            {
                timer -= Time.deltaTime;
            }
            if (this.transform.Find("Circle").GetComponent<SpriteRenderer>().enabled)
            {
                this.transform.Find("Circle").GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else 
        {
            if (!this.transform.Find("Circle").GetComponent<SpriteRenderer>().enabled)
            {
                this.transform.Find("Circle").GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
    void setTarget(Collider2D[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].gameObject.CompareTag("Enemy") && objects[i].gameObject.GetComponent<BasicEnemyScript>().RemainingDistance() < targetDistance && Vector3.Distance(objects[i].transform.position, this.transform.position) < radius)
            {
                target = objects[i].gameObject;
                targetDistance = objects[i].gameObject.GetComponent<BasicEnemyScript>().RemainingDistance();
            }
        }

    }
    void shootTarget()
    {
        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
        GameObject newBullet = Instantiate(bullet, this.gameObject.transform.GetChild(0).position, Quaternion.identity);
        newBullet.GetComponent<ProjectileBehavoir>().setTarget(target);
        if (buffed)
            timer = bulletRespawn - (bulletRespawn * buffMultiplier);
        else
            timer = bulletRespawn;
    }



}
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

    private SpriteRenderer radiusCircle;

    public enum type { Basic, Dom, Line }
    public type TowerType;

    // Start is called before the first frame update
    void Start()
    {
        timer = bulletRespawn;
        radiusCircle = this.transform.Find("Circle").GetComponent<SpriteRenderer>();
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
            if (radiusCircle.enabled)
            {
                radiusCircle.enabled = false;
            }
        }
        else 
        {
            if (!radiusCircle.enabled)
            {
                radiusCircle.enabled = true;
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
        newBullet.GetComponent<ProjectileClass>().setTarget(target);
        newBullet.GetComponent<ProjectileClass>().setShooter(this.gameObject);
        newBullet.transform.rotation = q;

        if (buffed)
            timer = bulletRespawn - (bulletRespawn * buffMultiplier);
        else
            timer = bulletRespawn;
    }

    public override void upgradeTower()
    {
        base.upgradeTower();
        if (TowerType == type.Basic)
        {
            if (tier == 1)
            { // TOWER JUST UPGRADED TO TIER ONE SO UPGRADE SPEED CHANGE AT FUTURE TIME
                bulletRespawn = 1.5f;
            }
        }
        else if (TowerType == type.Dom)
        {
            if (tier == 1)
            {
                bulletRespawn = 2.5f;
            }

        }
    }



}
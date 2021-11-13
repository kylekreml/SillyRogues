using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileBehavoir : ProjectileClass
{
    // Start is called before the first frame update

    public float damage = 1;
    public float speed = 1f;


    public enum type { Basic, Dom }
    private type bulletType;

    private int shooterTier;

    private void Start()
    {
        shooterTier = shooter.GetComponent<TowerBehavior>().getTier();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    Destroy(gameObject); //Change this to do damage later or something
        // }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (bulletType == type.Basic)
            {// Tower that Shot the bullet is basic tower
                if (shooterTier >= 1)
                {
                    damage = damage * 2; //CHANGE THIS TO DIFFERENT MULTIPLIER LATER 
                }
                other.gameObject.GetComponent<BasicEnemyScript>().damage(damage, shooter);
                Destroy(gameObject);
            }
            else if (bulletType == type.Dom)
            { // tower that shot bullet is Dom tower
                if (shooterTier == 2)
                {

                    //overlap circle for all enemies in a radius of the enemy hit CHANGE RADIUS SIZE AFTER BALANCING
                    Collider2D[] objects = Physics2D.OverlapCircleAll(this.transform.position, 3f);
                    for (int i = 0; i < objects.Length; i++)
                    {
                        if (objects[i].gameObject.CompareTag("Enemy") && Vector3.Distance(objects[i].transform.position, this.transform.position) < 3f)
                        {
                            objects[i].gameObject.GetComponent<BasicEnemyScript>().damage(damage, shooter);
                            
                        }
                    }

                }
                else
                {
                    other.gameObject.GetComponent<BasicEnemyScript>().damage(damage, shooter);
                }
                Destroy(gameObject);
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            this.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, speed);
            Vector3 vectorToTarget = target.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void setType(type t)
    {
        bulletType = t;
    }

    

}

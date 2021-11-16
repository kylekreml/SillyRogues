using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineProjectile : ProjectileClass
{
    // Start is called before the first frame update

    public float damage = 1;
    public float speed = 1f;
    public float life;
    Vector2 position;
    Vector2 newvector;
    Rigidbody2D rb;
    float timer = .25f;

    private int shooterTier;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        shooterTier = shooter.GetComponent<TowerBehavior>().getTier();
    }

    public override void setTarget(GameObject t)
    {
        target = t;
        position = target.transform.position - this.transform.position;
        position = position.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<BasicEnemyScript>().damage(damage, shooter);
            if (shooterTier >= 1)
            {// If the shooter tower is tier 1 or above, freeze the enemy for .5 seconds
                other.gameObject.GetComponent<BasicEnemyScript>().freeze(.5f);
            }
            if (shooterTier == 2)
            {// If the shooter tower is tier 2, double the damage CHANGE FOR BALANCING LATER
                damage = damage * 2;
            }
            other.gameObject.GetComponent<BasicEnemyScript>().damage(damage, shooter);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            //this.transform.position = Vector3.MoveTowards(this.transform.position, position, speed);
            this.GetComponent<Rigidbody2D>().AddForce(position);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
            {
            Destroy(gameObject);
        }
        
    }
}

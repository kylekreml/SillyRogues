using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineProjectile : ProjectileClass
{
    // Start is called before the first frame update

    public float damage = 1;
    public float speed = .5f;
    public float life;
    Vector2 position;
    Vector2 newvector;
    Rigidbody2D rb;
    float timer = .25f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    public override void setTarget(GameObject t)
    {
        target = t;
        position = target.transform.position - this.transform.position;
        position = position.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<BasicEnemyScript>().damage(damage);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, position, speed);
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
            {
            Destroy(gameObject);
        }
    }
}

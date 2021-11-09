using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavoir : ProjectileClass
{
    // Start is called before the first frame update

    public float damage = 1;
    public float speed = 1f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    Destroy(gameObject); //Change this to do damage later or something
        // }
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<BasicEnemyScript>().damage(damage);
            Destroy(gameObject);
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
}

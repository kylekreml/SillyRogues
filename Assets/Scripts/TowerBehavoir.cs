using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavoir : MonoBehaviour
{

    public float bulletRespawn = 2f;
    

    public GameObject bullet;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
            {
            if (bulletRespawn <= 0)
            {
                Vector3 vectorToTarget = collision.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg-90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                //transform.rotation = Quaternion.Slerp(transform.rotation, q, 10f);
                transform.rotation = q;


                GameObject newBullet = Instantiate(bullet, this.gameObject.transform.GetChild(0).position, Quaternion.identity);
                newBullet.GetComponent<ProjectileBehavoir>().setTarget(collision.gameObject);
                bulletRespawn = 2f;
            }
            else
                bulletRespawn -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (bulletRespawn <= 0)
            {
                Vector3 vectorToTarget = collision.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg-90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                //transform.rotation = Quaternion.Slerp(transform.rotation, q, 10f);
                transform.rotation = q;

                GameObject newBullet = Instantiate(bullet, this.gameObject.transform.GetChild(0).position, Quaternion.identity);
                newBullet.GetComponent<ProjectileBehavoir>().setTarget(collision.gameObject);
                bulletRespawn = 2f;
            }
            else
                bulletRespawn -= Time.deltaTime;
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}

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

    public float targetDistance = 1000000;

    public LineRenderer circleRenderer;


    // Start is called before the first frame update
    void Start()
    {
        timer = bulletRespawn;
        DrawCircle(100, radius);
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<BasicEnemyScript>().RemainingDistance();
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
    }*/



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
                    setTarget(objects);
                    if (target != null)
                    {
                        shootTarget();
                    }
                }

                

            }

            else
            { //if timer is <= 0 AND target != null
                if (Vector3.Distance(target.transform.position, this.transform.position) < radius)
                //enemy in range
                {
                    shootTarget();
                }
                else
                { //target out of range
                    target = null;
                    targetDistance = 1000000;

                    Collider2D[] objects = Physics2D.OverlapCircleAll(this.transform.position, radius);

                    if (objects.Length != 0)
                    {
                        setTarget(objects);
                    }

                    if (target != null)
                    {
                        shootTarget();

                    }
                }
            }
        

        }
        else 
        {
            timer -= Time.deltaTime;
        }
    }
    void setTarget(Collider2D[] objects)
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
    void shootTarget()
    {
        Vector3 vectorToTarget = target.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
        GameObject newBullet = Instantiate(bullet, this.gameObject.transform.GetChild(0).position, Quaternion.identity);
        newBullet.GetComponent<ProjectileBehavoir>().setTarget(target);
        timer = bulletRespawn;
    }

    void DrawCircle(int steps, float radius)
    {
        circleRenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / (steps - 1);

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = radius * xScaled;
            float y = radius * yScaled;
            float z = 0;

            Vector3 currentPosition = new Vector3(x, y, z);

            circleRenderer.SetPosition(currentStep, currentPosition);
        }
    }

}
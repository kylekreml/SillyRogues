using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavoir : MonoBehaviour
{
    // Start is called before the first frame update

    public float damage = 1;
    public GameObject target;

    void Start()
    {
        
    }

    public void setTarget (GameObject t)
    {
        target = t;
    }

    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    Destroy(gameObject); //Change this to do damage later or something
        // }
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<BasicEnemyScript>().damage(damage);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(gameObject.transform.position, target.transform.position, .10f);
    }
}

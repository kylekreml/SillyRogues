using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTowerBehavoir : MonoBehaviour
{

    public float slowPercentage = .30f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float currentSpeed = collision.gameObject.GetComponent<BasicEnemyScript>().getSpeed();
            float newSpeed = currentSpeed - currentSpeed * slowPercentage;
            collision.gameObject.GetComponent<BasicEnemyScript>().changeSpeed(newSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<BasicEnemyScript>().resetSpeed();
        }
    }


}

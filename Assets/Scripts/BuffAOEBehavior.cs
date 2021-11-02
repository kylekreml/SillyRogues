using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAOEBehavior : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.GetComponent<TowerClass>().buffTower();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            collision.GetComponent<TowerClass>().debuffTower();
        }
    }


}

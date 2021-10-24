using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoETower : MonoBehaviour
{
    public float attackSpeed = 1;
    public GameObject blade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
    }
    void Rotate()
    {
        gameObject.transform.Rotate(0, 0, attackSpeed * 10);
    }
    
}

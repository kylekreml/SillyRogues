using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;

    public char playerNumber = (char)1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal" + playerNumber);
        float verticalInput = Input.GetAxis("Vertical" + playerNumber);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

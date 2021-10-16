using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;

    public Collider2D held = null;
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
        CheckInteract();
        CheckHeld();
    }

    private void CheckInteract()
    {
        if(Input.GetButtonDown("Interact" + playerNumber))
        {
            if (held != null)
            {
                held = null;
                return;
            }
            Collider2D hit = Physics2D.OverlapCircle(this.transform.position, 1.5f);
            if (hit != null && hit.tag == "Tower")
            {
                held = hit;
            }
        }
    }

    private void CheckHeld()
    {
        if(held == null)
        {
            return;
        }
        held.transform.position = this.transform.position + new Vector3(0, 1, 0);
    }
}
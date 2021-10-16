using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustinPlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float pickupRange = 3f;

    public Collider2D held = null;
    public char playerNumber = (char)1;
    public float impulseConstant;
    public Transform heldObjectLocation;

    private Rigidbody2D rb2d;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.position = transform.position + direction * speed * Time.deltaTime;

        if(Input.GetAxisRaw("Horizontal"+playerNumber) != 0 || Input.GetAxisRaw("Vertical"+playerNumber) != 0)
        {
            Vector3 lookAt = (transform.position + direction) - transform.position;
            float angle = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            rb2d.SetRotation(q);
        }
        Debug.DrawRay(transform.position, transform.up * pickupRange, Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal" + playerNumber);
        float verticalInput = Input.GetAxisRaw("Vertical" + playerNumber);

        direction = new Vector3(horizontalInput, verticalInput, 0);
        heldObjectLocation.position = transform.position + transform.up;  

        CheckInteract();
        CheckHeld();
    }

    private void CheckInteract()
    {
        if(Input.GetButtonDown("Interact" + playerNumber))
        {
            if (held != null && held.gameObject.GetComponent<TowerPlaceCheck>().Obstructed().Count == 0)
            {
                held.isTrigger = false;
                held = null;
                return;
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, pickupRange);
            if (hit.collider != null && hit.collider.tag == "Tower")
            {
                held = hit.collider;
                held.isTrigger = true;
            }
        }
    }

    private void CheckHeld()
    {
        if(held == null)
        {
            return;
        }
        else if (held.gameObject.GetComponent<TowerPlaceCheck>().Obstructed().Count != 0)
            foreach(Collider2D obstruction in held.gameObject.GetComponent<TowerPlaceCheck>().Obstructed())
            {
                Vector3 lookAt = heldObjectLocation.position - obstruction.transform.position;

                float pushForce = impulseConstant - Vector3.Distance(heldObjectLocation.position, obstruction.transform.position);
                rb2d.AddForce(lookAt*pushForce*Time.deltaTime, ForceMode2D.Impulse);
                Debug.DrawRay(obstruction.transform.position, lookAt, Color.yellow);
            }

        held.gameObject.GetComponent<Rigidbody2D>().MovePosition(heldObjectLocation.position);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(heldObjectLocation.position, new Vector3(1f, 1f, 1));
    }
}

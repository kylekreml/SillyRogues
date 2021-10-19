using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public int lootCount = 0;
    public float playerInteractRange = 1.5f;
    public Collider2D held = null;
    public char playerNumber = (char)1;

    private Vector3 NormalizedDirection = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal" + playerNumber);
        float verticalInput = Input.GetAxis("Vertical" + playerNumber);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if (direction != Vector3.zero)
        {
            NormalizedDirection = direction.normalized;
        }
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteract();
        CheckHeld();
        CheckLoot();
    }

    private void CheckInteract()
    {
        if(Input.GetButtonDown("Interact" + playerNumber))
        {
            if (held != null)
            {
                //Going to have to place in grid somewhere around here.
                held.isTrigger = false;
                held = null;
                return;
            }
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.rotation * NormalizedDirection, playerInteractRange);
            Debug.Log(hit.collider.tag);
            if (hit && hit.transform.tag == "Tower")
            {
                hit.collider.isTrigger = true;
                held = hit.collider;
            }
        }
    }

    private void CheckHeld()
    {
        if(held == null)
        {
            return;
        }
        held.transform.position = this.transform.position + NormalizedDirection * 1.2f;
    }

    private void CheckLoot()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(this.transform.position, 4f);
        if (hit.Length == 0)
        {
            return;
        }
        foreach (Collider2D item in hit)
        {
            if (item.tag == "Loot")
            {
                Destroy(item.gameObject);
                lootCount++;
            }
        }
    }
}

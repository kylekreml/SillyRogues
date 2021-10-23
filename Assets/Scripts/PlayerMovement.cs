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
    public Animator animator;

    private Vector3 NormalizedDirection = new Vector3(0, 0, 0);
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    private void FixedUpdate()
    {
        if (direction != Vector3.zero)
        {
            NormalizedDirection = direction.normalized;
        }
        transform.Translate(direction.normalized * speed * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal" + playerNumber);
        direction.y = Input.GetAxisRaw("Vertical" + playerNumber);
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y)));
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
            //Debug.Log(hit.collider.tag);
            if (hit && hit.transform.tag == "Tower")
            {
                hit.collider.isTrigger = true;
                held = hit.collider;
                // held.transform.SetActive(false);
            }
            else if (hit && hit.transform.tag == "Resource")
            {
                //Probably also need a check for the resource so it doesn't get stuck in something
                ResourceScript resourceScript = hit.collider.gameObject.GetComponent<ResourceScript>();
                if (resourceScript.ResourceType() != Resource.Node)
                    held = hit.collider;
                else
                {
                    ResourceNodeScript node = hit.collider.gameObject.GetComponent<ResourceNodeScript>();
                    node.interactResourceNode();
                }
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

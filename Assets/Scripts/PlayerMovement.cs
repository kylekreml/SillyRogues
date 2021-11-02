using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundMap;

    public float speed = 10f;
    public int lootCount = 0;
    public float playerInteractRange = 1.5f;
    public Collider2D held = null;
    public char playerNumber = (char)1;
    public Animator animator;

    private Vector3 NormalizedDirection = new Vector3(0, 0, 0);
    private Vector3 direction;
    private GameObject indicator;
    private SpriteRenderer indicatorSprite;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        indicator = transform.Find("Indicator").gameObject;
        indicatorSprite = indicator.GetComponent<SpriteRenderer>();
        indicatorSprite.color = new Color(1f, 1f, 1f, 0f);
    }

    private void FixedUpdate()
    {
        if (direction != Vector3.zero)
        {
            NormalizedDirection = direction.normalized;
            animator.SetFloat("LastX", direction.x);
            animator.SetFloat("LastY", direction.y);
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
                if (held.gameObject.tag == "Resource")
                {
                    ResourceScript resourceScript = held.gameObject.GetComponent<ResourceScript>();
                    if (resourceScript.GetUseAsCraft())
                    {
                        resourceScript.SetPlayerInteracted(true);
                    }
                }
                if (held.gameObject.tag == "Tower") 
                { 
                    Collider2D[] overlaps = Physics2D.OverlapBoxAll(new Vector2(indicator.transform.position.x,indicator.transform.position.y), new Vector2(0.5f, 0.5f), 0f);
                    foreach (Collider2D o in overlaps)
                    {
                        Debug.Log(o.tag);
                        if (o.tag == "Tower")
                        {
                            return;
                        }
                    }
                }
                var oldHeld = held;
                held = null;
                SpriteRenderer heldSprite = oldHeld.GetComponent<SpriteRenderer>();
                heldSprite.color = new Color(1f, 1f, 1f, 1f);
                indicatorSprite.color = new Color(1f, 1f, 1f, 0f);
                oldHeld.transform.position = groundMap.WorldToCell(this.transform.position + NormalizedDirection);
                oldHeld.enabled = true;
                if(oldHeld.gameObject.tag == "Tower")
                {
                   oldHeld.GetComponent<TowerClass>().enableTower();
                }
                //Going to have to place in grid somewhere around here.
                return;
            }

            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.rotation * NormalizedDirection, playerInteractRange);
            //Debug.Log(hit.collider.tag);
            if (hit && hit.transform.tag == "Tower")
            {
                hit.collider.GetComponent<TowerClass>().disableTower();
                hit.collider.enabled = false;
                held = hit.collider;
                SpriteRenderer heldSprite = held.GetComponent<SpriteRenderer>();
                heldSprite.color = new Color(1f, 0.5f, 0.5f, 0.2f);
                // held.transform.SetActive(false);
                indicatorSprite.color = new Color(1f, 1f, 1f, 1f);
            }

            else if (hit && hit.transform.tag == "Resource")
            {
                //Probably also need a check for the resource so it doesn't get stuck in something
                ResourceScript resourceScript = hit.collider.gameObject.GetComponent<ResourceScript>();
                if (resourceScript.GetResourceType() != Resource.Node)
                {
                    held = hit.collider;
                    indicatorSprite.color = new Color(1f, 1f, 1f, 1f);
                }
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
        indicator.transform.position = groundMap.WorldToCell(this.transform.position + NormalizedDirection * 1.2f);
        held.transform.position = this.transform.position;
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

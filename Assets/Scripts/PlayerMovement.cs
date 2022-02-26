using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundMap;

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float timeToMaxSpeed = .08f;

    [Header("Player Components")]
    public int lootCount = 0;
    public float playerInteractRange = 1.5f;
    public float playerInteractWidth = 0.7f;
    public Collider2D held = null;
    public char playerNumber = (char)1;
    public Animator animator;

    private Vector3 NormalizedDirection = new Vector3(0, 0, 0);
    private Vector3 direction;
    private GameObject indicator;
    private SpriteRenderer indicatorSprite;
    private Collider2D nodeCollider;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        indicator = transform.Find("Indicator").gameObject;
        indicatorSprite = indicator.GetComponent<SpriteRenderer>();
        indicatorSprite.color = new Color(1f, 1f, 1f, 0f);
        nodeCollider = null;
    }

    private void FixedUpdate()
    {
        movement();
    }

    // Update is called once per frame
    void Update()
    {
        // direction.x = Input.GetAxisRaw("Horizontal" + playerNumber);
        // direction.y = Input.GetAxisRaw("Vertical" + playerNumber);
        if (Time.timeScale == 1f)
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y)));
        }
        // CheckInteract();
        CheckHeld();
        CheckLoot();

        //Debug for drawing the area box that the player can interact with in blue
        // Vector2 pVector = new Vector2(NormalizedDirection.y, -NormalizedDirection.x) / Mathf.Sqrt(Mathf.Pow(NormalizedDirection.x, 2f) + Mathf.Pow(NormalizedDirection.y, 2f));
        // Vector3 perpendicular = new Vector3(pVector.x, pVector.y, 0);
        // Vector3 closeLeft = transform.position - perpendicular * (playerInteractWidth/2) - NormalizedDirection/3;
        // Vector3 farRight = transform.position + perpendicular * (playerInteractWidth/2) + NormalizedDirection * playerInteractRange;
        // Debug.DrawLine(transform.position - perpendicular * (playerInteractWidth/2) - NormalizedDirection/3,
        //     transform.position + perpendicular * (playerInteractWidth/2) - NormalizedDirection/3,
        //     Color.blue);
        // Debug.DrawLine(transform.position - perpendicular * (playerInteractWidth/2) - NormalizedDirection/3,
        //     transform.position - perpendicular * (playerInteractWidth/2) + NormalizedDirection * playerInteractRange,
        //     Color.blue);
        // Debug.DrawLine(transform.position + perpendicular * (playerInteractWidth/2) - NormalizedDirection/3,
        //     transform.position + perpendicular * (playerInteractWidth/2) + NormalizedDirection * playerInteractRange,
        //     Color.blue);
        // Debug.DrawLine(transform.position - perpendicular * (playerInteractWidth/2) + NormalizedDirection * playerInteractRange,
        //     transform.position + perpendicular * (playerInteractWidth/2) + NormalizedDirection * playerInteractRange,
        //     Color.blue);
    }

    private void InteractPressed()
    {
        if (held != null)
        {
            Collider2D[] overlaps = Physics2D.OverlapBoxAll(new Vector2(indicator.transform.position.x,indicator.transform.position.y), new Vector2(0.5f, 0.5f), 0f);
            if (held.gameObject.tag == "Resource")
            {
                ResourceScript resourceScript = held.gameObject.GetComponent<ResourceScript>();

                foreach (Collider2D o in overlaps)
                {
                    //Debug.Log(o.tag);
                    if (o.tag == "Tower" || o.tag == "Wall")
                    {
                        return;
                    }

                }
                resourceScript.SetLastPlayer(gameObject);
                resourceScript.SetPlayerInteracted(true);
            }
            else if (held.gameObject.tag == "Tower") 
            { 
                
                foreach (Collider2D o in overlaps)
                {
                    //Debug.Log(o.tag);
                    if (o.tag == "Tower" || o.tag == "Wall")
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
            oldHeld.transform.position = groundMap.WorldToCell(this.transform.position + NormalizedDirection * 0.8f);
            oldHeld.enabled = true;
            if(oldHeld.gameObject.tag == "Tower")
            {
                oldHeld.GetComponent<TowerClass>().enableTower();
            }
            else if (oldHeld.gameObject.tag == "Resource")
            {
                oldHeld.isTrigger = false;
            }
            //Going to have to place in grid somewhere around here.
            return;
        }
        // RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.rotation * NormalizedDirection, playerInteractRange);
        // Replacing Raycast with overlap area so interact is not as narrow
        Collider2D hit = findClosestInInteractArea(overlapInteract());

        if (hit && hit.transform.tag == "Tower")
        {
            hit.gameObject.GetComponent<TowerClass>().disableTower();
            hit.enabled = false;
            SetHeld(hit);
            SpriteRenderer heldSprite = held.GetComponent<SpriteRenderer>();
            heldSprite.color = new Color(1f, 0.5f, 0.5f, 0.2f);
            // held.transform.SetActive(false);
        }

        else if (hit && hit.transform.tag == "Resource")
        {
            //Probably also need a check for the resource so it doesn't get stuck in something
            ResourceScript resourceScript = hit.gameObject.GetComponent<ResourceScript>();
            if (resourceScript.GetResourceType() != Resource.Node)
            {
                SetHeld(hit);
                held.isTrigger = true;
            }
            else
            {
                nodeCollider = hit;
                if (nodeCollider.gameObject.GetComponent<ResourceNodeScript>().PlayerInteracting(gameObject))
                    nodeCollider.gameObject.GetComponent<ResourceNodeScript>().interactTimeResourceNode(true);
            }
        }
        else if (hit && hit.transform.tag == "Upgrade")
        {
            SetHeld(hit);
        }
    }

    private void InteractReleased()
    {
        if (nodeCollider != null)
        {
            animator.SetBool("Gathering", false);
            nodeCollider.gameObject.GetComponent<ResourceNodeScript>().interactTimeResourceNode(false);
            nodeCollider = null;
        }
    }

    private void movement()
    {
        if (direction != Vector3.zero)
        {
            float smoothTime = timeToMaxSpeed;
            float currentVelocity = 0f;
            if (currentSpeed < maxSpeed - 1)
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, maxSpeed, ref currentVelocity, smoothTime);
            }
            else
            {
                currentSpeed = maxSpeed;
            }
            NormalizedDirection = direction.normalized;
            animator.SetFloat("LastX", direction.x);
            animator.SetFloat("LastY", direction.y);
        }
        else
        {
            // Instant deceleration
            currentSpeed = 0f;
        }
        transform.Translate(direction.normalized * currentSpeed * Time.fixedDeltaTime);
    }

    private void CheckHeld()
    {
        if (nodeCollider != null)
        {
            animator.SetBool("Gathering", true);
            List<Collider2D> check = new List<Collider2D>(overlapInteract());
            if (!check.Contains(nodeCollider))
            {
                animator.SetBool("Gathering", false);
                nodeCollider.gameObject.GetComponent<ResourceNodeScript>().interactTimeResourceNode(false);
                nodeCollider = null;
            }
        }

        if(held == null)
        {
            return;
        }
        indicator.transform.position = groundMap.WorldToCell(this.transform.position + NormalizedDirection * 0.8f);
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

    // overlapInteract() finds colliders inside of the OverlapAreaAll (a rectangle)
    private Collider2D[] overlapInteract()
    {
        // Does Vector math on a Vector2 then made into a Vector3
        // Finds the vector that is perpendicular to NormalizedDirection
        Vector2 pVector = new Vector2(NormalizedDirection.y, -NormalizedDirection.x) / Mathf.Sqrt(Mathf.Pow(NormalizedDirection.x, 2f) + Mathf.Pow(NormalizedDirection.y, 2f));
        Vector3 perpendicular = new Vector3(pVector.x, pVector.y, 0) * (playerInteractWidth/2);

        // If the player is looking up, names are self-explanatory
        // NormalizedDirection/3 is to cover the player sprite within the overlap
        Vector3 closeLeft = transform.position - perpendicular - NormalizedDirection/3;
        Vector3 farRight = transform.position + perpendicular + NormalizedDirection * playerInteractRange;

        Collider2D[] hits = Physics2D.OverlapAreaAll(
            new Vector2(closeLeft.x, closeLeft.y),
            new Vector2(farRight.x, farRight.y)
        );

        return hits;
    }

    // findClosestInInteractArea finds the closest collider in the list of Collider2D
    private Collider2D findClosestInInteractArea(Collider2D[] hits)
    {
        // Finds the closest collider to the player
        Collider2D closest = null;
        foreach (Collider2D hit in hits)
        {
            if (hit.transform.tag != "Player" && (hit.transform.tag == "Tower" || hit.transform.tag == "Resource" || hit.transform.tag == "Upgrade"))
            {
                if (closest == null)
                    closest = hit;
                else
                {
                    if (Vector3.Distance(transform.position, hit.transform.position) < Vector3.Distance(transform.position, closest.transform.position))
                        closest = hit;
                }
            }
        }

        // Uncomment if you need to find the name of what is being returned
        // if (closest)
        // {
        //     Debug.Log(closest.gameObject.name);
        // }
        return closest;
    }

    public bool SetHeld(Collider2D obj)
    {
        if (held == null)
        {
            held = obj;
            indicatorSprite.color = new Color(1f, 1f, 1f, 1f);
        }
        if (nodeCollider != null)
        {
            nodeCollider.gameObject.GetComponent<ResourceNodeScript>().interactTimeResourceNode(false);
            nodeCollider = null;
        }
        return false;
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        Vector2 movementInput = ctx.ReadValue<Vector2>();
        direction.x = movementInput.x;
        direction.y = movementInput.y;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        // Debug.Log(ctx.performed);
        if (ctx.started)
        {
            InteractPressed();
        }
        else if (ctx.canceled)
        {
            InteractReleased();
        }
    }

    // void OnEnable()
    // {
    //     input.Enable();
    // }

    // void OnDisable()
    // {
    //     input.Disable();
    // }
}

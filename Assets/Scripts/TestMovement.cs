using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestMovement : MonoBehaviour
{
    [SerializeField]
    private Tilemap groundMap;

    [SerializeField]
    private Tilemap colMap;

    private Vector2 direction;
    private bool isMoving;
    private float inputDelay = 0.2f;

    public char playerNumber = (char)1;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(0, 0);
        isMoving = false;
        transform.position = groundMap.WorldToCell(transform.position);
    }

    void FixedUpdate()
    {
        if (!isMoving)
        {
            isMoving = true;
            float horizontalInput = Input.GetAxis("Horizontal" + playerNumber);
            float verticalInput = Input.GetAxis("Vertical" + playerNumber);
            if (verticalInput > 0)
            {
                direction.y = 1;
                StartCoroutine(movePlayer());
            }
            else if (verticalInput < 0)
            {
                direction.y = -1;
                StartCoroutine(movePlayer());
            }
            else if (horizontalInput > 0)
            {
                direction.x = 1;
                StartCoroutine(movePlayer());
            }
            else if (horizontalInput < 0)
            {
                direction.x = -1;
                StartCoroutine(movePlayer());
            }
            else
            {
                isMoving = false;
            }
        }

    }

    private bool canMove()
    {
        Vector3Int gridPosition = groundMap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundMap.HasTile(gridPosition) || colMap.HasTile(gridPosition))
        {
            return false;
        }
        return true;
    }

    private IEnumerator movePlayer()
    {
        if (canMove())
        {

            float moveTime = 0;
            while (moveTime < inputDelay)
            {
               moveTime += Time.deltaTime;
                yield return null;
            }
            transform.position = groundMap.WorldToCell(transform.position + (Vector3)direction);
            direction.x = direction.y = 0;
        }
        isMoving = false;
        direction.x = direction.y = 0;
    }
}

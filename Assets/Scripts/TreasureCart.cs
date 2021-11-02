using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCart : MonoBehaviour
{
    public int totalLoot = 25;
    public int lootCollectRadius = 2;
    [SerializeField]
    private int lootCount;

    void Start()
    {
        lootCount = totalLoot;
    }
    void Update()
    {
        CheckPlayerLoot();
    }


    private void CheckPlayerLoot()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(this.transform.position, lootCollectRadius);
        if (hit.Length == 0)
        {
            return;
        }
        foreach (Collider2D objCol in hit)
        {
            if (objCol.tag == "Player")
            {
                PlayerMovement player = objCol.GetComponent<PlayerMovement>();
                lootCount += player.lootCount;
                player.lootCount = 0;
            }
            else if (objCol.tag == "Enemy" && lootCount > 0)
            {
                if (objCol.gameObject.GetComponent<BasicEnemyScript>().giveLoot())
                {
                    lootCount--;
                }
            }
        }
    }
}

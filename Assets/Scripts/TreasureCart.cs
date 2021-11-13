using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCart : MonoBehaviour
{
    public int totalLoot = 25;
    public int lootCollectRadius = 2;

    void Start()
    {
        GameManager.Instance.SetGold(totalLoot);
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
                GameManager.Instance.ChangeGold(player.lootCount);
                player.lootCount = 0;
            }
            else if (objCol.tag == "Enemy" && GameManager.Instance.GetGold() > 0)
            {
                if (objCol.gameObject.GetComponent<BasicEnemyScript>().giveLoot())
                {
                    GameManager.Instance.ChangeGold(-1);
                }
            }
        }
    }
}

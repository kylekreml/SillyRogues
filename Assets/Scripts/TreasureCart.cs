using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCart : MonoBehaviour
{
    public int totalLoot = 25;
    private int currentLoot;
    public int lootCollectRadius = 2;

    void Start()
    {
        currentLoot = totalLoot;
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
            else if (objCol.tag == "Enemy" && currentLoot > 0)
            {
                objCol.gameObject.GetComponent<BasicEnemyScript>().giveLoot();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCart : MonoBehaviour
{
    [SerializeField]
    private int totalLoot = 25;
    [SerializeField]
    private int currentLoot;
    [SerializeField]
    private int lootCollectRadius = 100;
    [SerializeField]
    private float enemyCollectRadius = 1.5f;

    void Start()
    {
        currentLoot = totalLoot;
        GameManager.Instance.SetGold(totalLoot);
    }
    void Update()
    {
        CheckPlayerLoot();
        EnemyTakeLoot();
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
                player.lootCount = 0;
            }
            else if (objCol.tag == "Loot")
            {
                currentLoot++;
                Destroy(objCol.gameObject);
            }
        }
    }

    private void EnemyTakeLoot()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(this.transform.position, enemyCollectRadius);
        if (hit.Length == 0)
        {
            return;
        }
        foreach (Collider2D objCol in hit)
        {
            if (objCol.tag == "Enemy" && currentLoot > 0)
            {
                if (objCol.gameObject.GetComponent<BasicEnemyScript>().giveLoot())
                    currentLoot--;
            }
        }
    }
}

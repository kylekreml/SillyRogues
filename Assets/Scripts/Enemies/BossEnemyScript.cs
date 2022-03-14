using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyScript : BasicEnemyScript
{
    [Header("Attack")]
    [Tooltip("Windup before attack")]
    [SerializeField]
    private float timeBeforeAttack;
    [Tooltip("Recoil after attack")]
    [SerializeField]
    private float timeAfterAttack;
    [SerializeField]
    private float attackRadius;
    [Tooltip("What percentage of health for the Boss to attack")]
    [SerializeField]
    private float[] attackPercentages;
    private int attackGateIndex = 0;
    [SerializeField]
    private float launchForce = 10;
    [SerializeField]
    private GameObject aoe;

    // Start is called before the first frame update
    new void Start()
    {
        
        animator = this.GetComponent<Animator>();
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (attackGateIndex != attackPercentages.Length && health/maxHealth <= attackPercentages[attackGateIndex])
        {
            attackGateIndex++;
            StartCoroutine(attackAround());
        }
    }


    IEnumerator attackAround()
    {
        changeSpeed(0f);
        aoe.SetActive(true);
        base.animator.SetBool("Windup", true);
        yield return new WaitForSeconds(0.3f);
        base.animator.SetBool("stopWind", true);
        yield return new WaitForSeconds(timeBeforeAttack-0.6f);
        base.animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.3f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.tag == "Tower")
            {
                hit.gameObject.GetComponent<TowerClass>().Knockback((hit.gameObject.transform.position - transform.position)*launchForce);
                // Having issues with towers so I'm doing spaghetti due to time constraints
                // TowerClass tc = hit.gameObject.GetComponent<TowerClass>();
                // if (tc != null)
                // {
                //     tc.Knockback(hit.gameObject.transform.position - transform.position);
                // }
                // else
                // {
                //     TowerBehavior tb = hit.gameObject.GetComponent<TowerBehavior>();
                //     if (tb != null)
                //     {
                //         tb.Knockback(hit.gameObject.transform.position - transform.position);
                //     }
                //     else
                //     {
                //         StatusTowerBehavior stb = hit.gameObject.GetComponent<StatusTowerBehavior>();
                //         if (stb != null)
                //         {
                //             stb.Knockback(hit.gameObject.transform.position - transform.position);
                //         }
                //         else
                //         {
                //             AoETower at = hit.gameObject.GetComponent<AoETower>();
                //             if (at != null)
                //             {
                //                 at.Knockback(hit.gameObject.transform.position - transform.position);
                //             }
                //         }
                //     }
                // }
            }
            if (hit.gameObject.tag == "Player")
            {
                hit.gameObject.GetComponent<PlayerMovement>().Knockback(hit.gameObject.transform.position - transform.position);
            }
        }
        aoe.SetActive(false);
        base.animator.SetBool("Attack", false);
        yield return new WaitForSeconds(timeAfterAttack);
        base.animator.SetBool("Windup", false);
        base.animator.SetBool("stopWind", false);
        resetSpeed();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

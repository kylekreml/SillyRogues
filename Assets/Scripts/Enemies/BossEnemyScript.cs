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
    private Animator animator;
    public float test;
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
        test = health/maxHealth;
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
        animator.SetBool("Windup", true);
        yield return new WaitForSeconds(timeBeforeAttack);
        animator.SetBool("Attack", true);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.tag == "Tower")
            {
                Destroy(hit.gameObject);
            }
        }
        aoe.SetActive(false);
        animator.SetBool("Attack", false);
        yield return new WaitForSeconds(timeAfterAttack);
        animator.SetBool("Windup", false);
        resetSpeed();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

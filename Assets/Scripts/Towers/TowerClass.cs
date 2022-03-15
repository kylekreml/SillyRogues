using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerClass : MonoBehaviour
{


    public bool disabled = false;
    public bool buffed = false;


    public int xp = 0;
    public int xpToTierOne;
    public int xpToTierTwo;
    public int tier = 0;
    public bool readyToUpgrade = false;

    private Rigidbody2D rb2d;

    [SerializeField]
    private float knockbackTimerLength = 1f;
    private float knockbackTimer = 0;

    public Sprite[] spriteList;
    
    //public int tier = 1;
    // Start is called before the first frame update
    public virtual void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
        else
        {
            StopKnockback();
        }
    }

    public void enableTower()
    {
        disabled = false;
    }

    public void disableTower()
    {
        disabled = true;
    }

    public void buffTower()
    {
        buffed = true;
    }

    public void debuffTower()
    {
        buffed = false;
    }

    public void giveXp()
    {  
        if (tier < 2)
        {
            if (!readyToUpgrade)
            {
                xp += 1;
                if ((xp >= xpToTierOne && tier == 0) || (xp >= xpToTierTwo && tier == 1))
                { //tower is tier 0 and ready to upgrade when player brings an upgrade kit
                    readyToUpgrade = true;
                    xp = 0;

                    this.transform.Find("Upgrade Ready").gameObject.SetActive(true);

                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Upgrade"))
        {
            //upgrade tower
            if (tier < 2)
            //tower is below tier 3
            {
                if (readyToUpgrade) 
                {
                    upgradeTower();
                    Destroy(collision.gameObject);
                }
            }

        }
    }

    public virtual void upgradeTower()
    {
            this.GetComponent<SpriteRenderer>().sprite = spriteList[tier];

            tier += 2;
            readyToUpgrade = false;
            this.transform.Find("Upgrade Ready").gameObject.SetActive(false);
    }

    public int getTier()
    {
        return tier;
    }

    public void Knockback(Vector3 direction)
    {
        disableTower();
        knockbackTimer = knockbackTimerLength;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.AddForce(direction, ForceMode2D.Impulse);
    }

    private void StopKnockback()
    {
        enableTower();
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        rb2d.velocity = new Vector2(0f, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            StopKnockback();
        }
    }
}

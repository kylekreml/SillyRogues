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

    public Sprite[] spriteList;
    
    //public int tier = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            xp += 1;
            if((xp >= xpToTierOne && tier == 0) || (xp >= xpToTierTwo && tier == 1))
            { //tower is tier 0 and ready to upgrade when player brings an upgrade kit
                readyToUpgrade = true;
                xp = 0;
                // insert Updgrade Indicator Here, IDK how to do animations
                //
                //
                //
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

            tier += 1;
            readyToUpgrade = false;
    }

    public int getTier()
    {
        return tier;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoETower : TowerClass
{
    public float attackSpeed = 1;
    public GameObject blade;

    private SpriteRenderer swordSprite;

    private BoxCollider2D swordCollider;

    private GameObject Sword;



    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Sword = this.transform.GetChild(0).gameObject;
        swordSprite = Sword.GetComponent<SpriteRenderer>();
        swordCollider = Sword.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!this.disabled)
        {
            Rotate();
            swordCollider.enabled = true;
        }
        else
        {
            swordCollider.enabled = false;
        }
    }
    void Rotate()
    {
        if (buffed)
        {
            gameObject.transform.Rotate(0, 0, attackSpeed * 1.25f * 10);
        }
        else
        {
            gameObject.transform.Rotate(0, 0, attackSpeed * 10);
        }
    }


    public override void upgradeTower()
    {
        //Debug.Log("SWORD UPGRADE!!!");
        swordSprite.sprite = spriteList[tier];
        
        tier += 1;
        if (tier == 1)
            // tower got upgraded to tier 1 so the range on the sword is longer
        {// TEMP CODE IN PLACE OF SPRITE
            swordSprite.color = new Color(1, 0, 0, 1);
            //CHANGE THIS TO CHANGE SIZE OF SWORD
            //swordCollider.size = new Vector2(swordCollider.size.x, swordCollider.size.y * 2);
            //swordSprite.size = new Vector2(swordSprite.size.x, swordSprite.size.y * 2);
            this.transform.GetChild(0).localScale = new Vector3(Sword.transform.localScale.x, Sword.transform.localScale.y * 2, Sword.transform.localScale.z);


        }
        else if (tier == 2)
            // tower got upgraded to tier 2 so the speed of rotation is faster
        {//TEMP CODE IN PLACE OF SPRITE
            swordSprite.color = new Color(1, 0, 1, 1);
            //CHANGE THIS LATER TO CHANGE SPEED OF UPGRADED TOWER
            attackSpeed = 1.5f;
        }
        readyToUpgrade = false;
        this.transform.Find("Upgrade Ready").gameObject.SetActive(false);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoEBladeDamage : MonoBehaviour
{
    public float damage = 1f;

    public AudioSource soundEffect;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!soundEffect.isPlaying)
            {
                soundEffect.Play();
            }

            other.gameObject.GetComponent<BasicEnemyScript>().damage(damage, this.transform.parent.gameObject);
        }
    }
}

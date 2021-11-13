using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileClass : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    public GameObject shooter;
    

    public virtual void setTarget(GameObject t)
    {
        
        target = t;
       
    }
    public void setShooter(GameObject tower)
    {
        shooter = tower;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resource
{
    Node,
    Wood,
    Stone,
    Metal,
    Crystal,
    Oil,
}
public class ResourceScript : MonoBehaviour
{
    [SerializeField]
    private Resource type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetType(Resource r)
    {
        type = r;
    }

    public Resource ResourceType()
    {
        return type;
    }
}

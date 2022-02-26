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
    private Resource type = Resource.Node;
    private bool playerInteracted;
    [SerializeField]
    private GameObject lastPlayer;

    [SerializeField]
    private ResourceScriptableObject resourceScriptableObject;

    public List<Sprite> sprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetResourceType(Resource r)
    {
        type = r;
        this.GetComponent<SpriteRenderer>().sprite = resourceScriptableObject.sprites[(int)r];
    }

    public Resource GetResourceType()
    {
        return type;
    }

    public void SetPlayerInteracted(bool state)
    {
        playerInteracted = state;
        if (state)
            StartCoroutine("resetPlayerInteracted");
    }

    public bool GetPlayerInteracted()
    {
        return playerInteracted;
    }

    IEnumerator resetPlayerInteracted()
    {
        yield return new WaitForSeconds(.2f);
        SetPlayerInteracted(false);
    }

    public void SetLastPlayer(GameObject player)
    {
        lastPlayer = player;
    }

    public GameObject GetLastPlayer()
    {
        return lastPlayer;
    }
}

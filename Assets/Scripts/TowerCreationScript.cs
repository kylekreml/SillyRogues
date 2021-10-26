using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCreationScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] towers;
    [SerializeField]
    private Transform spawnpoint;
    [SerializeField]
    private GameObject dummyResource;

    [SerializeField]
    private Resource resource1;
    [SerializeField]
    private Resource resource2;
    static Dictionary<Resource, HashSet<int>> recipes = new Dictionary<Resource, HashSet<int>>();

    // Start is called before the first frame update
    void Start()
    {
        spawnpoint = transform.GetChild(0);
        resource1 = Resource.Node;
        resource2 = Resource.Node;

        if (recipes.Count == 0)
        {
            //Crafting recipes
            //Towers that use Wood
            HashSet<int> craftedTower = new HashSet<int>();
            craftedTower.Add(0);
            craftedTower.Add(2);
            recipes.Add(Resource.Wood, craftedTower);

            //Towers that use Stone
            craftedTower = new HashSet<int>();
            craftedTower.Add(1);
            craftedTower.Add(2);
            recipes.Add(Resource.Stone, craftedTower);

            //Towers that use Metal
            craftedTower = new HashSet<int>();
            craftedTower.Add(0);
            craftedTower.Add(1);
            recipes.Add(Resource.Metal, craftedTower);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(resource1 != Resource.Node && resource2 != Resource.Node)
        {
            createTower(towerRecipes());
        }
    }

    //Recipes in a dictionary with the key as the first resource, then the values as possible recipes
    //Finds intersecting values in the two resources' recipe hashset
    //The recipes are just ints corresponding to the tower slot in the inspector
    int towerRecipes()
    {
        
        if (resource1 != resource2)
        {
            HashSet<int> possibleRecipes = new HashSet<int>(recipes[resource1]);
            possibleRecipes.IntersectWith(recipes[resource2]);
            //Jank way to get the only element in the hashset because I am dumb - Justin
            foreach (int i in possibleRecipes)
            {
                return i;
            }
        }
        return -1;
    }

    //Given an int to create a tower
    void createTower(int tower)
    {
        if (tower == -1)
        {
            GameObject r1 = Instantiate(dummyResource);
            r1.GetComponent<ResourceScript>().SetResourceType(resource1);
            GameObject r2 = Instantiate(dummyResource);
            r2.GetComponent<ResourceScript>().SetResourceType(resource2);
            r1.transform.position = spawnpoint.position;
            r2.transform.position = spawnpoint.position;
            resource1 = Resource.Node;
            resource2 = Resource.Node;
        }
        else
        {
            GameObject t = Instantiate(towers[tower]);
            t.transform.position = spawnpoint.position;
            resource1 = Resource.Node;
            resource2 = Resource.Node;
        }
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log(collider.gameObject.name);
        if (collider.gameObject.tag == "Resource")
        {
            if (resource1 == Resource.Node)
            {
                resource1 = collider.gameObject.GetComponent<ResourceScript>().GetResourceType();
                Destroy(collider.gameObject);
            }
            else if (resource2 == Resource.Node)
            {
                resource2 = collider.gameObject.GetComponent<ResourceScript>().GetResourceType();
                Destroy(collider.gameObject);
            }
        }
    }
}

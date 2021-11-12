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
    private GameObject upgradeKit;

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
            craftedTower.Add(0); //basic
            craftedTower.Add(1); //dom
            craftedTower.Add(4); //buff
            recipes.Add(Resource.Wood, craftedTower);

            //Towers that use Stone
            craftedTower = new HashSet<int>();
            craftedTower.Add(0); //basic
            craftedTower.Add(2); //slow
            craftedTower.Add(5); //line
            recipes.Add(Resource.Stone, craftedTower);

            //Towers that use Metal
            craftedTower = new HashSet<int>();
            craftedTower.Add(1); //dom
            craftedTower.Add(3); //sword
            craftedTower.Add(5); //line
            recipes.Add(Resource.Metal, craftedTower);

            //Towers that use Crystal
            craftedTower = new HashSet<int>();
            craftedTower.Add(2); //slow
            craftedTower.Add(3); //sword
            craftedTower.Add(4); //buff
            recipes.Add(Resource.Crystal, craftedTower);
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
        else
            return -1;
        return -2;
    }

    //Given an int to create a tower
    void createTower(int tower)
    {
        if (tower == -2)
        {
            // Old code to spit duplicate resources back out
            // This should never happen
            // If it does, something is majorly fucked
            
            GameObject r1 = Instantiate(dummyResource);
            r1.GetComponent<ResourceScript>().SetResourceType(resource1);
            GameObject r2 = Instantiate(dummyResource);
            r2.GetComponent<ResourceScript>().SetResourceType(resource2);
            r1.transform.position = spawnpoint.position;
            r2.transform.position = spawnpoint.position;
        }
        if (tower == -1)
        {
            GameObject uk = Instantiate(upgradeKit);
            uk.transform.position = spawnpoint.position;

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

    //Today I learned that triggers don't get detected in OnCollisionStay2d but just change with OnTriggerStay2D and the collider will detect triggers
    //
    // Want to detect trigger inside collider -> OnTriggerStay2D/similar function call
    // Want to detect collider with collider -> OnCollisionStay2D/similar function call
    private void OnCollisionEnter2D(Collision2D collider)
    {
        // Debug.Log(collider.gameObject.name);
        if (collider.gameObject.tag == "Resource")
        {
            ResourceScript resourceScript = collider.gameObject.GetComponent<ResourceScript>();
            if (resource1 == Resource.Node && resourceScript.GetPlayerInteracted())
            {
                resource1 = resourceScript.GetResourceType();
                Destroy(collider.gameObject);
            }
            else if (resource2 == Resource.Node && resourceScript.GetPlayerInteracted())
            {
                resource2 = resourceScript.GetResourceType();
                Destroy(collider.gameObject);
            }
        }
    }

    // private void OnTriggerEnter2D(Collider2D collider)
    // {
    //     Debug.Log(collider.gameObject.name);
    //     if (collider.gameObject.tag == "Resource")
    //     {
    //         ResourceScript resourceScript = collider.gameObject.GetComponent<ResourceScript>();
    //         // resourceScript.SetUseAsCraft(true);
    //         if (resource1 == Resource.Node && resourceScript.GetPlayerInteracted())
    //         {
    //             resource1 = resourceScript.GetResourceType();
    //             Destroy(collider.gameObject);
    //         }
    //         else if (resource2 == Resource.Node && resourceScript.GetPlayerInteracted())
    //         {
    //             resource2 = resourceScript.GetResourceType();
    //             Destroy(collider.gameObject);
    //         }
    //     }
    // }
}

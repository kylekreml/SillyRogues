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
    private GameObject upgradeKit;
    [SerializeField]
    private GameObject dummyResource;

    private GameObject resource1;
    private GameObject resource2;

    private ResourceScript resourceScript1;
    private ResourceScript resourceScript2;

    [SerializeField]
    private Resource resource1Type;
    [SerializeField]
    private Resource resource2Type;
    [SerializeField]
    private GameObject resource1LastPlayer;
    [SerializeField]
    private GameObject resource2LastPlayer;

    [SerializeField]
    private bool keepCountOfTowers = false;
    [SerializeField]
    private GameObject finalLevelTowersTrack = null;
    private List<GameObject> towersCreated;
    
    private GameObject craftingAssistant;
    private bool useCraftingAssistant;
    static Dictionary<Resource, HashSet<int>> recipes = new Dictionary<Resource, HashSet<int>>();

    // Start is called before the first frame update
    void Start()
    {
        spawnpoint = transform.GetChild(0);
        craftingAssistant = transform.GetChild(2).gameObject;
        useCraftingAssistant = PauseMenuSettings.CraftingAssistantToggle;
        resource1Type = Resource.Node;
        resource2Type = Resource.Node;

        if (finalLevelTowersTrack != null)
            keepCountOfTowers = true;

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
        if (resource1Type != Resource.Node && resource2Type != Resource.Node)
        {
            createTower(towerRecipes());
        }

        if (useCraftingAssistant != PauseMenuSettings.CraftingAssistantToggle)
        {
            useCraftingAssistant = PauseMenuSettings.CraftingAssistantToggle;
            CraftingAssistantToggle(PauseMenuSettings.CraftingAssistantToggle);
        }
    }

    //Recipes in a dictionary with the key as the first resource, then the values as possible recipes
    //Finds intersecting values in the two resources' recipe hashset
    //The recipes are just ints corresponding to the tower slot in the inspector
    int towerRecipes()
    {
        
        if (resource1Type != resource2Type)
        {
            HashSet<int> possibleRecipes = new HashSet<int>(recipes[resource1Type]);
            possibleRecipes.IntersectWith(recipes[resource2Type]);
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
            GameObject r2 = Instantiate(dummyResource);
            r1.GetComponent<ResourceScript>().SetResourceType(resource1Type);
            r2.GetComponent<ResourceScript>().SetResourceType(resource2Type);
            r1.transform.position = spawnpoint.position;
            r2.transform.position = spawnpoint.position;
        }
        if (tower == -1)
        {
            GameObject uk = Instantiate(upgradeKit);

            // Places down at spawnpoint location
            // uk.transform.position = spawnpoint.position;

            // Places in last player's hands
            resource2LastPlayer.GetComponent<PlayerMovement>().SetHeld(uk.GetComponent<BoxCollider2D>());

            resource1Type = Resource.Node;
            resource2Type = Resource.Node;
        }
        else
        {
            GameObject t = Instantiate(towers[tower]);
            // Places down at spawnpoint location
            // t.transform.position = spawnpoint.position;
            if (keepCountOfTowers && tower >= 0)
            {
                finalLevelTowersTrack.GetComponent<CRUDFinalLevelTowersScript>().TrackTowers(tower, t);
            }

            // Disables tower and
            // Places in last player's hands
            t.GetComponent<TowerClass>().disableTower();
            t.GetComponent<BoxCollider2D>().enabled = false;
            t.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 0.2f);
            resource2LastPlayer.GetComponent<PlayerMovement>().SetHeld(t.GetComponent<BoxCollider2D>());

            resource1Type = Resource.Node;
            resource2Type = Resource.Node;
        }
    }

    public void CraftingAssistantToggle(bool toggle)
    {
        if (toggle && PauseMenuSettings.CraftingAssistantToggle)
        {
            craftingAssistant.SetActive(true);
            craftingAssistant.transform.GetChild((int)resource1Type - 1).gameObject.SetActive(true);
        }
        
        if (!toggle)
        {
            craftingAssistant.transform.GetChild((int)resource1Type - 1).gameObject.SetActive(false);
            craftingAssistant.SetActive(false);
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
            SpriteRenderer spriteRenderer = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
            Sprite newSprite = collider.gameObject.GetComponent<SpriteRenderer>().sprite;
            Destroy(collider.gameObject);
            if (resource1Type == Resource.Node && resourceScript.GetPlayerInteracted())
            {
                resource1Type = resourceScript.GetResourceType();
                resource1LastPlayer = resourceScript.GetLastPlayer();
                spriteRenderer.sprite = newSprite;
                spriteRenderer.enabled = true;
                CraftingAssistantToggle(true);
            }
            else if (resource2Type == Resource.Node && resourceScript.GetPlayerInteracted())
            {
                resource2Type = resourceScript.GetResourceType();
                resource2LastPlayer = resourceScript.GetLastPlayer();
                spriteRenderer.enabled = false;
                CraftingAssistantToggle(false);
            }
        }
    }

    // private void OnTriggerEnter2D(Collider2D collider)
    // {
    //     // Debug.Log(collider.gameObject.name);
    //     if (collider.gameObject.tag == "Resource")
    //     {
    //         ResourceScript resourceScript = collider.gameObject.GetComponent<ResourceScript>();
    //         SpriteRenderer spriteRenderer = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
    //         if (resource1.GetResourceType() == Resource.Node && resourceScript.GetPlayerInteracted())
    //         {
    //             resource1.SetResourceType(resourceScript.GetResourceType());
    //             spriteRenderer.sprite = collider.gameObject.GetComponent<SpriteRenderer>().sprite;
    //             spriteRenderer.enabled = true;
    //             CraftingAssistantToggle(true);
    //             Destroy(collider.gameObject);
    //         }
    //         else if (resource2.GetResourceType() == Resource.Node && resourceScript.GetPlayerInteracted())
    //         {
    //             resource2.SetResourceType(resourceScript.GetResourceType());
    //             spriteRenderer.enabled = false;
    //             CraftingAssistantToggle(false);
    //             Destroy(collider.gameObject);
    //         }
    //     }
    // }
}

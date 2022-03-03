using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRUDFinalLevelTowersScript : MonoBehaviour
{
    [SerializeField]
    private bool finalLevel;
    [SerializeField]
    private GameObject[] towers;

    // Start is called before the first frame update
    void Start()
    {
        if (finalLevel)
        {
            SpawnTowers();
        }
        else
        {
            // do something to keep count
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnTowers()
    {
        int towerSpots = transform.childCount;
        int towerSpotIndex = 0;
        int[] totalTowers = new int[6];
        totalTowers[0] = FinalLevelTowersScript.basicTowers;
        totalTowers[1] = FinalLevelTowersScript.domTowers;
        totalTowers[2] = FinalLevelTowersScript.slowAoeTowers;
        totalTowers[3] = FinalLevelTowersScript.aoeTowers;
        totalTowers[4] = FinalLevelTowersScript.buffTowers;
        totalTowers[5] = FinalLevelTowersScript.lineAoeTowers;
        
        // Goes through amount of towers we have
        for (int t = 0; t < 6; t++)
        {
            // Spawns amount of towers
            for (int n = 0; n < totalTowers[t]; n++)
            {
                GameObject tower = Instantiate(towers[t]);
                tower.transform.position = transform.GetChild(towerSpotIndex).position;
                towerSpotIndex++;
                // if there are ever too many towers, the rest are gone once limit reached
                if (towerSpotIndex == towerSpots)
                    break;
            }
            if (towerSpotIndex == towerSpots)
                break;
        }
    }

    public void towerCount(int index)
    {
        switch (index)
        {
            case 0:
                FinalLevelTowersScript.basicTowers = FinalLevelTowersScript.basicTowers + 1;
                break;
            case 1:
                FinalLevelTowersScript.domTowers = FinalLevelTowersScript.domTowers + 1;
                break;
            case 2:
                FinalLevelTowersScript.slowAoeTowers = FinalLevelTowersScript.slowAoeTowers + 1;
                break;
            case 3:
                FinalLevelTowersScript.aoeTowers = FinalLevelTowersScript.aoeTowers + 1;
                break;
            case 4:
                FinalLevelTowersScript.buffTowers = FinalLevelTowersScript.buffTowers + 1;
                break;
            case 5:
                FinalLevelTowersScript.lineAoeTowers = FinalLevelTowersScript.lineAoeTowers + 1;
                break;
            default:
                break;
        }
    }
}

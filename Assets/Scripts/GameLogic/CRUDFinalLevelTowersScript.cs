using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRUDFinalLevelTowersScript : MonoBehaviour
{
    [SerializeField]
    private bool finalLevel;
    [SerializeField]
    private GameObject[] towers;
    // private List<(int towerNumber, GameObject tower)> upgradeCheck;
    private List<(int, GameObject)> upgradeCheck = new List<(int, GameObject)>();

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
        int[] totalTowers = new int[12];
        totalTowers[0] = FinalLevelTowersScript.basicTowers;
        totalTowers[1] = FinalLevelTowersScript.domTowers;
        totalTowers[2] = FinalLevelTowersScript.slowAoeTowers;
        totalTowers[3] = FinalLevelTowersScript.aoeTowers;
        totalTowers[4] = FinalLevelTowersScript.buffTowers;
        totalTowers[5] = FinalLevelTowersScript.lineAoeTowers;
        totalTowers[6] = FinalLevelTowersScript.basicTowersUpgraded;
        totalTowers[7] = FinalLevelTowersScript.domTowersUpgraded;
        totalTowers[8] = FinalLevelTowersScript.slowAoeTowersUpgraded;
        totalTowers[9] = FinalLevelTowersScript.aoeTowersUpgraded;
        totalTowers[10] = FinalLevelTowersScript.buffTowersUpgraded;
        totalTowers[11] = FinalLevelTowersScript.lineAoeTowersUpgraded;

        // Goes through tower types that can be spawned
        for (int t = 0; t < 12; t++)
        {
            // Spawns amount of towers
            for (int n = 0; n < totalTowers[t]; n++)
            {
                GameObject tower;
                // If t > 5, needs to upgrade tower
                if (t > 5)
                {
                    tower = Instantiate(towers[t-6]);
                    tower.GetComponent<TowerClass>().upgradeTower();
                }
                else
                {
                     tower = Instantiate(towers[t]);
                }
                
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

    private void towerCount(int index, GameObject tower)
    {
        switch (index)
        {
            case 0:
                if (tower.GetComponent<TowerClass>().getTier() == 0)
                {
                    FinalLevelTowersScript.basicTowers = FinalLevelTowersScript.basicTowers + 1;
                }
                else
                {
                    FinalLevelTowersScript.basicTowersUpgraded = FinalLevelTowersScript.basicTowersUpgraded + 1;
                }
                break;
            case 1:
                if (tower.GetComponent<TowerClass>().getTier() == 0)
                {
                    FinalLevelTowersScript.domTowers = FinalLevelTowersScript.domTowers + 1;
                }
                else
                {
                    FinalLevelTowersScript.domTowersUpgraded = FinalLevelTowersScript.domTowersUpgraded + 1;
                }
                break;
            case 2:
                if (tower.GetComponent<TowerClass>().getTier() == 0)
                {
                    FinalLevelTowersScript.slowAoeTowers = FinalLevelTowersScript.slowAoeTowers + 1;
                }
                else
                {
                    FinalLevelTowersScript.slowAoeTowersUpgraded = FinalLevelTowersScript.slowAoeTowersUpgraded + 1;
                }
                break;
            case 3:
                if (tower.GetComponent<TowerClass>().getTier() == 0)
                {
                    FinalLevelTowersScript.aoeTowers = FinalLevelTowersScript.aoeTowers + 1;
                }
                else
                {
                    FinalLevelTowersScript.aoeTowersUpgraded = FinalLevelTowersScript.aoeTowersUpgraded + 1;
                }
                break;
            case 4:
                if (tower.GetComponent<TowerClass>().getTier() == 0)
                {
                    FinalLevelTowersScript.buffTowers = FinalLevelTowersScript.buffTowers + 1;
                }
                else
                {
                    FinalLevelTowersScript.buffTowersUpgraded = FinalLevelTowersScript.buffTowersUpgraded + 1;
                }
                break;
            case 5:
                if (tower.GetComponent<TowerClass>().getTier() == 0)
                {
                    FinalLevelTowersScript.lineAoeTowers = FinalLevelTowersScript.lineAoeTowers + 1;
                }
                else
                {
                    FinalLevelTowersScript.lineAoeTowersUpgraded = FinalLevelTowersScript.lineAoeTowersUpgraded + 1;
                }
                break;
            default:
                break;
        }
    }

    public void TrackTowers(int towerNumber, GameObject tower)
    {
        upgradeCheck.Add((towerNumber, tower));
    }

    public void CheckUpgrades()
    {
        for (int i = 0; i < upgradeCheck.Count; i++)
        {
            (int towerNumber, GameObject tower) t = upgradeCheck[i];
            towerCount(t.towerNumber, t.tower);
        }
    }
}

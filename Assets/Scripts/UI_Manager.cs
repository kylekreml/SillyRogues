using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] GameManager gameManager;
    int totalGold;
    int currentGold;
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        totalGold = gameManager.GetGold();
        currentGold = totalGold;
        slider = this.transform.GetChild(0).Find("GoldBar").gameObject.GetComponent<Slider>();
        slider.value = currentGold / totalGold;
    }

    // Update is called once per frame
    void Update()
    {
        currentGold = gameManager.GetGold();
        slider.value = (float)currentGold / (float)totalGold;
    }
}

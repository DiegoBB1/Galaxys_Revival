using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stats_UI : MonoBehaviour
{
    private Transform container;
    private Transform statTemplate;
    ShipHubHandler shipHubHandler;

    [SerializeField] List<Sprite> statIcons;

    List<GameObject> statItems = new List<GameObject>();

    public void Awake(){
        container = transform.Find("StatContainer");
        statTemplate = container.Find("statTemplate");
        statTemplate.gameObject.SetActive(false);

    }
    
    public void Start(){
        // shipHubHandler = GameObject.Find("ShipHubHandler").GetComponent<ShipHubHandler>();
    }

    public void FillStats(){   
        CreateStat("Health", Player.playerHealth, statIcons.ElementAt(0), 0);
        CreateStat("Defense", Player.defense, statIcons.ElementAt(1), 1);
        CreateStat("Damage", Player.strength, statIcons.ElementAt(2), 2);
        CreateStat("Speed", Player.playerSpeed, statIcons.ElementAt(3), 3);
        CreateStat("Fire Rate", (float)System.Math.Round(1/Player.projectileCooldown,1), statIcons.ElementAt(4), 4);
        CreateStat("P. Speed", Player.projectileSpeed, statIcons.ElementAt(5), 5);
        CreateStat("Vitality", Player.maxHealth, statIcons.ElementAt(6), 6);
        CreateStat("Luck", Player.luck * 10, statIcons.ElementAt(7), 7);
    }

    public void CreateStat(string statName, float statValue, Sprite statIcon, int index){
        Transform statTransform = Instantiate(statTemplate, container);
        RectTransform statRectTransform = statTransform.GetComponent<RectTransform>();
        statItems.Add(statTransform.gameObject);
        float statHeight = 70f;

        statRectTransform.anchoredPosition = new Vector2(0, 245 + (-statHeight * index));

        statTransform.Find("StatValue").GetComponent<TextMeshProUGUI>().SetText(statName + ": " + statValue);
        statTransform.Find("StatImage").GetComponent<Image>().sprite = statIcon;

        statTransform.gameObject.SetActive(true);
    }

    public void ClearStats(){
 
        foreach (var stat in statItems) {
            Destroy(stat);
        }
        statItems.Clear();
    }
    

}

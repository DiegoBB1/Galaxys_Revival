using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{

    [SerializeField] List<Sprite> passiveIcons;
    [SerializeField] List<Sprite> abilityIcons;
    [SerializeField] List<Sprite> weaponIcons;
    [SerializeField] GameObject inventorySlot;
    [SerializeField] Sprite emptySlot;
    private Transform inventorySlotTemplate;
    List<GameObject> weapons = new List<GameObject>();

    public void Awake(){
        inventorySlotTemplate = transform.Find("InventorySlot");
        inventorySlotTemplate.gameObject.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        // gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillInventory(string itemType){

        if(itemType == "Weapons"){
            foreach(var item in Player.weaponsPurchased){
                GameObject newItem = Instantiate(inventorySlot);
                Image image = newItem.transform.Find("ItemIcon").GetComponent<Image>();
                switch(item) {
                    case "Default":
                        image.sprite = weaponIcons.ElementAt(0);
                        break;
                    case "Pierce Shot":
                        image.sprite = weaponIcons.ElementAt(1);
                        break;
                    case "Multi Shot":
                        image.sprite = weaponIcons.ElementAt(2);
                        break;
                    case "Charge Shot":
                        image.sprite = weaponIcons.ElementAt(3);
                        break;
                    case "Poison Shot":
                        image.sprite = weaponIcons.ElementAt(4);
                        break;
                    case "Explosive Shot":
                        image.sprite = weaponIcons.ElementAt(5);
                        break;
                }
                image.tag = "Weapon";
                newItem.transform.SetParent(gameObject.transform, false);
                weapons.Add(newItem.gameObject);
            }
        }
        else if(itemType == "Passives"){
            foreach(var item in Player.passivesPurchased){
                GameObject newItem = Instantiate(inventorySlot);
                Image image = newItem.transform.Find("ItemIcon").GetComponent<Image>();
                switch(item) {
                    case "None":
                        image.sprite = emptySlot;
                        break;
                    case "Shield":
                        image.sprite = passiveIcons.ElementAt(0);
                        break;
                    case "Slow Shot":
                        image.sprite = passiveIcons.ElementAt(1);
                        break;
                    case "Weakening Shot":
                        image.sprite = passiveIcons.ElementAt(2);
                        break;
                    case "Self Res":
                        image.sprite = passiveIcons.ElementAt(3);
                        break;
                }
                image.tag = "Passive";
                newItem.transform.SetParent(gameObject.transform, false);
                weapons.Add(newItem.gameObject);
            }
        }
        else if(itemType == "Abilities"){
            foreach(var item in Player.abilitiesPurchased){
                GameObject newItem = Instantiate(inventorySlot);
                Image image = newItem.transform.Find("ItemIcon").GetComponent<Image>();
                switch(item) {
                    case "None":
                        image.sprite = emptySlot;
                        break;
                    case "Cloaking Device":
                        image.sprite = abilityIcons.ElementAt(0);
                        break;
                    case "Time Slow":
                        image.sprite = abilityIcons.ElementAt(1);
                        break;
                    case "Credit Gen":
                        image.sprite = abilityIcons.ElementAt(2);
                        break;
                    case "Health Gen":
                        image.sprite = abilityIcons.ElementAt(3);
                        break;
                }
                image.tag = "Ability";
                newItem.transform.SetParent(gameObject.transform, false);
                weapons.Add(newItem.gameObject);
            }
        }
       
    }

    public Sprite GetEquippedSprite(string itemType){
        if(itemType == "Weapon"){
            switch(Player.weaponEquipped) {
                case "Default":
                    return weaponIcons.ElementAt(0);
                case "Pierce Shot":
                    return weaponIcons.ElementAt(1);
                case "Multi Shot":
                    return weaponIcons.ElementAt(2);
                case "Charge Shot":
                    return weaponIcons.ElementAt(3);
                case "Poison Shot":
                    return weaponIcons.ElementAt(4);
                case "Explosive Shot":
                    return weaponIcons.ElementAt(5);
            }
        }
        else if(itemType == "Passive"){
            
            switch(Player.passiveEquipped) {
                case "None":
                    return emptySlot;
                case "Shield":
                    return passiveIcons.ElementAt(0);
                case "Slow Shot":
                    return passiveIcons.ElementAt(1);
                case "Weakening Shot":
                    return passiveIcons.ElementAt(2);
                case "Self Res":
                    return passiveIcons.ElementAt(3);
            }

            
        }
        else if(itemType == "Ability"){

            switch(Player.abilityEquipped) {
                case "None":
                    return emptySlot;
                case "Cloaking Device":
                    return abilityIcons.ElementAt(0);
                case "Time Slow":
                    return abilityIcons.ElementAt(1);
                case "Credit Gen":
                    return abilityIcons.ElementAt(2);
                case "Health Gen":
                    return abilityIcons.ElementAt(3);
            }
        }
        return emptySlot;
    }

    public void ClearInventory(){

        foreach (var item in weapons) {
            Destroy(item);
        }
        weapons.Clear();
    }

}

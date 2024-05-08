using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour, IDropHandler
{
    private Transform itemIcon;
    private Image image;
    [SerializeField] public string slotType;
    public void Awake(){
        itemIcon = transform.Find("Icon");
        image = itemIcon.GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Check if the dragged object is valid and that the item type matches the slot type
        if(eventData.pointerDrag != null && slotType == eventData.pointerDrag.GetComponent<DragItem>().itemType){
            image.sprite = eventData.pointerDrag.GetComponent<DragItem>().sprite;

            if(slotType == "Weapon"){
                switch(image.sprite.name){
                    case "weapons_0":
                        Player.weaponEquipped = "Default";
                        Debug.Log("Default Weapon Equipped");
                        break;
                    case "weapons_1":
                        Player.weaponEquipped = "Charge Shot";
                        Debug.Log("Charge Shot Equipped");
                        break;
                    case "weapons_2":
                        Player.weaponEquipped = "Pierce Shot";
                        Debug.Log("Pierce Shot Equipped");
                        break;
                    case "weapons_3":
                        Player.weaponEquipped = "Poison Shot";
                        Debug.Log("Poison Shot Equipped");
                        break;
                    case "weapons_4":
                        Player.weaponEquipped = "Explosive Shot";
                        Debug.Log("Explosive Shot Equipped");
                        break;
                    case "weapons_5":
                        Player.weaponEquipped = "Multi Shot";
                        Debug.Log("Multi Shot Equipped");
                        break;
                }
            }
            else if(slotType == "Passive"){
                switch(image.sprite.name){
                    case "ability_icons_0":
                        Player.passiveEquipped = "None";
                        Debug.Log("None Equipped");
                        break;
                    case "ability_icons_2":
                        Player.passiveEquipped = "Shield";
                        Debug.Log("Shield Equipped");
                        break;
                    case "ability_icons_1":
                        Player.passiveEquipped = "Weakening Shot";
                        Debug.Log("Weakening Shot Equipped");
                        break;
                    case "ability_icons_4":
                        Player.passiveEquipped = "Slow Shot";
                        Debug.Log("Slow Shot Equipped");
                        break;
                    case "ability_icons_5":
                        Player.passiveEquipped = "Self Res";
                        Debug.Log("Self Res Equipped");
                        break;
                }
            }
            else if(slotType == "Ability"){
                switch(image.sprite.name){
                    case "ability_icons_0":
                        Player.abilityEquipped = "None";
                        Debug.Log("None Equipped");
                        break;
                    case "ability_icons_8":
                        Player.abilityEquipped = "Cloaking Device";
                        Debug.Log("Cloaking Device Equipped");
                        break;
                    case "ability_icons_3":
                        Player.abilityEquipped = "Time Slow";
                        Debug.Log("Time Slow Equipped");
                        break;
                    case "ability_icons_7":
                        Player.abilityEquipped = "Credit Gen";
                        Debug.Log("Credit Gen Equipped");
                        break;
                    case "ability_icons_6":
                        Player.abilityEquipped = "Health Gen";
                        Debug.Log("Health Gen Equipped");
                        break;
                }                
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

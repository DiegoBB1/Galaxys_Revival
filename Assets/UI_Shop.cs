using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    ShipHubHandler shipHubHandler;
    private bool statMaxed;
    public void Awake(){
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }
    
    public void Start(){
        shipHubHandler = GameObject.Find("ShipHubHandler").GetComponent<ShipHubHandler>();
        createItemButton("Health", 25, "Gives +2 HP", -500, 0);
        createItemButton("Increased Fire Rate", 75, "Reduces time between projectiles fired by the player", -500, 1);
        createItemButton("Increased Damage", 125, "Increases the amount of damage done by projectiles", -500, 2);
        createItemButton("Increased Speed", 50, "Increases the movement speed of the player", 500, 0);
        createItemButton("Increased Defense", 100, "Decreases the amount of damage taken when hit by enemies", 500, 1);
        createItemButton("Piercing Projectiles", 200, "Allows for fired projectiles to pierce enemies to hit multiple targets", 500, 2);

        //Creates tooltips for shop items

    }

    public void createItemButton(string itemName, int itemPrice, string itemDesc, int xVal, int index){
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float itemHeight = 225f;

        shopItemRectTransform.anchoredPosition = new Vector2(xVal, 250 + (-itemHeight * index));

        shopItemTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("ItemPrice").GetComponent<TextMeshProUGUI>().SetText("Price: " + itemPrice +"c");
        shopItemTransform.Find("ItemDesc").GetComponent<TextMeshProUGUI>().SetText(itemDesc);

        shopItemTransform.gameObject.SetActive(true);
        shopItemTransform.GetComponent<Button>().onClick.AddListener(delegate {itemClicked(itemName,itemPrice); });
    }


    public void itemClicked(string itemName, int itemPrice){
        if(Player.currency >= itemPrice){
            statMaxed = false;
            //Grant upgrade
            switch(itemName) {
            case "Health":
                if(Player.playerHealth < 10){
                    if(Player.playerHealth + 2 > 10)
                        Player.playerHealth = 10;
                    else
                        Player.playerHealth += 2;

                    Debug.Log("Health Purchased");
                    Tooltip.showTooltip_Static("Health Increased");
                }
                else{
                    statMaxed = true;
                    Debug.Log("Max Health Reached");
                    Tooltip.showTooltip_Static("Max Health Reached");
                }
                break;
            case "Increased Fire Rate":
                if(Player.projectileCooldown > 0.4f){
                    Player.projectileCooldown -= 0.2f;
                    Debug.Log("Increased Fire Rate Purchased");
                    Tooltip.showTooltip_Static("Fire Rate Increased");
                }
                else{
                    statMaxed = true;
                    Debug.Log("Max Fire Rate Reached");
                    Tooltip.showTooltip_Static("Max Fire Rate Reached");
                }
                break;
            case "Increased Damage":
                if(Player.strength < 4){
                    Player.strength++;
                    Debug.Log("Increased Damage Purchased");
                    Tooltip.showTooltip_Static("Damage Increased");
                }
                else{
                    statMaxed = true;
                    Debug.Log("Max Damage Reached");
                    Tooltip.showTooltip_Static("Max Damage Reached");
                }
                break;
            case "Increased Speed":
                if(Player.playerSpeed < 10){
                    Player.playerSpeed++;
                    Debug.Log("Increased Speed Purchased");
                    Tooltip.showTooltip_Static("Speed Increased");
                    
                }
                else{
                    statMaxed = true;
                    Debug.Log("Max Speed Reached");
                    Tooltip.showTooltip_Static("Max Speed Reached");
                }
                break;
            case "Increased Defense":
                if(Player.defense < 4){
                    Player.defense++;
                    Debug.Log("Increased Defense Purchased");
                    Tooltip.showTooltip_Static("Defense Increased");
                }
                else{
                    statMaxed = true;
                    Debug.Log("Max Defense Reached");
                    Tooltip.showTooltip_Static("Max Defense Reached");
                }
                break;
            case "Piercing Projectiles":
                // Implement piercing projectiles and give to player
                if(Player.pierceProjectiles == true){
                    statMaxed = true;
                    Tooltip.showTooltip_Static("Piercing Projectiles Already Owned");
                    Debug.Log("Piercing Projectiles Already Owned");
                }
                else{
                    Player.pierceProjectiles = true;
                    Tooltip.showTooltip_Static("Piercing Projectiles Purchased");
                    Debug.Log("Piercing Projectiles Purchased");
                }
                break;
            }

            //Decrement player currency

            if(!statMaxed){
                Player.currency = Player.currency - itemPrice;
                shipHubHandler.currencyCount.text = "Total Credits: " + Player.currency + "c";
            }
        }
        else{
            //Give message saying that player does not have enough money to purchase upgrade
            Debug.Log("You do not have enough currency to buy this upgrade");
            Tooltip.showTooltip_Static("You do not have enough currency to buy this upgrade");
        }
    }
}

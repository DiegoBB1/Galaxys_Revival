using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    ShipHubHandler shipHubHandler;

    private List<Shop_Item> stat_upgrades = new List<Shop_Item>();
    private List<Shop_Item> weapon_upgrades = new List<Shop_Item>();
    private List<Shop_Item> passive_upgrades = new List<Shop_Item>();
    private List<Shop_Item> ability_upgrades = new List<Shop_Item>();

    public struct Shop_Item
    {
        public string image_path;
        public string item_name;
        public int item_price;
        public string item_desc;

        public Shop_Item(string path, string name, int price, string desc){
            image_path = path;
            item_name = name;
            item_price = price;
            item_desc = desc;
        }
    }
    public void Awake(){
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);

        // On awake, will create each of the items of the shop and store their reference in a list to be called upon shop generation

        //Stat Upgrades
        Shop_Item health = new Shop_Item("", "Health", 25, "Gives +2 HP");
        Shop_Item fireRate = new Shop_Item("", "Increased Fire Rate", 75, "Reduces time between projectiles fired by the player");        
        Shop_Item damage = new Shop_Item("", "Increased Damage", 125, "Increases the amount of damage done by projectiles");        
        Shop_Item speed = new Shop_Item("", "Increased Speed", 50, "Increases the movement speed of the player");        
        Shop_Item defense = new Shop_Item("", "Increased Defense", 100, "Decreases the amount of damage taken when hit by enemies");        
        Shop_Item projectileSpeed = new Shop_Item("", "Faster Projectiles", 25, "Increases the speed of the projectiles fired by the player");        
        Shop_Item vitality = new Shop_Item("", "Increased Vitality", 25, "Increases the maximum health of the player");        
        Shop_Item luck = new Shop_Item("", "Increased Luck", 25, "Increases the chance of earning bonus credits on mission complete");    

        stat_upgrades.AddRange(new List<Shop_Item>(){health, fireRate, damage, speed, defense, projectileSpeed, vitality, luck});

        //Passive Upgrades
        Shop_Item shield = new Shop_Item("", "Regenerative Shield", 0, "If the player is hit, the shield will nullify damage, temporarily breaking it before use again");
        Shop_Item slowProjectiles = new Shop_Item("", "Slowing Projectiles", 0, "On contact, projectile will temporarily slow target");
        Shop_Item hinderProjectiles = new Shop_Item("", "Weakening Projectiles", 0, "On contact, projectile will temporarily reduce damage of the target");
        Shop_Item selfRes = new Shop_Item("", "Self Resurrection", 0, "Upon death, player will be brought back to life, with 5 HP");

        passive_upgrades.AddRange(new List<Shop_Item>() {shield, slowProjectiles, hinderProjectiles, selfRes});

        //Ability Upgrades
        Shop_Item slowTime = new Shop_Item("", "Time Slow", 0, "When activated, time is temporarily slowed. (Cooldown: 60 seconds)");
        Shop_Item cloakDevice = new Shop_Item("", "Cloaking Device", 0, "When activated, enemies will ignore the player. (Cooldown: 60 seconds)");
        Shop_Item creditGen = new Shop_Item("", "Credit Generator", 0, "When activated, all enemies defeated reward credits. Can use once per mission");
        Shop_Item healthGen = new Shop_Item("", "Health Generator", 0, "When activated, defeating enemies heals you. Can use once per mission");
        // Shop_Item slowEnemies = new Shop_Item("", "Piercing Projectiles", 200, "Allows for fired projectiles to pierce enemies to hit multiple targets");

        ability_upgrades.AddRange(new List<Shop_Item>() {slowTime, cloakDevice, creditGen, healthGen});

        //Weapon Upgrades
        Shop_Item piercingShot = new Shop_Item("", "Pierce Shot", 200, "Projectiles fired will pierce enemies, hitting multiple targets");
        Shop_Item multiShot = new Shop_Item("", "Multi Shot", 0, "Weapon capable of firing three projectiles forward in cone shape");
        Shop_Item chargeShot = new Shop_Item("", "Charge Shot", 0, "Projectiles can be charged and released, dealing additional damage");
        Shop_Item poisonShot = new Shop_Item("", "Poison Shot", 0, "Projectiles poison targets, dealing damage over time");
        Shop_Item explosiveShot = new Shop_Item("", "Explosive Shot", 0, "Projectiles explode on contact, dealing damage to nearby targets");

        weapon_upgrades.AddRange(new List<Shop_Item>() {piercingShot, multiShot, chargeShot, poisonShot, explosiveShot});

    }
    
    public void Start(){
        //TODO: Have start function check whether it should refresh the shop, if so, would call function to randomly generate new shop
        // Shop should only be reset on system launch?

        shipHubHandler = GameObject.Find("ShipHubHandler").GetComponent<ShipHubHandler>();
        fillShop();
    }

    public void fillShop(){
        //TODO: Randomly select 6 upgrdades: 3 stat upgrades, 1 passive upgrade, 1 ability upgrade, and 1 weapon upgrades. Populate the shop with these six upgrades.

        //Use shuffle algorithm to rearrange the stat list randomly and choose the first three shop items to be displayed in the shop
        //Randomly select 1 upgrade from the passive, ability, and weapon upgrade pools
        Shuffle(stat_upgrades);
        int randomWeapon = Random.Range(0,4);
        int randomPassive = Random.Range(0,3);
        int randomAbility = Random.Range(0,3);
    
        createItemButton(stat_upgrades.ElementAt(0), -500, 0);
        createItemButton(stat_upgrades.ElementAt(1), -500, 1);
        createItemButton(ability_upgrades.ElementAt(0), -500, 2);
        createItemButton(stat_upgrades.ElementAt(2), 500, 0);
        createItemButton(passive_upgrades.ElementAt(randomPassive), 500, 1);
        createItemButton(weapon_upgrades.ElementAt(1), 500, 2);
    }

    public void createItemButton(Shop_Item item, int xVal, int index){
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float itemHeight = 225f;

        shopItemRectTransform.anchoredPosition = new Vector2(xVal, 250 + (-itemHeight * index));

        shopItemTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(item.item_name);
        shopItemTransform.Find("ItemPrice").GetComponent<TextMeshProUGUI>().SetText("Price: " + item.item_price +"c");
        shopItemTransform.Find("ItemDesc").GetComponent<TextMeshProUGUI>().SetText(item.item_desc);

        shopItemTransform.gameObject.SetActive(true);
        shopItemTransform.GetComponent<Button>().onClick.AddListener(delegate {itemClicked(item.item_name, item.item_price); });
    }


    public void itemClicked(string itemName, int itemPrice){
        //Checks if a stat is maxed or if a single-type upgrade has already been purchased
        if(checkStatMaxed(itemName)){
            Debug.Log("Upgrade already owned");
            return;
        }

        if(Player.currency >= itemPrice){
            //Grant upgrade
            switch(itemName) {
                //The following 8 cases handle the stat upgrades
                case "Health":
                    Player.playerHealth = Player.playerHealth + 2 > 10 ? 10: Player.playerHealth + 2;
                    Debug.Log("Health Purchased");
                    Tooltip.showTooltip_Static("Health Increased");
                    break;
                case "Increased Fire Rate":
                    Player.projectileCooldown -= 0.2f;
                    Debug.Log("Increased Fire Rate Purchased");
                    Tooltip.showTooltip_Static("Fire Rate Increased");
                    break;
                case "Increased Damage":
                    Player.strength++;
                    Debug.Log("Increased Damage Purchased");
                    Tooltip.showTooltip_Static("Damage Increased");
                    break;
                case "Increased Speed":
                    Player.playerSpeed++;
                    Debug.Log("Increased Speed Purchased");
                    Tooltip.showTooltip_Static("Speed Increased");
                    break;
                case "Increased Defense":
                    Player.defense++;
                    Debug.Log("Increased Defense Purchased");
                    Tooltip.showTooltip_Static("Defense Increased");
                    break;
                case "Faster Projectiles":
                    Player.projectileSpeed += .5f;
                    Debug.Log("Faster Projectiles Purchased");
                    Tooltip.showTooltip_Static("Projectile Speed Increased");
                    break;
                case "Increased Vitality":
                    Player.maxHealth = Player.maxHealth + 2 > 10 ? 10: Player.maxHealth + 2;
                    Debug.Log("Increased Vitality Purchased");
                    Tooltip.showTooltip_Static("Vitality Increased");
                    break;
                case "Increased Luck":
                    Player.luck += .1f;
                    Debug.Log("Increased Luck Purchased");
                    Tooltip.showTooltip_Static("Luck Increased");
                    break;

                //The following 4 cases handle the passive upgrades
                case "Regenerative Shield":
                    Player.passiveEquipped = "Shield";
                    Debug.Log("Regenerative Shield Purchased");
                    Tooltip.showTooltip_Static("Regenerative Shield Purchased");                    
                    break;
                case "Slowing Projectiles":
                    Player.passiveEquipped = "Slow Shot";
                    Debug.Log("Slowing Projectiles Purchased");
                    Tooltip.showTooltip_Static("Slowing Projectiles Purchased");
                    break;
                case "Weakening Projectiles":
                    Player.passiveEquipped = "Weakening Shot";
                    Debug.Log("Weakening Projectiles Purchased");
                    Tooltip.showTooltip_Static("Weakening Projectiles Purchased");
                    break;
                case "Self Resurrection":
                    Player.passiveEquipped = "Self Res";
                    Debug.Log("Self Resurrection Purchased");
                    Tooltip.showTooltip_Static("Self Resurrection Purchased");
                    break;

                //The following 4 cases handle the ability upgrades
                case "Cloaking Device":
                    Player.abilityEquipped = "Cloaking Device";
                    Debug.Log("Cloaking Device Purchased");
                    Tooltip.showTooltip_Static("Cloaking Device Purchased");
                    break;
                case "Time Slow":
                    Player.abilityEquipped = "Time Slow";
                    Debug.Log("Time Slow Purchased");
                    Tooltip.showTooltip_Static("Time Slow Purchased");
                    break;
                case "Credit Generator":
                    Player.abilityEquipped = "Credit Gen";
                    Debug.Log("Credit Generator Purchased");
                    Tooltip.showTooltip_Static("Credit Generator Purchased");
                    break;
                case "Health Generator":
                    Player.abilityEquipped = "Health Gen";
                    Debug.Log("Health Generator Purchased");
                    Tooltip.showTooltip_Static("Health Generator Purchased");
                    break;

                //The following 5 cases handle the weapon upgrades
                case "Pierce Shot":
                    Player.weaponEquipped = "Pierce Shot";
                    Tooltip.showTooltip_Static("Piercing Projectiles Purchased");
                    Debug.Log("Piercing Projectiles Purchased");
                    break;
                case "Multi Shot":
                    Player.weaponEquipped = "Multi Shot";
                    Tooltip.showTooltip_Static("Multi Shot Purchased");
                    Debug.Log("Multi Shot Purchased");
                    break;
                case "Charge Shot":
                    Player.weaponEquipped = "Charge Shot";
                    Tooltip.showTooltip_Static("Charge Shot Purchased");
                    Debug.Log("Charge Shot Purchased");
                    break;
                case "Poison Shot":
                    Player.weaponEquipped = "Poison Shot";
                    Tooltip.showTooltip_Static("Poison Shot Purchased");
                    Debug.Log("Poison Shot Purchased");
                    break;
            }

            //Decrement player currency
            Player.currency = Player.currency - itemPrice;
            shipHubHandler.currencyCount.text = "Total Credits: " + Player.currency + "c";
    
        }
        else{
            //Give message saying that player does not have enough money to purchase upgrade
            Debug.Log("You do not have enough currency to buy this upgrade");
            Tooltip.showTooltip_Static("You do not have enough currency to buy this upgrade");
        }
    }

    public bool checkStatMaxed(string upgradeName){
        switch(upgradeName){
            //The following 8 cases check if the player stat is maxed out
            case "Health":
                if(Player.playerHealth < Player.maxHealth)
                    return false;
                else
                    return true;
            case "Increased Fire Rate":
                if(Player.projectileCooldown > 0.4f)
                    return false;
                else
                    return true;
            case "Increased Damage":
                if(Player.strength < 4)
                    return false;
                else
                    return true;
            case "Increased Speed":
                if(Player.playerSpeed < 10)
                    return false;
                else
                    return true;
            case "Increased Defense":
                if(Player.defense < 4)
                    return false;
                else
                    return true;
            case "Faster Projectiles":
                if(Player.projectileSpeed < 6)
                    return false;
                else
                    return true;            
            case "Increased Vitality":
                if(Player.maxHealth < 10)
                    return false;
                else
                    return true;            
            case "Increased Luck":
                if(Player.luck < .5f)
                    return false;
                else
                    return true;
            //The following 4 cases check if the passive upgrade has been purchased already

            //The following 5 cases check if the ability upgrade has been purchased already

            //The following 5 cases check if the ability upgrade has been purchased already
            case "Piercing Projectiles":
                if(Player.weaponEquipped == "Pierce Shot")
                    return true;
                else
                    return false;
        }
        return false;
    }
    public static void Shuffle<T>(IList<T> list){  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = Random.Range(0, n + 1); 
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}

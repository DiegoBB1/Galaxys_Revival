using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [SerializeField] List<Sprite> statIcons;
    [SerializeField] List<Sprite> passiveIcons;
    [SerializeField] List<Sprite> abilityIcons;
    [SerializeField] List<Sprite> weaponIcons;

    public struct Shop_Item
    {
        public Sprite item_icon;
        public string item_name;
        public int item_price;
        public string item_desc;

        public Shop_Item(Sprite sprite, string name, int price, string desc){
            item_icon = sprite;
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
        Shop_Item health = new Shop_Item(statIcons.ElementAt(0), "Health", 25, "Gives +2 HP");
        Shop_Item fireRate = new Shop_Item(statIcons.ElementAt(1), "Increased Fire Rate", 75, "Reduces time between projectiles fired by the player");        
        Shop_Item damage = new Shop_Item(statIcons.ElementAt(2), "Increased Damage", 125, "Increases the amount of damage done by projectiles");        
        Shop_Item speed = new Shop_Item(statIcons.ElementAt(3), "Increased Speed", 50, "Increases the movement speed of the player");        
        Shop_Item defense = new Shop_Item(statIcons.ElementAt(4), "Increased Defense", 100, "Decreases the amount of damage taken when hit by enemies");        
        Shop_Item projectileSpeed = new Shop_Item(statIcons.ElementAt(5), "Faster Projectiles", 25, "Increases the speed of the projectiles fired by the player");        
        Shop_Item vitality = new Shop_Item(statIcons.ElementAt(6), "Increased Vitality", 25, "Increases the maximum health of the player");        
        Shop_Item luck = new Shop_Item(statIcons.ElementAt(7), "Increased Luck", 25, "Increases the chance of earning bonus credits on mission complete");    

        stat_upgrades.AddRange(new List<Shop_Item>(){health, fireRate, damage, speed, defense, projectileSpeed, vitality, luck});

        //Passive Upgrades
        Shop_Item shield = new Shop_Item(passiveIcons.ElementAt(0), "Regenerative Shield", 0, "If the player is hit, the shield will nullify damage, temporarily breaking it before use again");
        Shop_Item slowProjectiles = new Shop_Item(passiveIcons.ElementAt(1), "Slowing Projectiles", 0, "On contact, projectile will temporarily slow target");
        Shop_Item hinderProjectiles = new Shop_Item(passiveIcons.ElementAt(2), "Weakening Projectiles", 0, "On contact, projectile will temporarily reduce damage of the target");
        Shop_Item selfRes = new Shop_Item(passiveIcons.ElementAt(3), "Self Resurrection", 0, "Upon death, player will be brought back to life, with 5 HP");

        passive_upgrades.AddRange(new List<Shop_Item>() {shield, slowProjectiles, hinderProjectiles, selfRes});

        //Ability Upgrades
        Shop_Item slowTime = new Shop_Item(abilityIcons.ElementAt(0), "Time Slow", 0, "When activated, time is temporarily slowed. (Cooldown: 60 seconds)");
        Shop_Item cloakDevice = new Shop_Item(abilityIcons.ElementAt(1), "Cloaking Device", 0, "When activated, enemies will ignore the player. (Cooldown: 60 seconds)");
        Shop_Item creditGen = new Shop_Item(abilityIcons.ElementAt(2), "Credit Generator", 0, "When activated, all enemies defeated reward credits. Can use once per mission");
        Shop_Item healthGen = new Shop_Item(abilityIcons.ElementAt(3), "Health Generator", 0, "When activated, defeating enemies heals you. Can use once per mission");

        ability_upgrades.AddRange(new List<Shop_Item>() {slowTime, cloakDevice, creditGen, healthGen});

        //Weapon Upgrades
        Shop_Item piercingShot = new Shop_Item(weaponIcons.ElementAt(0), "Pierce Shot", 200, "Projectiles fired will pierce enemies, hitting multiple targets");
        Shop_Item multiShot = new Shop_Item(weaponIcons.ElementAt(1), "Multi Shot", 0, "Weapon capable of firing three projectiles forward in cone shape");
        Shop_Item chargeShot = new Shop_Item(weaponIcons.ElementAt(2), "Charge Shot", 0, "Projectiles can be charged and released, dealing additional damage");
        Shop_Item poisonShot = new Shop_Item(weaponIcons.ElementAt(3), "Poison Shot", 0, "Projectiles poison targets, dealing damage over time");
        Shop_Item explosiveShot = new Shop_Item(weaponIcons.ElementAt(4), "Explosive Shot", 0, "Projectiles explode on contact, dealing damage to nearby targets");

        weapon_upgrades.AddRange(new List<Shop_Item>() {piercingShot, multiShot, chargeShot, poisonShot, explosiveShot});

    }
    
    public void Start(){
        shipHubHandler = GameObject.Find("ShipHubHandler").GetComponent<ShipHubHandler>();
        FillShop();
    }

    public void FillShop(){

        //Use shuffle algorithm to rearrange the stat list randomly and choose the first three shop items to be displayed in the shop
        //Randomly select 1 upgrade from the passive, ability, and weapon upgrade pools
        Shuffle(stat_upgrades);
        int randomWeapon = Random.Range(0,4);
        int randomPassive = Random.Range(0,3);
        int randomAbility = Random.Range(0,3);
    
        CreateItemButton(stat_upgrades.ElementAt(0), -500, 0);
        CreateItemButton(stat_upgrades.ElementAt(1), -500, 1);
        CreateItemButton(ability_upgrades.ElementAt(randomAbility), -500, 2);
        CreateItemButton(stat_upgrades.ElementAt(2), 500, 0);
        CreateItemButton(passive_upgrades.ElementAt(randomPassive), 500, 1);
        CreateItemButton(weapon_upgrades.ElementAt(randomWeapon), 500, 2);
    }

    public void CreateItemButton(Shop_Item item, int xVal, int index){
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float itemHeight = 225f;

        shopItemRectTransform.anchoredPosition = new Vector2(xVal, 250 + (-itemHeight * index));

        shopItemTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(item.item_name);
        shopItemTransform.Find("ItemPrice").GetComponent<TextMeshProUGUI>().SetText("Price: " + item.item_price +"c");
        shopItemTransform.Find("ItemDesc").GetComponent<TextMeshProUGUI>().SetText(item.item_desc);
        shopItemTransform.Find("ItemImage").GetComponent<Image>().sprite = item.item_icon;

        shopItemTransform.gameObject.SetActive(true);
        shopItemTransform.GetComponent<Button>().onClick.AddListener(delegate {ItemClicked(item.item_name, item.item_price); });
    }


    public void ItemClicked(string itemName, int itemPrice){
        //Checks if a stat is maxed or if a single-type upgrade has already been purchased
        if(CheckStatMaxed(itemName)){
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
                    Tooltip.ShowTooltip_Static("Health Increased");
                    break;
                case "Increased Fire Rate":
                    Player.projectileCooldown -= 0.2f;
                    Debug.Log("Increased Fire Rate Purchased");
                    Tooltip.ShowTooltip_Static("Fire Rate Increased");
                    break;
                case "Increased Damage":
                    Player.strength++;
                    Debug.Log("Increased Damage Purchased");
                    Tooltip.ShowTooltip_Static("Damage Increased");
                    break;
                case "Increased Speed":
                    Player.playerSpeed++;
                    Debug.Log("Increased Speed Purchased");
                    Tooltip.ShowTooltip_Static("Speed Increased");
                    break;
                case "Increased Defense":
                    Player.defense++;
                    Debug.Log("Increased Defense Purchased");
                    Tooltip.ShowTooltip_Static("Defense Increased");
                    break;
                case "Faster Projectiles":
                    Player.projectileSpeed += .5f;
                    Debug.Log("Faster Projectiles Purchased");
                    Tooltip.ShowTooltip_Static("Projectile Speed Increased");
                    break;
                case "Increased Vitality":
                    Player.maxHealth = Player.maxHealth + 2 > 10 ? 10: Player.maxHealth + 2;
                    Debug.Log("Increased Vitality Purchased");
                    Tooltip.ShowTooltip_Static("Vitality Increased");
                    break;
                case "Increased Luck":
                    Player.luck += .1f;
                    Debug.Log("Increased Luck Purchased");
                    Tooltip.ShowTooltip_Static("Luck Increased");
                    break;

                //The following 4 cases handle the passive upgrades
                case "Regenerative Shield":
                    Player.passiveEquipped = "Shield";
                    Player.passivesPurchased.Add("Shield");
                    Debug.Log("Regenerative Shield Purchased");
                    Tooltip.ShowTooltip_Static("Regenerative Shield Purchased");                    
                    break;
                case "Slowing Projectiles":
                    Player.passiveEquipped = "Slow Shot";
                    Player.passivesPurchased.Add("Slow Shot");
                    Debug.Log("Slowing Projectiles Purchased");
                    Tooltip.ShowTooltip_Static("Slowing Projectiles Purchased");
                    break;
                case "Weakening Projectiles":
                    Player.passiveEquipped = "Weakening Shot";
                    Player.passivesPurchased.Add("Weakening Shot");
                    Debug.Log("Weakening Projectiles Purchased");
                    Tooltip.ShowTooltip_Static("Weakening Projectiles Purchased");
                    break;
                case "Self Resurrection":
                    Player.passiveEquipped = "Self Res";
                    Player.passivesPurchased.Add("Self Res");
                    Debug.Log("Self Resurrection Purchased");
                    Tooltip.ShowTooltip_Static("Self Resurrection Purchased");
                    break;

                //The following 4 cases handle the ability upgrades
                case "Cloaking Device":
                    Player.abilityEquipped = "Cloaking Device";
                    Player.abilitiesPurchased.Add("Cloaking Device");
                    Debug.Log("Cloaking Device Purchased");
                    Tooltip.ShowTooltip_Static("Cloaking Device Purchased");
                    break;
                case "Time Slow":
                    Player.abilityEquipped = "Time Slow";
                    Player.abilitiesPurchased.Add("Time Slow");
                    Debug.Log("Time Slow Purchased");
                    Tooltip.ShowTooltip_Static("Time Slow Purchased");
                    break;
                case "Credit Generator":
                    Player.abilityEquipped = "Credit Gen";
                    Player.abilitiesPurchased.Add("Credit Gen");
                    Debug.Log("Credit Generator Purchased");
                    Tooltip.ShowTooltip_Static("Credit Generator Purchased");
                    break;
                case "Health Generator":
                    Player.abilityEquipped = "Health Gen";
                    Player.abilitiesPurchased.Add("Health Gen");
                    Debug.Log("Health Generator Purchased");
                    Tooltip.ShowTooltip_Static("Health Generator Purchased");
                    break;

                //The following 5 cases handle the weapon upgrades
                case "Pierce Shot":
                    Player.weaponEquipped = "Pierce Shot";
                    Player.weaponsPurchased.Add("Pierce Shot");
                    Tooltip.ShowTooltip_Static("Piercing Projectiles Purchased");
                    Debug.Log("Piercing Projectiles Purchased");
                    break;
                case "Multi Shot":
                    Player.weaponEquipped = "Multi Shot";
                    Player.weaponsPurchased.Add("Multi Shot");
                    Tooltip.ShowTooltip_Static("Multi Shot Purchased");
                    Debug.Log("Multi Shot Purchased");
                    break;
                case "Charge Shot":
                    Player.weaponEquipped = "Charge Shot";
                    Player.weaponsPurchased.Add("Charge Shot");
                    Tooltip.ShowTooltip_Static("Charge Shot Purchased");
                    Debug.Log("Charge Shot Purchased");
                    break;
                case "Poison Shot":
                    Player.weaponEquipped = "Poison Shot";
                    Player.weaponsPurchased.Add("Poison Shot");
                    Tooltip.ShowTooltip_Static("Poison Shot Purchased");
                    Debug.Log("Poison Shot Purchased");
                    break;
                case "Explosive Shot":
                    Player.weaponEquipped = "Explosive Shot";
                    Player.weaponsPurchased.Add("Explosive Shot");
                    Tooltip.ShowTooltip_Static("Explosive Shot Purchased");
                    Debug.Log("Explosive Shot Purchased");
                    break;
            }

            //Decrement player currency
            Player.currency = Player.currency - itemPrice;
            shipHubHandler.currencyCount.text = "Total Credits: " + Player.currency + "c";
    
        }
        else{
            //Give message saying that player does not have enough money to purchase upgrade
            Debug.Log("You do not have enough currency to buy this upgrade");
            Tooltip.ShowTooltip_Static("You do not have enough currency to buy this upgrade");
        }
    }

    public bool CheckStatMaxed(string upgradeName){
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
            case "Regenerative Shield":
                if(Player.passivesPurchased.Contains("Shield"))
                    return true;
                else
                    return false;
            case "Slowing Projectiles":
                if(Player.passivesPurchased.Contains("Slow Shot"))
                    return true;
                else
                    return false;
            case "Weakening Projectiles":
                if(Player.passivesPurchased.Contains("Weakening Shot"))
                    return true;
                else
                    return false;
            case "Self Resurrection":
                if(Player.passivesPurchased.Contains("Self Res"))
                    return true;
                else
                    return false;
            //The following 4 cases check if the ability upgrade has been purchased already
            case "Cloaking Device":
                if(Player.abilitiesPurchased.Contains("Cloaking Device"))
                    return true;
                else
                    return false;
            case "Time Slow":
                if(Player.abilitiesPurchased.Contains("Time Slow"))
                    return true;
                else
                    return false;
            case "Credit Generator":
                if(Player.abilitiesPurchased.Contains("Credit Gen"))
                    return true;
                else
                    return false;
            case "Health Generator":
                if(Player.abilitiesPurchased.Contains("Health Gen"))
                    return true;
                else
                    return false;
            //The following 5 cases check if the ability upgrade has been purchased already
            case "Pierce Shot":
                if(Player.weaponsPurchased.Contains("Pierce Shot"))
                    return true;
                else 
                    return false;
            case "Multi Shot":
                if(Player.weaponsPurchased.Contains("Multi Shot"))
                    return true;
                else 
                    return false;
            case "Charge Shot":
                if(Player.weaponsPurchased.Contains("Charge Shot"))
                    return true;
                else 
                    return false;
            case "Poison Shot":
                if(Player.weaponsPurchased.Contains("Poison Shot"))
                    return true;
                else 
                    return false;
            case "Explosive Shot":
                if(Player.weaponsPurchased.Contains("Explosive Shot"))
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

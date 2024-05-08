using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
public class ShipHubHandler : MonoBehaviour
{

    [SerializeField] private ZoomTransition zoomTransition;

    [SerializeField] public TextMeshProUGUI currencyCount;
    [SerializeField] public TextMeshProUGUI encounterText;

    [SerializeField] public TextMeshProUGUI planetOneText;
    [SerializeField] public TextMeshProUGUI planetTwoText;
    [SerializeField] public TextMeshProUGUI planetThreeText;
    [SerializeField] public Stats_UI stats_UI;
    [SerializeField] public Inventory_UI inventory_UI;

    public Camera mainCamera;

    public GameObject mainCanvas;
    public GameObject shopCanvas;
    public GameObject planetSelectCanvas;
    public GameObject armoryCanvas;

    public static int planetsLiberated = 0;
    public static int planetsRequired = 3;
    public static int currentGrid = 1;
    public static List<string> gridTypes = new List<string> {"Passives", "Weapons", "Abilities"};
    [SerializeField] public TextMeshProUGUI leftText;
    [SerializeField] public TextMeshProUGUI middleText;

    [SerializeField] public TextMeshProUGUI rightText;
    [SerializeField] public Image weaponImage;
    [SerializeField] public Image passiveImage;
    [SerializeField] public Image abilityImage;

    private List<string> planetNames = new List<string> {"Hyperion IX", "Titanus", "Orionis", "Elysium", "Lumina", "Pulsaria", "Arcadia", "Astralyn", "Seraphim IV", "Chronosia", "Olympus XI", "Stellara"};

    
    [SerializeField] public Image backgroundImage;

    [SerializeField] public List<Sprite> backgroundImages;
    [SerializeField] public AudioClip planetLaunchSFX;
    [SerializeField] public AudioClip systemLaunchSFX;
    private AudioSource audioSource;
    [SerializeField] public Image easyPlanetImage;
    [SerializeField] public Image mediumPlanetImage;
    [SerializeField] public Image hardPlanetImage;
    [SerializeField] public List<Sprite> grassPlanetImages;
    [SerializeField] public List<Sprite> desertPlanetImages;
    [SerializeField] public List<Sprite> rockPlanetImages;
    [SerializeField] public List<Sprite> snowPlanetImages;
    private List<string> planetTypes = new List<string> {"Grass", "Rock", "Desert", "Snow"};

    public void Awake(){
        audioSource = GetComponent<AudioSource>();  
        backgroundImage.sprite = backgroundImages.ElementAt(Random.Range(0, backgroundImages.Count));
        UI_Shop.Shuffle(planetTypes);
    }

    // Start is called before the first frame update
    void Start()
    {
        UI_Shop.Shuffle(planetNames);
        planetOneText.text = "Planet: " + planetNames.ElementAt(0) + "\nThreat Level: C";
        planetTwoText.text = "Planet: " + planetNames.ElementAt(1) + "\nThreat Level: B";
        planetThreeText.text = "Planet: " + planetNames.ElementAt(2) + "\nThreat Level: A";
        
        SetPlanetImage(easyPlanetImage, planetTypes.ElementAt(0));
        SetPlanetImage(mediumPlanetImage, planetTypes.ElementAt(1));
        SetPlanetImage(hardPlanetImage, planetTypes.ElementAt(2));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlanetImage(Image image, string planet){
        switch (planet)
        {
            case "Grass":
                image.sprite = grassPlanetImages.ElementAt(Random.Range(0,grassPlanetImages.Count));
                break;
            case "Rock":
                image.sprite = rockPlanetImages.ElementAt(Random.Range(0,rockPlanetImages.Count));
                break;
            case "Desert":
                image.sprite = desertPlanetImages.ElementAt(Random.Range(0, desertPlanetImages.Count));
                break;
            case "Snow":
                image.sprite = snowPlanetImages.ElementAt(Random.Range(0,snowPlanetImages.Count));
                break;
        }
    }

    public void PlanetSelect(){
        encounterText.text = "Progress toward Next System: Liberate " + planetsRequired + " Sectors. " + planetsLiberated + "/" + planetsRequired + " Liberated.";
        zoomTransition.SwapCanvas(mainCanvas, planetSelectCanvas, mainCamera);
    }

    public void ShopSelect(){
        if(Player.currency < 0)
            currencyCount.text = "Total Credits: 0";
        else
            currencyCount.text = "Total Credits: " + Player.currency + "c";
        zoomTransition.SwapCanvas(mainCanvas,shopCanvas, mainCamera);
    }

    public void ArmorySelect(){
        stats_UI.ClearStats();
        inventory_UI.ClearInventory();
        zoomTransition.SwapCanvas(mainCanvas,armoryCanvas, mainCamera);
    }

    public void ArrowClicked(bool initialLoad){
        string buttonName = null;
        
        if(!initialLoad)
            buttonName = EventSystem.current.currentSelectedGameObject.name;
        int leftIdx = (currentGrid - 1 < 0)? 2: currentGrid - 1;
        int rightIdx = (currentGrid + 1 > 2)? 0: currentGrid + 1;
        if(buttonName == "LeftArrow"){
            leftIdx = (leftIdx - 1 < 0)? 2: leftIdx-1;
            rightIdx = (rightIdx - 1 < 0)? 2: rightIdx-1;
            currentGrid = (currentGrid - 1 < 0)? 2: currentGrid-1;
        }
        else if(buttonName == "RightArrow"){
            leftIdx = (leftIdx + 1 > 2)? 0: leftIdx + 1;
            rightIdx = (rightIdx + 1 > 2)? 0: rightIdx + 1;
            currentGrid = (currentGrid + 1 > 2)? 0: currentGrid + 1;
        }

        leftText.text = gridTypes.ElementAt(leftIdx);
        middleText.text = gridTypes.ElementAt(currentGrid);
        rightText.text = gridTypes.ElementAt(rightIdx);
        inventory_UI.ClearInventory();
        inventory_UI.FillInventory(gridTypes.ElementAt(currentGrid));
    }

    public void ReturnToMain(){
        zoomTransition.ZoomIn("MainMenu");
    }

    public void BackSelect(){
        if(shopCanvas.activeSelf){
            zoomTransition.SwapCanvas(shopCanvas, mainCanvas, mainCamera);
        }
        else if(planetSelectCanvas.activeSelf){
            zoomTransition.SwapCanvas(planetSelectCanvas, mainCanvas, mainCamera);
        }
        else{
            zoomTransition.SwapCanvas(armoryCanvas, mainCanvas, mainCamera);
 
        }
        Tooltip.hidden = true;
    }

    public void PlanetLaunch(){
        if(EventSystem.current.currentSelectedGameObject.name == "LaunchEasyButton"){
           EncounterHandler.difficulty = "Easy";
           EncounterHandler.planetType = planetTypes.ElementAt(0);
        }
        else if(EventSystem.current.currentSelectedGameObject.name == "LaunchMedButton"){
           EncounterHandler.difficulty = "Medium";
           EncounterHandler.planetType = planetTypes.ElementAt(1);
        }
        else{
           EncounterHandler.difficulty = "Hard";
           EncounterHandler.planetType = planetTypes.ElementAt(2);
        }

        //If you have completed enough encounters to go to the next system, you are unable to stay in the current system.
        if(planetsLiberated < planetsRequired){
            audioSource.PlayOneShot(planetLaunchSFX);
            zoomTransition.ZoomIn("MainScene");
        }
        else
            Debug.Log("You have liberated enough sectors, launch to the next system!");
    }

    public void SystemLaunch(){
        // If player has completed the required number of encounters, they are able to to travel to the next system.
        // Once a new system is launched, a transition will play and bring the player back to the ship hub.
        // The ship hub will have refreshed shop and planets, counter to travel to next system is also reset to 0. All
        // All upgrades and stats remain the same, but difficulty increases.
        if(planetsLiberated >= planetsRequired){
            audioSource.PlayOneShot(systemLaunchSFX);
            //Show a confirmation message before moving to next system?
            planetsLiberated = 0;
            Player.systemsComplete += 1;
            //Create transition for moving to next system
            //Reload the scene
            // if(planetsRequired <= 10){
            //     planetsRequired++;
            // }
            zoomTransition.ZoomIn("ShipHub");
            //Increase sight distance across the board on system launch, other variables are increased on instance creation
            EnemyAI.addedSight += 2;

        }
        else{
            //Tell player that they have not completed enough encounters.
            Debug.Log("Player needs to complete more encounters");
        }
    }   
}

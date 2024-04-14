using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Linq;
public class ShipHubHandler : MonoBehaviour
{

    [SerializeField] private ZoomTransition zoomTransition;

    [SerializeField] public TextMeshProUGUI currencyCount;
    [SerializeField] public TextMeshProUGUI encounterText;

    [SerializeField] public TextMeshProUGUI planetOneText;
    [SerializeField] public TextMeshProUGUI planetTwoText;
    [SerializeField] public TextMeshProUGUI planetThreeText;
    public Camera mainCamera;

    public GameObject mainCanvas;
    public GameObject shopCanvas;
    public GameObject planetSelectCanvas;

    private GameObject player;
    public static int planetsLiberated = 0;
    public static int planetsRequired = 3;

    private List<string> planetNames = new List<string> {"Hyperion IX", "Titanus", "Orionis", "Elysium", "Lumina", "Pulsaria", "Arcadia", "Astralyn", "Seraphim IV", "Chronosia", "Olympus XI", "Stellara"};

    // Start is called before the first frame update
    void Start()
    {
        UI_Shop.Shuffle(planetNames);
        planetOneText.text = "Planet: " + planetNames.ElementAt(0) + "\nThreat Level: C";
        planetTwoText.text = "Planet: " + planetNames.ElementAt(1) + "\nThreat Level: B";
        planetThreeText.text = "Planet: " + planetNames.ElementAt(2) + "\nThreat Level: A";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void planetSelect(){
        encounterText.text = "Progress toward Next System: Liberate " + planetsRequired + " Sectors. " + planetsLiberated + "/" + planetsRequired + " Liberated.";
        zoomTransition.swapCanvas(mainCanvas, planetSelectCanvas, mainCamera);
    }

    public void shopSelect(){
        if(Player.currency < 0)
             currencyCount.text = "Total Credits: 0";
        else
            currencyCount.text = "Total Credits: " + Player.currency + "c";
        zoomTransition.swapCanvas(mainCanvas,shopCanvas, mainCamera);
    }

    public void returnToMain(){
        zoomTransition.zoomIn("MainMenu");
    }

    public void backSelect(){
        if(shopCanvas.activeSelf){
            zoomTransition.swapCanvas(shopCanvas, mainCanvas, mainCamera);
        }
        else{
            zoomTransition.swapCanvas(planetSelectCanvas, mainCanvas, mainCamera);
        }
        Tooltip.hidden = true;
    }

    public void planetLaunch(){
        if(EventSystem.current.currentSelectedGameObject.name == "LaunchEasyButton"){
           EncounterHandler.difficulty = "Easy";
        }
        else if(EventSystem.current.currentSelectedGameObject.name == "LaunchMedButton"){
           EncounterHandler.difficulty = "Medium";
        }
        else{
           EncounterHandler.difficulty = "Hard";
        }

        //If you have completed enough encounters to go to the next system, you are unable to stay in the current system.
        if(planetsLiberated != planetsRequired)
            zoomTransition.zoomIn("MainScene");
        else
            Debug.Log("You have liberated enough sectors, launch to the next system!");
    }

    public void systemLaunch(){
        // If player has completed the required number of encounters, they are able to to travel to the next system.
        // Once a new system is launched, a transition will play and bring the player back to the ship hub.
        // The ship hub will have refreshed shop and planets, counter to travel to next system is also reset to 0. All
        // All upgrades and stats remain the same, but difficulty increases.
        if(planetsLiberated == planetsRequired){
            //Show a confirmation message before moving to next system?
            planetsLiberated = 0;
            Player.systemsComplete += 1;
            //Create transition for moving to next system
            //Reload the scene
            if(planetsRequired <= 10){
                planetsRequired++;
            }
            SceneManager.LoadScene("ShipHub");

            //Increase sight distance across the board on system launch, other variables are increased on instance creation
            EnemyAI.sightDistance += 2;

        }
        else{
            //Tell player that they have not completed enough encounters.
            Debug.Log("Player needs to complete more encounters");
        }
    }
}

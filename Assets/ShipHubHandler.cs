using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class ShipHubHandler : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI currencyCount;

    public GameObject mainCam;
    public GameObject shopCam;
    public GameObject planetSelectCam;

    public GameObject mainCanvas;
    public GameObject shopCanvas;
    public GameObject planetSelectCanvas;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void planetSelect(){
        mainCanvas.SetActive(false);
        planetSelectCanvas.SetActive(true);
        mainCam.SetActive(false);
        planetSelectCam.SetActive(true);
    }

    public void shopSelect(){
        // Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(Player.currency < 0)
             currencyCount.text = "Current Amount: 0";
        else
            currencyCount.text = "Current Amount: " + Player.currency;
        mainCanvas.SetActive(false);
        shopCanvas.SetActive(true);
        mainCam.SetActive(false);
        shopCam.SetActive(true);
    }

    public void purchaseUpgrade(){
        //Retrieve upgrade name and price

        //Determine if player has enough currency
        //If so, decrement the current balance and grant upgrade

        //Change purchase icon to purchased and update the current balance displayed on screen
    }

    public void returnToMain(){
        SceneManager.LoadScene("MainMenu");
    }

    public void backSelect(){
        shopCanvas.SetActive(false);
        planetSelectCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        shopCam.SetActive(false);
        planetSelectCam.SetActive(false);
        mainCam.SetActive(true);
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
        
        SceneManager.LoadScene("MainScene");
    }
}

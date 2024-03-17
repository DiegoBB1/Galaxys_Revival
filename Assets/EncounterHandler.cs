using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EncounterHandler : MonoBehaviour
{
    public GameObject gameOverMenu;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI gameOverDesc;
    [SerializeField] private TextMeshProUGUI backButtonText;

    [SerializeField] private TextMeshProUGUI objectiveText;

    public static String difficulty;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void encounterFinish(){
        // gameOverMenu.SetActive(true);
        if(GameObject.FindWithTag("Player").GetComponent<Player>().objectiveCompleted == true){
            gameOverText.text = "You Win!";
            gameOverDesc.text = "Testing";
            backButtonText.text = "Return to Ship Hub";
            if(difficulty == "Easy"){
                Player.currency += 50;
            }else if (difficulty == "Medium"){
                Player.currency += 75;
            }else{
                Player.currency += 100;
            }
        }
        else{
            gameOverText.text = "You Lose!";
            gameOverDesc.text = "Testing";
            backButtonText.text = "Start a New Game";

            //Reset currency back to 100
            Player.currency = 100;
        }
        gameOverMenu.SetActive(true);
    }

    public void enemyDefeated(){
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        GameObject.FindWithTag("Player").GetComponent<Player>().enemiesDefeated++;
        // Debug.Log("Enemies Defeated: " + ++GameObject.FindWithTag("Player").GetComponent<Player>().enemiesDefeated);
        objectiveText.text = "Current Objective: Defeat all enemies (" + player.enemiesDefeated + "/" + player.enemiesRequired + " defeated";
        if(player.enemiesDefeated == player.enemiesRequired){
            Time.timeScale = 0;
            GameObject.FindWithTag("Player").GetComponent<Player>().objectiveCompleted = true;
            encounterFinish();
        }
    }

    public void returnToShipHub(){
        //Check if button text is to return to ship hub, if so then move to that scene
        Time.timeScale = 1;
        SceneManager.LoadScene("ShipHub");
        //Check if button text is for start a new game, if so then reset variables based on that
        
    }

    public void returnToMain(){
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");    
        }
}

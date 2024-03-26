using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] public TextMeshProUGUI objectiveText;

    [SerializeField] private Player player;
    public static String difficulty;

    public static int totalCaches;
    public int cachesFound;
    public float ambushTime = 90;
    public static bool timerNotStarted = true;
    public GameObject caches;
    public GameObject signal;
    public GameObject signalLight;
    public float lightFadeDuration;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if(difficulty == "Easy"){
            objectiveText.text = "Current Objective: Defeat Enemies (0/" + player.enemiesRequired + " defeated)";
        }
        else if(difficulty == "Medium"){
            caches.SetActive(true);
            cachesFound = 0;
            totalCaches = 9;
            objectiveText.text = "Find and steal enemy supply caches (0/" + totalCaches + ")";
        }
        else if(difficulty == "Hard"){
            ambushTime += 10 * Player.systemsComplete;
            objectiveText.text = "Locate the source of the distress call";
            signal.SetActive(true);

            StartCoroutine(AlarmRoutine());
            IEnumerator AlarmRoutine(){
                while(true){
                    float timer = 0;
                    //Fades light from white to red
                    while(timer < lightFadeDuration){
                        yield return null;
                        timer+=Time.deltaTime;
                        signalLight.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, timer/lightFadeDuration);
                    }
                    signalLight.GetComponent<SpriteRenderer>().color = Color.red;

                    //Fades light from red to white
                    timer = 0;
                    while(timer < lightFadeDuration){
                        yield return null;
                        timer+=Time.deltaTime;
                        signalLight.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, timer/lightFadeDuration);
                    }
                    signalLight.GetComponent<SpriteRenderer>().color = Color.white;
                    }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void encounterFinish(){
        int creditsEarned;
        Time.timeScale = 0;
        // gameOverMenu.SetActive(true);
        if(GameObject.FindWithTag("Player").GetComponent<Player>().objectiveCompleted == true){
            gameOverText.text = "Mission Complete. Sector Liberated";
            backButtonText.text = "Return to Ship Hub";
            if(difficulty == "Easy"){
                creditsEarned = 50 + (50 * Player.systemsComplete);
            }else if (difficulty == "Medium"){
                creditsEarned = 75 + (75 * Player.systemsComplete);
            }else{
                creditsEarned = 100 + (100 * Player.systemsComplete);
                EnemyAI.sightDistance -= 10;
            }
            Player.currency += creditsEarned;
            gameOverDesc.text = "Your efforts have paid off! You have earned " + creditsEarned + " credits.";
            ShipHubHandler.planetsLiberated++;
            Player.totalPlanets++;
        }
        else{
            gameOverText.text = "Mission Failed. Game Over";
            gameOverDesc.text = "You traveled to " + (Player.systemsComplete + 1) +" system(s) and liberated " + Player.totalPlanets + " planet(s)";
            backButtonText.text = "Return to Main Menu";


            if(difficulty == "Hard")
                EnemyAI.sightDistance -= 10;

        }
        gameOverMenu.SetActive(true);
    }

    public void enemyDefeated(){
        // Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        GameObject.FindWithTag("Player").GetComponent<Player>().enemiesDefeated++;
        Spawner.currentEnemies--;
        // Debug.Log("Enemies Defeated: " + ++GameObject.FindWithTag("Player").GetComponent<Player>().enemiesDefeated);
        if(difficulty == "Easy")
            objectiveText.text = "Current Objective: Defeat Enemies (" + player.enemiesDefeated + "/" + player.enemiesRequired + " defeated)";
        
        if(player.enemiesDefeated == player.enemiesRequired && difficulty == "Easy"){
            GameObject.FindWithTag("Player").GetComponent<Player>().objectiveCompleted = true;
            encounterFinish();
        }
    }

    public void cacheCollected(){
        cachesFound++;
        objectiveText.text = "Find and steal enemy supply caches (" + cachesFound + "/" + totalCaches + ")";
        if(cachesFound == totalCaches){
            player.objectiveCompleted = true;
            encounterFinish();
        }
    }

    public void gameOverButton(){
        if(backButtonText.text == "Return to Ship Hub"){
            Time.timeScale = 1;
            SceneManager.LoadScene("ShipHub");
        }
        else{
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");  
        }        
    }

}

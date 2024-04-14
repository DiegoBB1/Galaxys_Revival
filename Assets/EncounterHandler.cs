using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class EncounterHandler : MonoBehaviour
{
    public GameObject gameOverMenu;
    public GameObject stageOne;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI gameOverDesc;
    [SerializeField] private TextMeshProUGUI backButtonText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] public TextMeshProUGUI objectiveText;

    [SerializeField] private Player player;
    public static string difficulty;

    public static int totalCaches;
    public int cachesFound;
    public float ambushTime = 90;
    public static bool timerNotStarted = true;
    public GameObject caches;
    public GameObject signal;
    public GameObject signalLight;
    public float lightFadeDuration;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {

        // Randomly selects a map for the encounter
        int randNum;
        randNum = Random.Range(0,4);
        switch(randNum){
            case 0:
                stageOne.SetActive(true);
                break;
            case 1:
                stageOne.SetActive(true);
                break;
            case 2:
                stageOne.SetActive(true);
                break;
            case 3:
                stageOne.SetActive(true);
                break;
        }
        // Randomly selects an encounter based on the difficulty selected
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if(difficulty == "Easy"){
            randNum = Random.Range(0,3);
            switch(randNum){
                case 0:
                    objectiveText.text = "Current Objective: Defeat Enemies (0/" + player.enemiesRequired + " defeated)";
                    break;
                case 1:
                    objectiveText.text = "Current Objective: Repair malfunctioning terminals (0/5)";
                    break;
                case 2:
                    objectiveText.text = "Current Objective: Deliver supplies to friendly outposts";
                    break;
            }

        }
        else if(difficulty == "Medium"){
            randNum = Random.Range(0,3);
            switch(randNum){
                case 0:
                    caches.SetActive(true);
                    cachesFound = 0;
                    totalCaches = 9;
                    objectiveText.text = "Current Objective: Find and steal enemy supply caches (0/" + totalCaches + ")";;
                    break;
                case 1:
                    objectiveText.text = "Current Objective: Locate and defeat high-value targets (0/3)";
                    break;
                case 2:
                    objectiveText.text = "Current Objective: Retake enemy controlled zones (0/3)";
                    break;
            }

        }
        else if(difficulty == "Hard"){
            randNum = Random.Range(0,3);
            switch(randNum){
                case 0:
                    EnemyAI.sightDistance += 10;
                    ambushTime += 10 * Player.systemsComplete;
                    objectiveText.text = "Current Objective: Locate the source of the distress call";
                    signal.SetActive(true);

                    StartCoroutine(AlarmRoutine());
                    break;
                case 1:
                    objectiveText.text = "Current Objective: Release prisoners from enemy bases and clear them a path for exfiltration (0/3)";
                    break;
                case 2:
                    objectiveText.text = "Current Objective: Destroy the enemy stronghold";
                    break;
            }

        }


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

    public void encounterFinish(){
        int creditsEarned;
        int bonusCredits = 0;
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

            //Checks luck stat and gives bonus credits based on the probability
            if (Random.value <= Player.luck){
                bonusCredits = 50 * Player.systemsComplete;
            }

            Player.currency += creditsEarned + bonusCredits;
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
        //If the player has the credit generation ability active, they are rewarded 50 credits for defeating an enemy
        if(Player.abilityActive && Player.abilityEquipped == "Credit Gen"){
            Debug.Log("10 credits granted");
            Player.currency += 10;
        }
        
        //If the player has the health generation ability active, they are rewarded 1 HP for every 5 enemies defeated
        if(Player.abilityActive && Player.abilityEquipped == "Health Gen"){
            counter++;
            if(counter >= 5){
                Debug.Log("1 HP granted");
                Player.playerHealth = (Player.playerHealth < Player.maxHealth) ? Player.playerHealth + 1: Player.maxHealth;
                counter = 0;
                healthText.text = "HP: " + Player.playerHealth;
            }
        }

        GameObject.FindWithTag("Player").GetComponent<Player>().enemiesDefeated++;
        Spawner.currentEnemies--;
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

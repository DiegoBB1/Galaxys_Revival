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
    public static string currentEncounter;
    public static int totalCaches;
    public int cachesFound;
    public float ambushTime = 90;
    public static bool timerNotStarted = true;
    public GameObject enemyCaches;
    public GameObject friendlyCaches;
    public GameObject signal;
    public GameObject signalLight;
    public GameObject terminals;
    public GameObject zones;
    public GameObject zone1;
    public GameObject zone2;
    public GameObject zone3;
    public GameObject cages;
    public GameObject cage1;
    public GameObject cage2;
    public GameObject cage3;
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
                    currentEncounter = "Defeat Enemies";
                    objectiveText.text = "Current Objective: Defeat Enemies (0/" + player.enemiesRequired + " defeated)";
                    break;
                case 1:
                    enemyCaches.SetActive(true);
                    cachesFound = 0;
                    totalCaches = 9;
                    currentEncounter = "Steal Caches";
                    objectiveText.text = "Current Objective: Find and steal enemy supply caches (0/" + totalCaches + ")";
                    break;
                case 2:
                    friendlyCaches.SetActive(true);
                    cachesFound = 0;
                    currentEncounter = "Deliver Supplies";
                    objectiveText.text = "Current Objective: Collect the nearby supply caches (0/3)";
                    break;
            }

        }
        else if(difficulty == "Medium"){
            randNum = Random.Range(0,3);
            switch(randNum){
                case 0:
                    terminals.SetActive(true);
                    currentEncounter = "Repair Terminals";
                    objectiveText.text = "Current Objective: Repair malfunctioning terminals (0/3)";
                    break;
                case 1:
                    currentEncounter = "High Value Targets";
                    objectiveText.text = "Current Objective: Locate and defeat high-value targets (0/3)";
                    break;
                case 2:
                    currentEncounter = "Retake Zones";
                    zones.SetActive(true);
                    objectiveText.text = "Current Objective: Retake enemy controlled zones (0/3)";
                    break;
            }

        }
        else if(difficulty == "Hard"){
            randNum = Random.Range(0,3);
            switch(randNum){
                case 0:
                    EnemyAI.addedSight += 10;
                    ambushTime += 10 * Player.systemsComplete;
                    currentEncounter = "Ambush";
                    objectiveText.text = "Current Objective: Locate the source of the distress call";
                    signal.SetActive(true);

                    StartCoroutine(AlarmRoutine());
                    break;
                case 1:
                    cages.SetActive(true);
                    currentEncounter = "Release Prisoners";
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

    public void EncounterFinish(){
        int creditsEarned;
        int bonusCredits = 0;
        Time.timeScale = 0;
        if(GameObject.FindWithTag("Player").GetComponent<Player>().objectiveCompleted == true){
            gameOverText.text = "Mission Complete. Sector Liberated";
            backButtonText.text = "Return to Ship Hub";
            if(difficulty == "Easy"){
                creditsEarned = 50 + (50 * Player.systemsComplete);
            }else if (difficulty == "Medium"){
                creditsEarned = 75 + (75 * Player.systemsComplete);
            }else{
                creditsEarned = 100 + (100 * Player.systemsComplete);
                EnemyAI.addedSight -= 10;
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


            if(currentEncounter == "Ambush")
                EnemyAI.addedSight -= 10;

        }
        gameOverMenu.SetActive(true);
    }

    public void EnemyDefeated(){
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

        player.enemiesDefeated++;
        Spawner.currentEnemies--;
        if(currentEncounter == "Defeat Enemies" && !player.objectiveCompleted)
            objectiveText.text = "Current Objective: Defeat Enemies (" + player.enemiesDefeated + "/" + player.enemiesRequired + " defeated)";
        
        if(player.enemiesDefeated == player.enemiesRequired && currentEncounter == "Defeat Enemies"){
            player.objectiveCompleted = true;
            objectiveText.text = "Current Objective: Return to teleporter for exfiltration";
        }

        if(currentEncounter == "Retake Zones" && Player.inZone){
            //Check each of the zones 
            if(zone1.GetComponent<Zone>().active)
                zone1.GetComponent<Zone>().ZoneDefended();
            if(zone2.GetComponent<Zone>().active)
                zone2.GetComponent<Zone>().ZoneDefended();
            if(zone3.GetComponent<Zone>().active)
                zone3.GetComponent<Zone>().ZoneDefended();
        }
    }

    public void CacheCollected(){
        cachesFound++;
        if(currentEncounter == "Steal Caches"){
            objectiveText.text = "Find and steal enemy supply caches (" + cachesFound + "/" + totalCaches + ")";
            if(cachesFound == totalCaches){
                player.objectiveCompleted = true;
                objectiveText.text = "Current Objective: Return to teleporter for exfiltration";
            }
        }
        else{
            objectiveText.text = "Current Objective: Collect the nearby supply caches (" + cachesFound + "/" + (3 - player.cachesDelivered) +")";
            if(cachesFound + player.cachesDelivered == 3)
                objectiveText.text = "Current Objective: Deliver the collected supplies to friendly outposts (" + (0 + player.cachesDelivered) + "/3)";
        }
    }
    
    public void GameOverButton(){
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

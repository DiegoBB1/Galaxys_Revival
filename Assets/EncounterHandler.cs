using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
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
    public static string difficulty;
    public static string currentEncounter;
    public int totalCaches = 9;
    public int cachesFound = 0;
    public float ambushTime = 45;
    public static bool timerNotStarted;
    public GameObject signalLight;
    public GameObject zone1;
    public GameObject zone2;
    public GameObject zone3;
    public float lightFadeDuration;
    private int counter = 0;
    [SerializeField] public TextMeshProUGUI abilityText;
    [SerializeField] public Image abilityImage;
    [SerializeField] private GameObject abilityBar;
    [SerializeField] List<Sprite> abilityIcons;
    private AudioSource audioSource;
    [SerializeField] List<AudioClip> encounterMusic;
    public static string planetType;
    [SerializeField] public GameObject pauseMenu;
    private bool isPaused = false;
    [SerializeField] List<GameObject> planetTilesets;
    [SerializeField] List<GameObject> teleporters;
    [SerializeField] List<GameObject> supplies;
    [SerializeField] List<GameObject> terminals;
    [SerializeField] GameObject zones;
    [SerializeField] List<GameObject> enemyCaches;
    [SerializeField] GameObject beacon;
    [SerializeField] List<GameObject> cages;
    private int planetNum;
    public void Awake(){
        audioSource = GetComponent<AudioSource>();  
        audioSource.PlayOneShot(encounterMusic.ElementAt(Random.Range(0,encounterMusic.Count)));
        if(planetType == "Grass"){
            planetTilesets.ElementAt(0).SetActive(true);
            planetNum = 0;
            player.transform.position = new Vector3(-12,-40,0);
        }
        else if(planetType == "Rock"){
            planetTilesets.ElementAt(1).SetActive(true);
            planetNum = 1;
            player.transform.position = new Vector3(-96,-13,0);
        }
        else if(planetType == "Desert"){
            planetTilesets.ElementAt(2).SetActive(true);
            planetNum = 2;
            player.transform.position = new Vector3(91,-38,0);
        }
        else if(planetType == "Snow"){
            planetTilesets.ElementAt(3).SetActive(true);
            planetNum = 3;
            player.transform.position = new Vector3(57,45,0);
        }
        
        teleporters.ElementAt(planetNum).SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {   
        if(Player.abilityEquipped != "None"){
            abilityBar.SetActive(true);
            //Add sprite to ability image
            switch(Player.abilityEquipped){
                case "Cloaking Device":
                    abilityImage.sprite = abilityIcons.ElementAt(0);
                    break;
                case "Time Slow":
                    abilityImage.sprite = abilityIcons.ElementAt(1);
                    break;
                case "Credit Gen":
                    abilityImage.sprite = abilityIcons.ElementAt(2);
                    break;
                case "Health Gen":
                    abilityImage.sprite = abilityIcons.ElementAt(3);
                    break;
            }
        }
        // Randomly selects an encounter based on the difficulty selected
        int randNum;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if(difficulty == "Easy"){
            randNum = Random.Range(0,3);
            switch(randNum){
                case 0:
                    currentEncounter = "Defeat Enemies";
                    objectiveText.text = "Current Objective: Defeat Enemies (0/" + Player.enemiesRequired + " defeated)";
                    break;
                case 1:
                    enemyCaches.ElementAt(planetNum).SetActive(true);
                    currentEncounter = "Steal Caches";
                    objectiveText.text = "Current Objective: Find and steal enemy supply caches (0/" + totalCaches + ")";
                    break;
                case 2:
                    supplies.ElementAt(planetNum).SetActive(true);
                    currentEncounter = "Deliver Supplies";
                    objectiveText.text = "Current Objective: Collect the nearby supply caches (0/3)";
                    break;
            }

        }
        else if(difficulty == "Medium"){
            randNum = Random.Range(0,3);
            switch(randNum){
                case 0:
                    terminals.ElementAt(planetNum).SetActive(true);
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

                    switch(planetNum){
                        case 0:
                            zone1.transform.position = new Vector3(-24, 5,0);
                            zone2.transform.position = new Vector3(87, 8,0);
                            zone3.transform.position = new Vector3(3, 48,0);
                            break;
                        case 1:
                            zone1.transform.position = new Vector3(60, 32,0);
                            zone2.transform.position = new Vector3(17.5f, 1.5f, 0);
                            zone3.transform.position = new Vector3(-64, 21, 0);
                            break;
                        case 2:
                            zone1.transform.position = new Vector3(-30, 15, 0);
                            zone2.transform.position = new Vector3(3, 48, 0);
                            zone3.transform.position = new Vector3(73, 8, 0);
                            break;
                        case 3:
                            zone1.transform.position = new Vector3(63, -16, 0);
                            zone2.transform.position = new Vector3(-57, -14, 0);
                            zone3.transform.position = new Vector3(-5.5f, 8.5f, 0);
                            break;
                    }
                    break;
            }

        }
        else if(difficulty == "Hard"){
            randNum = Random.Range(0,2);
            switch(randNum){
                case 0:
                    timerNotStarted = true;
                    EnemyAI.addedSight += 10;
                    ambushTime += 10 * Player.systemsComplete;
                    currentEncounter = "Ambush";
                    objectiveText.text = "Current Objective: Locate the source of the distress call";
                    switch(planetNum){
                        case 0:
                            beacon.transform.position = new Vector3(14, -2, -7);
                            break;
                        case 1:
                            beacon.transform.position = new Vector3(29, 2, -7);
                            break;
                        case 2:
                            beacon.transform.position = new Vector3(-14, -1, -7);
                            break;
                        case 3:
                            beacon.transform.position = new Vector3(-5, 11, -7);
                            break;
                    }                   
                    beacon.SetActive(true);
                    StartCoroutine(AlarmRoutine());
                    break;
                case 1:
                    cages.ElementAt(planetNum).SetActive(true);
                    currentEncounter = "Release Prisoners";
                    objectiveText.text = "Current Objective: Release prisoners from enemy bases and clear them a path for exfiltration (0/3)";
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

    public void EncounterFinish(bool objComplete){
        audioSource.volume = .25f;
        Spawner spawner = GameObject.Find("EnemySpawner").GetComponent<Spawner>();
        spawner.ActiveEnemies.Clear();
        spawner.InactiveEnemies.Clear();    
        int creditsEarned;
        int bonusCredits = 0;
        Time.timeScale = 0;
        if(objComplete == true){
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
                healthText.text = ": " + Player.playerHealth;
            }
        }

        player.enemiesDefeated++;
        if(currentEncounter == "Defeat Enemies" && !player.objectiveCompleted)
            objectiveText.text = "Current Objective: Defeat Enemies (" + player.enemiesDefeated + "/" + Player.enemiesRequired + " defeated)";
        
        if(player.enemiesDefeated == Player.enemiesRequired && currentEncounter == "Defeat Enemies"){
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
            objectiveText.text = "Current Objective: Find and steal enemy supply caches (" + cachesFound + "/" + totalCaches + ")";
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
        Time.timeScale = 1;
        if(backButtonText.text == "Return to Ship Hub")
            SceneManager.LoadScene("ShipHub");
        else
            SceneManager.LoadScene("MainMenu");      
    }
    
    public void Pause(){
        if(!player.exfiltrated){
            //If the game is already paused and the pause button is clicked, the game will be unpaused
            if(isPaused){
                pauseMenu.SetActive(false);
                isPaused = false;
                audioSource.volume = .5f;
                if(Player.abilityEquipped == "Time Slow" && Player.abilityActive)
                    Time.timeScale = .5f;
                else
                    Time.timeScale = 1;
                    
                
            }
            //Otherwise, if the game is unpaused, then it will be paused
            else{
                pauseMenu.SetActive(true);
                isPaused = true;
                Time.timeScale = 0;
                audioSource.volume = .25f;
            }
        }
    }

    public void MainMenu(){
        Player.isImmune = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit(){
        Application.Quit();
    }

}

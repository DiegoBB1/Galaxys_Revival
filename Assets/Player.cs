using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject weaponProjectile;

    [SerializeField] public int enemiesRequired = 20;
    [SerializeField] private TextMeshProUGUI healthText;

    Rigidbody2D rb;

    public static bool isImmune = false;

    public bool objectiveCompleted = false; 
    

    // Player Stats
    public static int currency = 150;
    public static float playerHealth = 3;
    public static int maxHealth = 3;
    public static float playerSpeed = 5f;
    public static int strength = 1;
    public static int defense = 0;
    public static float luck = .1f;
    public static float projectileSpeed = 3f; //Orig 5
    public static float projectileCooldown = 1;

    //Player abilites/weapons
    public static List<string> passivesPurchased = new List<string>{};
    public static List<string> abilitiesPurchased = new List<string>{};
    public static List<string> weaponsPurchased = new List<string>{};


    public static string passiveEquipped = "None";
    public static string abilityEquipped = "None";
    public static string weaponEquipped = "Default";
    public static bool passiveAvailable = true;
    public static bool abilityAvailable = true;
    public int enemiesDefeated = 0;
    public static bool abilityActive = false;
    public static int chargeLevel = 0;

    public static int systemsComplete = 0;
    
    public static int totalPlanets = 0;
    public bool abilityUsed = false;
    EncounterHandler encounterHandler;

    SpriteRenderer spriteRenderer;
    private float fixedDeltaTime;

    // Start is called before the first frame update
    void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        encounterHandler = GameObject.Find("EncounterHandler").GetComponent<EncounterHandler>();
        isImmune = false;
        healthText.text = "HP: " + playerHealth;
        objectiveCompleted = false;
        enemiesRequired = 20 + 10 * systemsComplete;

        if(passiveEquipped == "Shield")
            spriteRenderer.color = Color.cyan;
    }

    public void movePlayer(Vector3 input){
        rb.velocity = input * playerSpeed;
    }

    public void shootProjectile(Vector3 pos, Vector3 direction){
        GameObject newProjectile = Instantiate(weaponProjectile, transform.position, Quaternion.Euler(pos));
        newProjectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);
        Destroy(newProjectile, 3); //5 orig
    }

    public void useAbility(){
        switch(abilityEquipped){
            case "Time Slow":
                if(!abilityActive && abilityAvailable){
                    Debug.Log("Time Slow Activated");
                    abilityActive = true;
                    abilityAvailable = false;
                    Time.timeScale = .5f;
                    Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
                    StartCoroutine(timerRoutine(10,60));
                }
                break;
            case "Cloaking Device":
                if(!abilityActive && abilityAvailable){
                    Debug.Log("Cloaking Device Activated");
                    abilityActive = true;
                    abilityAvailable = false;
                    StartCoroutine(timerRoutine(5,30));
                }
                break;
            //Credit Generator will grant X credits for every enemy defeated, lasts for 60 seconds. Can only be used once per encounter
            case "Credit Gen":
                if(!abilityUsed){
                    Debug.Log("Credit Generator Activated");
                    abilityActive = true;
                    abilityUsed = true;
                    StartCoroutine(timerRoutine(20,0));
                }
                break;

            //Credit Generator will grant 1 HP for every X enemies defeated, lasts for 60 seconds. Can only be used once per encounter
            case "Health Gen":
                if(!abilityUsed){
                    Debug.Log("Health Generator Activated");
                    abilityActive = true;
                    abilityUsed = true;
                    StartCoroutine(timerRoutine(20,0));
                }
                break;
            default:
                break;
        }

        IEnumerator timerRoutine(float duration, float cooldown){
            yield return new WaitForSecondsRealtime(duration);
            Debug.Log("Ability Deactivated");
            abilityActive = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
            StartCoroutine(CooldownRoutine());
            IEnumerator CooldownRoutine(){
                Debug.Log("Ability on cooldown");
                yield return new WaitForSeconds(cooldown);
                Debug.Log("Ability off cooldown");
                abilityAvailable = true;
            }
        }
        //Create coroutine that accepts x number of seconds to wait for
    }

    public void damageTaken(int damage){
        //Checks if the player has the shield passive equipped and it is not on cooldown
        if(passiveEquipped == "Shield" && passiveAvailable){
            immuneCooldown();
            spriteRenderer.color = Color.white;
            passiveAvailable = false;
            Debug.Log("Shield broken. Will be recharged in 60 seconds");
            StartCoroutine(ShieldCooldownRoutine());
            IEnumerator ShieldCooldownRoutine(){
                    yield return new WaitForSeconds(60);
                    passiveAvailable = true;
                    Debug.Log("Shield is recharged");
                    spriteRenderer.color = Color.cyan;
            }
            return;
        }

        immuneCooldown();
        spriteRenderer.color = Color.red;
        StartCoroutine(damageEffect());
        //Regardless of strength, you cannot be completely immune to an attack unless you have iframes. Defaults to 1 damage if you're defense is high enough to technically make you immune.
        if(damage-defense <= 0){
            damage = 1;
        }
        // Otherwise damage is mitigated using your defense stat.
        else{
            damage = damage - defense;
        }
        playerHealth = playerHealth - damage;
        
        if(playerHealth <= 0){

            if(passiveEquipped == "Self Res" && passiveAvailable){
                playerHealth = maxHealth > 5 ? 5: 3;
                healthText.text = "HP: " + playerHealth;
                Debug.Log("Self Res Used");
                passiveAvailable = false;
            }
            else{
                healthText.text = "HP: 0";
                Time.timeScale = 0;
                encounterHandler.encounterFinish();
            }
        }
        else
            healthText.text = "HP: " + playerHealth;
    }

    public void immuneCooldown(){
        if(isImmune){
            return;
        }

        isImmune = true;

        //wait for cooldown seconds until we can shoot again
        StartCoroutine(immuneCooldownRoutine());
        IEnumerator immuneCooldownRoutine(){
                yield return new WaitForSeconds(2);
                isImmune = false;
                // Debug.Log("Player is no longer immune.");
            }
    }

    public IEnumerator damageEffect(){
        yield return new WaitForSeconds(2);
        spriteRenderer.color = Color.white;
    }

    public void OnTriggerEnter2D(Collider2D other){
        // Branch taken if player walks into a supply cache
        if(other.gameObject.tag == "Cache"){
            Destroy(other.gameObject);
            encounterHandler.cacheCollected();
        }

        // Branch taken if player finds the distress beacon for the ambush encounter
        if(other.gameObject.tag == "Signal" && EncounterHandler.timerNotStarted){
            // Logic for ambush encounter
            // Once ambush starts, change objective text, start timer, and start spawning enemies (which are more aggro)
            Spawner spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
            spawner.spawnEnemies();
            EncounterHandler.timerNotStarted = false;

            StartCoroutine(TimerRoutine());

            IEnumerator TimerRoutine(){
                while (encounterHandler.ambushTime > 0)
                {
                    float minutes = Mathf.FloorToInt(encounterHandler.ambushTime / 60); //divide the time by 60
                    float seconds = Mathf.FloorToInt(encounterHandler.ambushTime % 60); // returns the remainder
                    encounterHandler.objectiveText.text = "Survive the ambush (" + string.Format("{0:00}:{1:00}", minutes, seconds) + " remaining until exfiltration)";
                    yield return new WaitForSeconds(1.0f);
                    encounterHandler.ambushTime--;
                }

                encounterHandler.objectiveText.text = "Survive the ambush (" + string.Format("{0:00}:{1:00}", 0, 0) + " remaining until exfiltration)";
                objectiveCompleted = true;
                encounterHandler.encounterFinish();
            }

        }
    }
}

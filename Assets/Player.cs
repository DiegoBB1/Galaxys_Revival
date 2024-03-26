using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float playerSpeed = 5f;
    [SerializeField] GameObject weaponProjectile;
    [SerializeField] float projectileSpeed = 5f;

    [SerializeField] public int enemiesRequired = 20;
    [SerializeField] private TextMeshProUGUI healthText;

    Rigidbody2D rb;

    public static int currency = 150;

    public static bool isImmune = false;
    public static float projectileCooldown = 1;

    public bool objectiveCompleted = false; 
    public static float playerHealth = 3;
    public static int strength = 1;
    public static int defense = 0;

    public int enemiesDefeated = 0;
    public static bool pierceProjectiles = false;
    public static int systemsComplete = 0;
    
    public static int totalPlanets = 0;
    EncounterHandler encounterHandler;

    SpriteRenderer spriteRenderer;

    // void Awake(){
    //     this.playerHealth = 3;
    // }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        encounterHandler = GameObject.Find("EncounterHandler").GetComponent<EncounterHandler>();
        isImmune = false;
        healthText.text = "HP: " + playerHealth;
        objectiveCompleted = false;
        enemiesRequired = 20 + 10 * systemsComplete;

    }

    public void movePlayer(Vector3 input){
        rb.velocity = input * playerSpeed;
    }

    public void shootProjectile(Vector3 pos, Vector3 direction){
        GameObject newProjectile = Instantiate(weaponProjectile, transform.position, Quaternion.Euler(pos));
        newProjectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);
        Destroy(newProjectile, 5);
    }

    public void damageTaken(int damage){
        // Debug.Log("Damage Taken. Player is temporarily Immune.");
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
            healthText.text = "HP: 0";
            Time.timeScale = 0;
            encounterHandler.encounterFinish();
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
        StartCoroutine(ShootCooldownRoutine());
        IEnumerator ShootCooldownRoutine(){
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

        //}
        }
    }
}

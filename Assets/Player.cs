using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player Stats
    public static int currency = 150;
    public static int playerHealth = 3;
    public static int maxHealth = 3;
    public static int playerSpeed = 5;
    public static int strength = 1;
    public static int defense = 0;
    public static float luck = .1f;
    public static float projectileSpeed = 3f; //Orig 5 CHANGE TO VELOCITY
    public static float projectileCooldown = 1;
    public static bool isImmune = false;   

    //Player passives
    public static List<string> passivesPurchased = new List<string>();
    public static string passiveEquipped = "None";
    public static bool passiveAvailable = true;

    //Player abilities
    public static List<string> abilitiesPurchased = new List<string>();
    public static string abilityEquipped = "None";
    public static bool abilityAvailable = true;
    public static bool abilityActive = false;
    public bool abilityUsed = false;

    //Player weapons
    public static List<string> weaponsPurchased = new List<string>();
    public static string weaponEquipped = "Default";
    public static int chargeLevel = 0;

    [SerializeField] List<Sprite> weaponSprites;
 
    //Encounter/Game Information
    public bool objectiveCompleted = false; 
    public int enemiesDefeated = 0;
    public static int enemiesRequired; //20 to start
    public static int systemsComplete = 0;
    public static int totalPlanets = 0;
    public int cachesDelivered = 0;
    public static int numHvts = 0;
    public int terminalsActivated = 0;
    public bool repairingTerminal = false;
    public static bool inZone = false;
    public int zonesActivated = 0;
    public int prisonersRescued = 0;

    //Other necessary game references
    [SerializeField] GameObject defaultProjectile;
    [SerializeField] GameObject altProjectile;

    [SerializeField] private TextMeshProUGUI healthText;
    Rigidbody2D rb;    
    public EncounterHandler encounterHandler;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite activeTerminalSprite;

    private float fixedDeltaTime;
    public bool exfiltrated;
    [SerializeField] public List<AudioClip> explosiveSFX;
    [SerializeField] public List<AudioClip> chargeSFX;
    [SerializeField] AudioClip defaultWeaponSFX;
    [SerializeField] AudioClip altWeaponSFX;
    [SerializeField] AudioClip damageSFX;
    public AudioSource audioSource;

    public void Awake(){
        audioSource = GetComponent<AudioSource>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        //Reset Player variables on encounter start
        isImmune = false;
        passiveAvailable = true;
        abilityAvailable = true;
        abilityActive = false;
        numHvts = 0;
        inZone = false;
        fixedDeltaTime = Time.fixedDeltaTime;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        encounterHandler = GameObject.Find("EncounterHandler").GetComponent<EncounterHandler>();
        healthText.text = ": " + playerHealth;
        
        enemiesRequired = 20 + 10 * systemsComplete;
        
        if(passiveEquipped == "Shield")
            spriteRenderer.color = Color.cyan;
    }

    public void MovePlayer(Vector3 input){
        rb.velocity = input * playerSpeed;
    }

    public void ShootProjectile(Vector3 pos, Vector3 direction){
        GameObject newProjectile;

        //instantiates the projectile game object based on the weapon that is currently equipped
        if(weaponEquipped == "Pierce Shot"){
            newProjectile = Instantiate(altProjectile, transform.position, Quaternion.Euler(pos));
            audioSource.PlayOneShot(altWeaponSFX);
        }
        else if(weaponEquipped == "Poison Shot"){
            newProjectile = Instantiate(altProjectile, transform.position, Quaternion.Euler(pos));
            newProjectile.GetComponent<SpriteRenderer>().sprite = weaponSprites.ElementAt(0);
            audioSource.PlayOneShot(altWeaponSFX);

        }
        else if(weaponEquipped == "Multi Shot"){
            newProjectile = Instantiate(defaultProjectile, transform.position, Quaternion.Euler(pos));
            newProjectile.GetComponent<SpriteRenderer>().sprite = weaponSprites.ElementAt(1);
            audioSource.PlayOneShot(defaultWeaponSFX);
        }
        else if(weaponEquipped == "Charge Shot"){
            newProjectile = Instantiate(defaultProjectile, transform.position, Quaternion.Euler(pos));
            audioSource.PlayOneShot(chargeSFX.ElementAt(chargeLevel-1));
            if(chargeLevel == 1)
                newProjectile.GetComponent<SpriteRenderer>().sprite = weaponSprites.ElementAt(2);
            else if(chargeLevel == 2)
                newProjectile.GetComponent<SpriteRenderer>().sprite = weaponSprites.ElementAt(3);
            else
                newProjectile.GetComponent<SpriteRenderer>().sprite = weaponSprites.ElementAt(4);
            
        }
        else if(weaponEquipped == "Explosive Shot"){
            newProjectile = Instantiate(defaultProjectile, transform.position, Quaternion.Euler(pos));
            newProjectile.GetComponent<SpriteRenderer>().sprite = weaponSprites.ElementAt(5);
            audioSource.PlayOneShot(defaultWeaponSFX);
        }
        else{
            newProjectile = Instantiate(defaultProjectile, transform.position, Quaternion.Euler(pos));
            audioSource.PlayOneShot(defaultWeaponSFX);
        }
        

        newProjectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);
        Destroy(newProjectile, 3); //5 orig
    }

    public void UseAbility(){
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

        // IEnumerator timerRoutine(float duration, float cooldown){
        //     yield return new WaitForSecondsRealtime(duration);
        //     Debug.Log("Ability Deactivated");
        //     abilityActive = false;
        //     Time.timeScale = 1f;
        //     Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
        //     StartCoroutine(CooldownRoutine());
        //     IEnumerator CooldownRoutine(){
        //         Debug.Log("Ability on cooldown");
        //         yield return new WaitForSeconds(cooldown);
        //         Debug.Log("Ability off cooldown");
        //         abilityAvailable = true;
        //     }
        // }

        IEnumerator timerRoutine(float duration, float cooldown){
            StartCoroutine(FadeRoutine());
            while(duration > 0){
                encounterHandler.abilityText.text = duration + "s";
                if(abilityEquipped == "Time Slow")
                    yield return new WaitForSeconds(.5f);
                else
                    yield return new WaitForSeconds(1);
                duration--;
            }
            encounterHandler.abilityText.text = "0s";
            abilityActive = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale;
            Debug.Log("Ability Deactivated");
            StartCoroutine(CooldownRoutine());
            IEnumerator CooldownRoutine(){
                Debug.Log("Ability on cooldown");
                //Coroutine is used to make the ability icon fade in and out when active
                while(cooldown > 0){
                    yield return new WaitForSeconds(1);
                    encounterHandler.abilityText.text = cooldown + "s";
                    cooldown--;
                }
                Debug.Log("Ability off cooldown");
                abilityAvailable = true;

                yield return new WaitForSeconds(1);
                if(abilityUsed){
                    encounterHandler.abilityText.text = "";
                    encounterHandler.abilityImage.color = new Color(1f,1f,1f,.25f);
                }
                else{
                    encounterHandler.abilityText.text = "Use: Space";
                    encounterHandler.abilityImage.color = new Color(1f,1f,1f,1f);
                }
            }

            IEnumerator FadeRoutine(){
                float timer;
                Color origColor;
                while(duration > 0){
                    timer = 0;
                    origColor = encounterHandler.abilityImage.color;
                    //Fades from light to dark
                    while(timer < 1){
                        yield return null;
                        timer+=Time.deltaTime;
                        encounterHandler.abilityImage.color = Color.Lerp(origColor, new Color(1f,1f,1f,.25f), timer/1);
                    }
                    encounterHandler.abilityImage.color = new Color(1f,1f,1f,.25f);

                    //Fades dark to light
                    timer = 0;
                    while(timer < 1){
                        yield return null;
                        timer+=Time.deltaTime;
                        encounterHandler.abilityImage.color = Color.Lerp(new Color(1f,1f,1f,.25f), origColor, timer/1);
                    }
                }
                encounterHandler.abilityImage.color = new Color(1f,1f,1f,.25f);
            }
        }
        //Create coroutine that accepts x number of seconds to wait for
    }

    public void DamageTaken(int damage){
        //Checks if the player has the shield passive equipped and it is not on cooldown
        if(passiveEquipped == "Shield" && passiveAvailable){
            ImmuneCooldown();
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

        audioSource.PlayOneShot(damageSFX);
        ImmuneCooldown();
        spriteRenderer.color = Color.red;
        StartCoroutine(DamageEffect());
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
                healthText.text = ": " + playerHealth;
                Debug.Log("Self Res Used");
                passiveAvailable = false;
            }
            else{
                healthText.text = ": 0";
                Time.timeScale = 0;
                encounterHandler.EncounterFinish(false);
            }
        }
        else
            healthText.text = ": " + playerHealth;
    }

    public void ImmuneCooldown(){
        if(isImmune){
            return;
        }

        isImmune = true;

        //wait for cooldown seconds until we can shoot again
        StartCoroutine(immuneCooldownRoutine());
        IEnumerator immuneCooldownRoutine(){
                yield return new WaitForSeconds(2);
                isImmune = false;
            }
    }

    public IEnumerator DamageEffect(){
        yield return new WaitForSeconds(2);
        spriteRenderer.color = Color.white;
    }

    public void OnTriggerEnter2D(Collider2D other){
        //EASY DIFFICULTY ENCOUNTERS

        // Branch taken if player walks into a supply cache, can either be the steal cache or deliver supplies encounter
        if(other.gameObject.tag == "Cache"){
            Destroy(other.gameObject);
            encounterHandler.CacheCollected();
        }

        //Branch taken if player walks into a location where a supply cache can be placed in the deliver supplies encounter
        if(other.gameObject.tag == "EmptyCache"){
            if(encounterHandler.cachesFound > 0){
                other.isTrigger = false;
                SpriteRenderer tempSR = other.GetComponent<SpriteRenderer>();
                
                Color color = new Color(tempSR.color.r, tempSR.color.g, tempSR.color.b, 1);
                other.GetComponent<SpriteRenderer>().color = color;
                cachesDelivered++;

                encounterHandler.objectiveText.text = "Current Objective: Deliver the collected supplies to friendly outposts (" + cachesDelivered + "/3)";

                if(cachesDelivered == 3){
                    // encounterHandler.objectiveText.text = "Current Objective: Exfiltrate";
                    objectiveCompleted = true;
                    encounterHandler.objectiveText.text = "Current Objective: Return to teleporter for exfiltration";
                }
                else if(--encounterHandler.cachesFound == 0)
                    encounterHandler.objectiveText.text = "Current Objective: Collect and deliver the additional caches (" + encounterHandler.cachesFound + "/" + (3 - cachesDelivered) +")";

            }
            else
                Debug.Log("Player does have any remaining supplies");
        }

        //MEDIUM DIFFICULTY ENCOUNTERS

        //Branch taken if player walks to the location of a terminal in the repair terminal encounter
        if(other.gameObject.tag == "Terminal" && !repairingTerminal){
            repairingTerminal = true;
            Debug.Log("Activating Terminal");
            other.isTrigger = false;
            StartCoroutine(RepairRoutine());

            IEnumerator RepairRoutine(){
                float timer = 0;
                while(Vector3.Distance(transform.position, other.gameObject.transform.position) < 10){
                    yield return null;
                    timer+=Time.deltaTime;
                    encounterHandler.objectiveText.text = "Current Objective: Stay in range of terminal while it activates";
                    if(timer >= 10f){
                        Debug.Log("Terminal Activated");
                        repairingTerminal = false;
                        other.GetComponent<SpriteRenderer>().sprite = activeTerminalSprite;
                        encounterHandler.objectiveText.text = "Current Objective: Repair malfunctioning terminals (" + ++terminalsActivated + "/3)";
                        if(terminalsActivated == 3){
                            objectiveCompleted = true;
                            encounterHandler.objectiveText.text = "Current Objective: Return to teleporter for exfiltration";
                        }
                        yield break;
                    }
                }
                other.isTrigger = true;
                encounterHandler.objectiveText.text = "Current Objective: Activate terminal to reestablish connection";
                Debug.Log("Terminal Activation Disbanded, player exited range");
                repairingTerminal = false;

            }
        }

        //Branch taken if player walks into one of the the zones for the retake zones encounter
        if(other.gameObject.tag == "Zone" && !other.gameObject.GetComponent<Zone>().captured){
            //If a zone is entered, it is activated, so that any kills while inside of it, will grant progress towards that specific zone
            EnemyAI.addedSight += 10;
            other.gameObject.GetComponent<Zone>().active = true;
            other.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            inZone = true;
            Debug.Log("Zone Entered");
        }

        //HARD DIFFICULTY ENCOUNTERS

        // Branch taken if player finds the distress beacon for the ambush encounter
        if(other.gameObject.tag == "Signal" && EncounterHandler.timerNotStarted){
            // Logic for ambush encounter
            // Once ambush starts, change objective text, start timer, and start spawning enemies (which are more aggro)
            Spawner spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
            spawner.SpawnEnemies();
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
                objectiveCompleted = true;
                encounterHandler.objectiveText.text = "Current Objective: Return to teleporter for exfiltration";
            }
        }

        //Branch taken when player reaches the teleporter at the end of the encounter
        // if(other.gameObject.tag == "Teleporter" && objectiveCompleted)
        //     Debug.Log("Triggered");
        //     // encounterHandler.encounterFinish();
    }

    public void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag == "Teleporter" && objectiveCompleted && Vector3.Distance(other.transform.position,transform.position) < .8f && !exfiltrated){
            exfiltrated = true;
            encounterHandler.EncounterFinish(true);
        }
    }
    
    public void OnCollisionStay2D(Collision2D collision){
        //Branch taken if player walks up to one of the prisoner cages
        if(collision.gameObject.tag == "Cage" && !collision.gameObject.GetComponent<Cage>().cageOpen)
            collision.gameObject.GetComponent<Cage>().CageOpened();
    }

    public void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Zone"){
            EnemyAI.addedSight -= 10;
            other.gameObject.GetComponent<Zone>().active = false;
            inZone = false;
            Debug.Log("Zone exited");
            if(!other.gameObject.GetComponent<Zone>().captured)
                other.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
    }

    public void HvtDefeated(){
        encounterHandler.objectiveText.text = "Current Objective: Locate and defeat high-value targets (" + ++numHvts + "/3)";
        if(numHvts == 3){
            objectiveCompleted = true;
            encounterHandler.objectiveText.text = "Current Objective: Return to teleporter for exfiltration";
        }
    }
}

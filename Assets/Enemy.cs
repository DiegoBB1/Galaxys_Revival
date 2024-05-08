using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int sightDistance;
    [SerializeField] public int baseHealth;
    [SerializeField] public int baseDamage;
    [SerializeField] public int baseSpeed;
    [SerializeField] public int enemyHealth;
    [SerializeField] public int damageDealt;
    [SerializeField] public float enemySpeed;
    public SpriteRenderer enemySR;
    Rigidbody2D rb;
    public bool speedReduced = false;
    public bool damageReduced = false;
    public bool poisoned = false;
    EncounterHandler encounterHandler;
    Spawner spawner;

    // Start is called before the first frame update
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
    }
    void Start()
    {

        encounterHandler = GameObject.Find("EncounterHandler").GetComponent<EncounterHandler>();
        spawner = GameObject.Find("EnemySpawner").GetComponent<Spawner>();
    }

    public void OnCollisionStay2D(Collision2D collision){
        if(collision.gameObject.tag == "Player" && !Player.isImmune){
            collision.gameObject.GetComponent<Player>().DamageTaken(damageDealt);
        }
    }

    //Method handles debuffs being applied to the enemy when they take damage
    public void DamageTaken(){
        if(enemyHealth <= 0){
                if(gameObject.tag == "HVT"){
                    Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
                    player.HvtDefeated();
                    Destroy(gameObject);
                }
                else
                    IsDefeated();
                
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        //Checks if player has slowing projectiles, lowers speed if so
        if(Player.passiveEquipped == "Slow Shot"){
            if(!speedReduced){
                sr.color = Color.cyan;
                enemySpeed -= 1.5f;
                speedReduced = true;

                if(gameObject.activeSelf)
                    StartCoroutine(slowRoutine());

                IEnumerator slowRoutine(){
                    yield return new WaitForSeconds(5);
                    sr.color = Color.white;
                    speedReduced = false;
                    enemySpeed += 1.5f;
                }
            }
        }
        //Checks if player has weakening projectiles, lowers damage if so
        else if(Player.passiveEquipped == "Weakening Shot"){
            if(!damageReduced){
                //Handles case so that an enemy cannot deal 0 damage
                bool tooWeak = false;
                if(damageDealt-1 == 0)
                    tooWeak = true;
                else
                    damageDealt -= 1;

                sr.color = Color.yellow;
                damageReduced = true;

                if(gameObject.activeSelf)
                    StartCoroutine(weakenRoutine());

                IEnumerator weakenRoutine(){
                    yield return new WaitForSeconds(5);
                    sr.color = Color.white;
                    damageReduced = false;

                    if(!tooWeak)
                        damageDealt += 1;
                }
            }
        }
        else if(Player.weaponEquipped != "Poison Shot"){
            if(Player.weaponEquipped == "Explosive Shot")
                sr.color = Color.black;
            else
                sr.color = Color.red;

            if(gameObject.activeSelf)
                StartCoroutine(damageEffect());

            IEnumerator damageEffect(){
                yield return new WaitForSeconds(1);
                sr.color = Color.white;
            
            }
        }

        //Checks if player has the poison shot upgrade, applies poison debuff if so
        if(Player.weaponEquipped == "Poison Shot" && !poisoned){
            poisoned = true;
            sr.color = Color.green;

            if(gameObject.activeSelf)
                StartCoroutine(SpawnRoutine());

            IEnumerator SpawnRoutine(){
                int i;
                for(i = 0; i < 3; i++){
                    yield return new WaitForSeconds(2.0f);
                    enemyHealth--;
                    if(enemyHealth <= 0)
                        IsDefeated();
                              
                }
                poisoned = false;
                sr.color = Color.white;
            }

        }

    }

    public void IsDefeated(){
        spawner.currentEnemies--;
        spawner.ActiveEnemies.Remove(gameObject);
        spawner.InactiveEnemies.Add(gameObject);
        gameObject.SetActive(false);

        //Handles case where enemy is despawned for dying vs despawned for being too far from player.
        if(enemyHealth <= 0)
            encounterHandler.EnemyDefeated();
    }

    public void MoveToward(Vector3 target){
        Vector3 direction = target - transform.position;
        MoveCreatureTransform(direction.normalized);
    }

    public void Stop(){
        MoveCreatureTransform(Vector3.zero);
    }

    public void MoveCreatureTransform(Vector3 direction)
    {
        if(direction != null){
            if(direction.x > 0){
                enemySR.flipX = true;
            }
            else if(direction.x < 0){
                enemySR.flipX = false;
            }

            rb.velocity = direction * enemySpeed;
        }
    }
}

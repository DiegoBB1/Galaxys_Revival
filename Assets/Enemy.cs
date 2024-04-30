using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public int sightDistance;
    public int enemyHealth;
    public int damageDealt;
    public float enemySpeed;
    SpriteRenderer enemySR;

    Rigidbody2D rb;

    private bool speedReduced = false;
    private bool damageReduced = false;
    private bool poisoned = false;
    GameObject encounterHandler;

    // Start is called before the first frame update
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        encounterHandler = GameObject.Find("EncounterHandler");
        enemySR = GetComponent<SpriteRenderer>();
    }

    public void OnCollisionStay2D(Collision2D collision){
        if(collision.gameObject.tag == "Player" && !Player.isImmune){
            collision.gameObject.GetComponent<Player>().DamageTaken(damageDealt);
        }
    }

    //Method handles debuffs being applied to the enemy when they take damage
    public void DamageTaken(){
        if(enemyHealth <= 0){
                Destroy(gameObject);

                if(gameObject.tag == "HVT"){
                    Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
                    player.HvtDefeated();
                }
                else
                    encounterHandler.GetComponent<EncounterHandler>().EnemyDefeated();
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        //Checks if player has slowing projectiles, lowers speed if so
        if(Player.passiveEquipped == "Slow Shot"){
            if(!speedReduced){
                sr.color = Color.cyan;
                enemySpeed -= 1.5f;
                speedReduced = true;
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

            StartCoroutine(SpawnRoutine());

            IEnumerator SpawnRoutine(){
                int i;
                for(i = 0; i < 3; i++){
                    yield return new WaitForSeconds(2.0f);
                    enemyHealth--;
                    if(enemyHealth <= 0){
                        Destroy(gameObject);
                        EncounterHandler encounterHandler = GameObject.Find("EncounterHandler").GetComponent<EncounterHandler>();
                        encounterHandler.EnemyDefeated();
                    }            
                }
                poisoned = false;
                sr.color = Color.white;
            }

        }

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

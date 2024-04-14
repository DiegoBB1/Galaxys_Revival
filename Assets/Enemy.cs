using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int enemyHealth;
    public int damageDealt;
    public float enemySpeed;
    SpriteRenderer enemySR;

    Rigidbody2D rb;

    private bool speedReduced = false;
    private bool damageReduced = false;
    private bool poisoned = false;


    // Start is called before the first frame update
    void Start()
    {
        enemySR = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnCollisionStay2D(Collision2D collision){
        if(collision.gameObject.tag == "Player" && !Player.isImmune){
            collision.gameObject.GetComponent<Player>().damageTaken(damageDealt);
        }
    }

    //Method handles debuffs being applied to the enemy when they take damage
    public void damageTaken(){
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
                        encounterHandler.enemyDefeated();
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
        if(direction.x > 0){
            enemySR.flipX = true;
        }
        else if(direction.x < 0){
            enemySR.flipX = false;
        }

        rb.velocity = direction * enemySpeed;
    }
}

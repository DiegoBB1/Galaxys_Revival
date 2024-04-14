using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    GameObject encounterHandler;
    SpriteRenderer spriteRenderer; 

    // Start is called before the first frame update
    void Start()
    {
        encounterHandler = GameObject.Find("EncounterHandler");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            if(Player.weaponEquipped != "Pierce Shot"){
                Destroy(gameObject);
            }
            other.GetComponent<Enemy>().enemyHealth -= Player.strength + Player.chargeLevel;
            other.GetComponent<Enemy>().damageTaken();
            if(other.GetComponent<Enemy>().enemyHealth <= 0){
                Destroy(other.GetComponent<Enemy>().gameObject);
                encounterHandler.GetComponent<EncounterHandler>().enemyDefeated();
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    GameObject encounterHandler;
    SpriteRenderer spriteRenderer; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "HVT"){
            if(Player.weaponEquipped != "Pierce Shot"){
                Destroy(gameObject);
            }

            if(Player.weaponEquipped == "Explosive Shot")
                ExplosionDamage(transform.position, 2.5f);
            else{
                other.GetComponent<Enemy>().enemyHealth -= Player.strength + Player.chargeLevel;
                other.GetComponent<Enemy>().DamageTaken();
            }
        }
    }

    void ExplosionDamage(Vector3 center, float radius)
    {
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.NoFilter();
        Physics2D.OverlapCircle(center, radius, contactFilter, results);
        foreach (var hitCollider in results)
        {
            if(hitCollider.gameObject.tag == "Enemy" || hitCollider.gameObject.tag == "HVT"){
                hitCollider.gameObject.GetComponent<Enemy>().enemyHealth -= Player.strength;
                hitCollider.gameObject.GetComponent<Enemy>().DamageTaken();
            }
        }

    }
}

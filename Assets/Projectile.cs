using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "HVT"){
            if(Player.weaponEquipped != "Pierce Shot"){
                Destroy(gameObject);
            }

            if(Player.weaponEquipped == "Explosive Shot"){
                Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
                player.audioSource.PlayOneShot(player.explosiveSFX.ElementAt(Random.Range(0,player.explosiveSFX.Count)));
                ExplosionDamage(transform.position, 2.5f);
            }
            else{
                other.gameObject.GetComponent<Enemy>().enemyHealth -= Player.strength;
                if(Player.weaponEquipped == "Charge Shot")
                    other.gameObject.GetComponent<Enemy>().enemyHealth -= Player.chargeLevel;

                other.gameObject.GetComponent<Enemy>().DamageTaken();
            }
        }
        else
            Destroy(gameObject);
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

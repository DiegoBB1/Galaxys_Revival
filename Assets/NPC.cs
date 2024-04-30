using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool freed = false;
    public bool enemyClose = false;
    public Player player;
    public GameObject teleporter;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public int speed = 4;
    ContactFilter2D contactFilter = new ContactFilter2D();
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        contactFilter.NoFilter();
    }

    // Update is called once per frame
    void Update()
    {
        if(freed){
            
            if(Vector3.Distance(teleporter.transform.position,transform.position) < .6f){
                Debug.Log("Successfully Exfiltrated");
                Destroy(gameObject);
                EncounterHandler encounterHandler = GameObject.Find("EncounterHandler").GetComponent<EncounterHandler>();
                encounterHandler.objectiveText.text = "Current Objective: Release prisoners from enemy bases and clear them a path for exfiltration (" + ++player.prisonersRescued + "/3)";

                if(player.prisonersRescued == 3){
                    player.objectiveCompleted = true;
                    encounterHandler.objectiveText.text = "Current Objective: Return to teleporter for exfiltration";
                }
            }
            else if(Vector3.Distance(teleporter.transform.position,transform.position) < 5f){
                Vector3 direction = teleporter.transform.position - transform.position;
                MoveNPC(direction.normalized);
            }
            else if(EnemyIsNear()){
                rb.velocity = Vector3.zero * speed;
            }
            else{
                Vector3 direction = player.transform.position - transform.position;
                MoveNPC(direction.normalized);
            }
        }
    }

    public bool EnemyIsNear(){
        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCircle(transform.position, 2.5f, contactFilter, results);
        foreach (var hitCollider in results)
        {
            if(hitCollider.gameObject.tag == "Enemy")
                return true;
            
        }
        return false;
    }

    public void MoveNPC(Vector3 direction)
    {
        if(direction.x > 0){
            sr.flipX = false;
        }
        else if(direction.x < 0){
            sr.flipX = true;
        }

        rb.velocity = direction * speed;
    }
}

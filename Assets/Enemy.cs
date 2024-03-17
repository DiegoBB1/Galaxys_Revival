using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int enemyHealth = 2;
    GameObject encounterHandler; 

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
        if(other.gameObject.tag == "Player"){
            other.GetComponent<Player>().playerHealth--;
            if(other.GetComponent<Player>().playerHealth == 0){
                Time.timeScale = 0;
                encounterHandler.GetComponent<EncounterHandler>().encounterFinish();
            }
        }
    }
}

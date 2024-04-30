using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public bool active = false;
    public bool captured = false;
    public int enemiesRemaining = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //If enemy defeated in zone, the zone counter is decremented. Once counter reaches 0, the zone is captured.
    public void ZoneDefended(){
        Debug.Log("Enemy defeated in Zone");
        if(--enemiesRemaining == 0){
            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
            EncounterHandler encounterHandler = GameObject.Find("EncounterHandler").GetComponent<EncounterHandler>();
            encounterHandler.objectiveText.text = "Current Objective: Retake enemy controlled zones (" + ++player.zonesActivated + "/3)";
            Debug.Log("Zone Captured");
            GetComponent<SpriteRenderer>().color = Color.green;
            captured = true;
            if(player.zonesActivated == 3){
                player.objectiveCompleted = true;
                encounterHandler.objectiveText.text = "Current Objective: Return to teleporter for exfiltration";
            }

        }
        Debug.Log(enemiesRemaining + " enemies remaining");
    }
}

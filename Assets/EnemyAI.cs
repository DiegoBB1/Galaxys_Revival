using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aoiti.Pathfinding; //import the pathfinding library

public class EnemyAI : MonoBehaviour
{

    //blackboard=======================================================
    public Enemy myEnemy; //the creature we are piloting
    public Player player;

    [Header("Config")]
    public LayerMask obstacles;
    public static float addedSight = 0;

    //State machine====================================================
    EnemyAIState currentState;
    public EnemyAIIdleState idleState{get; private set;}
    public EnemyAIAggroState aggroState{get; private set;}
    public EnemyAIPatrolState patrolState{get; private set;}


    public void ChangeState(EnemyAIState newState){

        currentState = newState;

        currentState.BeginStateBase();
    }


    // Start is called before the first frame update
    void Start()
    {
        idleState = new EnemyAIIdleState(this);
        aggroState = new EnemyAIAggroState(this);
        patrolState = new EnemyAIPatrolState(this);
        currentState = idleState;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }


    void FixedUpdate()
    {
        currentState.UpdateStateBase(); //work the current state

    }

    public Player GetTarget(){
        //If enemy is too far away from the player, they will be despawned
        if(myEnemy.tag != "HVT" && Vector3.Distance(myEnemy.transform.position,player.transform.position) > 55f){
            Spawner.currentEnemies--;
            Destroy(myEnemy.gameObject);
            
        }

        //are we close enough?
        if(Vector3.Distance(myEnemy.transform.position,player.transform.position) > myEnemy.sightDistance + addedSight){
            // Debug.Log(Vector3.Distance(myEnemy.transform.position,player.transform.position));
            return null;
        }

        //is vision blocked by a wall?
        RaycastHit2D hit = Physics2D.Linecast(myEnemy.transform.position, player.transform.position,obstacles);
        if(hit.collider != null){
            return null;
        }

        if(Player.abilityEquipped == "Cloaking Device" && Player.abilityActive)
            return null;

        return player;

    }


}
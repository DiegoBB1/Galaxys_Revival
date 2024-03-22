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
    public static float sightDistance = 5;

    [Header("Pathfinding")]
    Pathfinder<Vector2> pathfinder;
    [SerializeField] float gridSize = 1f;

    //State machine====================================================
    //States go here
    EnemyAIState currentState;
    public EnemyAIIdleState idleState{get; private set;}
    public EnemyAIAggroState aggroState{get; private set;}
    public EnemyAIPatrolState patrolState{get; private set;}
    // public CreatureAIInvestigateState investigateState{get; private set;}


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
        // investigateState = new CreatureAIInvestigateState(this);
        currentState = idleState;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        pathfinder = new Pathfinder<Vector2>(GetDistance,GetNeighbourNodes,1000);
    }


    void FixedUpdate()
    {
        currentState.UpdateStateBase(); //work the current state

    }

    public Player GetTarget(){
        //are we close enough?
        if(Vector3.Distance(myEnemy.transform.position,player.transform.position) > sightDistance){
            // Debug.Log(Vector3.Distance(myEnemy.transform.position,player.transform.position));
            return null;
        }

        //is vision blocked by a wall?
        RaycastHit2D hit = Physics2D.Linecast(myEnemy.transform.position, player.transform.position,obstacles);
        if(hit.collider != null){
            return null;
        }

        return player;

    }

    // public void SetColor(Color c){
    //     myEnemy.body.GetComponent<SpriteRenderer>().color = c;
    // }

    //pathfinding
    public float GetDistance(Vector2 A, Vector2 B)
    {
        return (A - B).sqrMagnitude; //Uses square magnitude to lessen the CPU time.
    }

    Dictionary<Vector2,float> GetNeighbourNodes(Vector2 pos)
    {
        Dictionary<Vector2, float> neighbours = new Dictionary<Vector2, float>();
        for (int i=-1;i<2;i++)
        {
            for (int j=-1;j<2;j++)
            {
                if (i == 0 && j == 0) continue;

                Vector2 dir = new Vector2(i, j)*gridSize;
                if (!Physics2D.Linecast(pos,pos+dir, obstacles))
                {
                    neighbours.Add(GetClosestNode( pos + dir), dir.magnitude);
                }
            }

        }
        return neighbours;
    }

    //find the closest spot on the grid to begin our pathfinding adventure
    Vector2 GetClosestNode(Vector2 target){
        return new Vector2(Mathf.Round(target.x/gridSize)*gridSize, Mathf.Round(target.y / gridSize) * gridSize);
    }

    public void GetMoveCommand(Vector2 target, ref List<Vector2> path) //passing path with ref argument so original path is changed
    {
        path.Clear();
        Vector2 closestNode = GetClosestNode(myEnemy.transform.position);
        if (pathfinder.GenerateAstarPath(closestNode, GetClosestNode(target), out path)) //Generate path between two points on grid that are close to the transform position and the assigned target.
        {
            path.Add(target); //add the final position as our last stop
        }



    }

    //simple wrapper to pathfind to our target
    public void GetTargetMoveCommand(ref List<Vector2> path){
        GetMoveCommand(player.transform.position, ref path);

    }


}
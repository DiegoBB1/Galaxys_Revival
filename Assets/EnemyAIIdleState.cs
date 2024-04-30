using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIIdleState : EnemyAIState
{

    public EnemyAIIdleState(EnemyAI enemyAI) : base(enemyAI){}

    Vector3 moveVec;

    public override void UpdateState()
    {
        // enemyAI.myEnemy.Stop();

        if(timer < 1.0f){
            enemyAI.myEnemy.MoveCreatureTransform(moveVec);
        }
        else if(timer < 3.0f){
            enemyAI.myEnemy.Stop();
        }
        else{
            MoveRandom();
            timer = 0;
        }

        if(enemyAI.GetTarget() != null){
            enemyAI.ChangeState(enemyAI.aggroState);
        }
    }

    public override void BeginState()
    {
        
    }

    public void MoveRandom(){
        moveVec = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
    }
}
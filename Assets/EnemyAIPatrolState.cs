using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIPatrolState : EnemyAIState
{
    public EnemyAIPatrolState(EnemyAI enemyAI) : base(enemyAI){}

    public override void BeginState()
    {
        MoveRandom();
    }
    Vector3 moveVec;
    public override void UpdateState()
    {
        if(timer > 1.5f){
            timer = 0;

            MoveRandom();
        }

        enemyAI.myEnemy.MoveCreatureTransform(moveVec);

        if(enemyAI.GetTarget() != null){
            enemyAI.ChangeState(enemyAI.aggroState);
        }
    }

    public void MoveRandom(){
        moveVec = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0);
    }

}
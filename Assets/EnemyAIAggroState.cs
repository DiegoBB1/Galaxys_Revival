using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIAggroState : EnemyAIState
{

    public EnemyAIAggroState(EnemyAI enemyAI) : base(enemyAI){}

    public override void BeginState()
    {
        // enemyAI.SetColor(Color.red);
    }

    public override void UpdateState()
    {
        if(enemyAI.GetTarget() != null){
            enemyAI.myEnemy.MoveToward(enemyAI.GetTarget().transform.position);
        }
        else{
            enemyAI.ChangeState(enemyAI.patrolState);
        }
        // }else{
        //     // enemyAI.ChangeState(enemyAI.investigateState);
        // }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Weapon.gameObject.SetActive(false);
        GameObject.Destroy(stateMachine.Target);
        GameObject.Destroy(stateMachine.gameObject);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {

    }
}

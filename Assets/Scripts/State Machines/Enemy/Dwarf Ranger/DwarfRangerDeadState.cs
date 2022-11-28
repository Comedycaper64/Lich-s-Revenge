using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfRangerDeadState : DwarfRangerBaseState
{
    public DwarfRangerDeadState(DwarfRangerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Weapon.gameObject.SetActive(false);
        GameObject.Instantiate(stateMachine.Bone, stateMachine.transform.position, Quaternion.identity);
        GameObject.Destroy(stateMachine.gameObject);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {

    }
}

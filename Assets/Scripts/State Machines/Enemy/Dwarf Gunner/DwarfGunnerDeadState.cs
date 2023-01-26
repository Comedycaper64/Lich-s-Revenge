using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Gunner
{
    public class DwarfGunnerDeadState : DwarfGunnerBaseState
    {
        public DwarfGunnerDeadState(DwarfGunnerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.ForceReceiver.enabled = false;
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
}
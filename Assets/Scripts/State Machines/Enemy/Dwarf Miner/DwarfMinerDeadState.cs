using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Enemy.Miner
{
    public class DwarfMinerDeadState : DwarfMinerBaseState
    {
        public DwarfMinerDeadState(DwarfMinerStateMachine stateMachine) : base(stateMachine)
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
}

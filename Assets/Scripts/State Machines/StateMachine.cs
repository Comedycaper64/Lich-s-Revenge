using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The base class inherited by all other StateMachines
public abstract class StateMachine : MonoBehaviour
{
    public static event EventHandler<GameObject> OnEnemyUnitDead;

    private State currentState;

    //Each statemachine continuously runs the "Tick" method of its current state
    public virtual void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public virtual void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    protected void EnemyUnitDied(GameObject unit)
    {
        OnEnemyUnitDead?.Invoke(this, unit);
    }
}

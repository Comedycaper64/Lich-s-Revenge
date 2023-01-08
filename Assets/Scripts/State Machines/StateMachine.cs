using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public static event EventHandler<GameObject> OnEnemyUnitDead;

    private State currentState;

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

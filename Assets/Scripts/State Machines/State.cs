using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The base class inherited by all other States
public abstract class State 
{
    //Defines the structure that each state should have
    public abstract void Enter();

    public abstract void Tick(float deltaTime);
    
    public abstract void Exit();

    public virtual string GetStateName()
    {
        return "State";
    }

    //An animation is sometimes triggered when a state is entered. This returns how much of the animation has been played at the moment of asking in normalised time
    protected float GetNormalizedTime(Animator animator)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}

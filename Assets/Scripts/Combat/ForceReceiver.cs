using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Force receiver deals with an entity's physics, e.g. gravity and being knocked back by an enemy attack
public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float drag;

    private Vector3 dampingVelocity;

    private Vector3 impact;
    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update() 
    {
        //If the entity is off the ground, then gravity-based force is gradually applied
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        //Impact is smoothly reduced to zero, based on drag
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent == null) {return;}

        //At a point where the impact is small enough, it's set to zero and the navmesh agent is enabled
        if (impact.sqrMagnitude <= 0.2f * 0.2f)
        {
            impact = Vector3.zero;
            agent.enabled = true;  
        }
    }

    //Adds a directional force that's applied to the entity. If the entity has a navmesh agent, it is disabled
    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)
        {
            agent.enabled = false;
        }
    }

    //Jumping is achieved by adding a vertical velocity to the entity
    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}

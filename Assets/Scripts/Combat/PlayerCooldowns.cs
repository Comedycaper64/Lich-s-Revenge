using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

public class PlayerCooldowns : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine stateMachine;

    private float dodgeCooldown;
    private float fireboltCooldown;
    private float fireballCooldown;
    private float aegisCooldown;

    private void Start() 
    {
        SetDodgeCooldown();
        SetFireboltCooldown();
        SetFireballCooldown();
        SetAegisCooldown();
    }

    void Update()
    {
        if (dodgeCooldown > 0f)
        {
            dodgeCooldown -= Time.deltaTime;
        }
        if (fireboltCooldown > 0f)
        {
            fireboltCooldown -= Time.deltaTime;
        }
        if (fireballCooldown > 0f)
        {
            fireballCooldown -= Time.deltaTime;
        }
        if (aegisCooldown > 0f)
        {
            aegisCooldown -= Time.deltaTime;
        }
    }

    public bool IsDodgeReady()
    {
        if (dodgeCooldown > 0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsFireboltReady()
    {
        if (fireboltCooldown > 0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsFireballReady()
    {
        if (fireballCooldown > 0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsAegisReady()
    {
        if (aegisCooldown > 0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetDodgeCooldown()
    {
        dodgeCooldown = stateMachine.LichStats.GetLichDodgeCooldown();
    }

    public void SetFireboltCooldown()
    {
        fireboltCooldown = stateMachine.FireboltStats.GetFireboltSpellCooldown();
    }

    public void SetFireballCooldown()
    {
        fireballCooldown = stateMachine.FireballStats.GetFireballSpellCooldown();
    }

    public void SetAegisCooldown()
    {
        aegisCooldown = stateMachine.LichStats.GetLichAegisBreakCooldown();
    }
}

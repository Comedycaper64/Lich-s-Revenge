using System;
using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

//Used by the player statemachine to find out if certain abilities are ready to be used.
public class PlayerCooldowns : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine stateMachine;

    private float dodgeCooldown;
    private float fireboltCooldown;
    private float fireballCooldown;
    private float aegisCooldown;
    private float mineCooldown;
    private Coroutine lichIframes;

    private void Awake() 
    {
        dodgeCooldown = 0;
        fireballCooldown = 0;
        fireboltCooldown = 0;
        aegisCooldown = 0;
        mineCooldown = 0;
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
        if (mineCooldown > 0f)
        {
            mineCooldown -= Time.deltaTime;
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

    public bool IsMineReady()
    {
        if (mineCooldown > 0f)
        {
            return false;
        }
        {
            return true;
        }
    }

    public float GetDodgeCooldownNormalised()
    {
        return dodgeCooldown / stateMachine.LichStats.GetLichDodgeCooldown();
    }

    public float GetFireboltCooldownNormalised()
    {
        return fireboltCooldown / stateMachine.FireboltStats.GetFireboltSpellCooldown();
    }

    public float GetFireballCooldownNormalised()
    {
        return fireballCooldown / stateMachine.FireballStats.GetFireballSpellCooldown();
    }

    public float GetAegisCooldownNormalised()
    {
        return aegisCooldown / stateMachine.LichStats.GetLichAbsorbCooldown();
    }

    public float GetMineCooldownNormalised()
    {
        return mineCooldown / stateMachine.LichStats.GetMineCooldown();
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
        aegisCooldown = stateMachine.LichStats.GetLichAbsorbCooldown();
    }
    
    public void SetMineCooldown()
    {
        mineCooldown = stateMachine.LichStats.GetMineCooldown();
    }

    //Activated when the player takes damage, gives a brief period of invincibility
    public void SetLichInvincibility()
    {
        if (lichIframes != null)
        {
            StopCoroutine(lichIframes);
        }
        lichIframes = StartCoroutine(LichIFrames());
    }

    public IEnumerator LichIFrames()
    {
        yield return new WaitForSeconds(stateMachine.LichStats.GetLichIframes());
        stateMachine.Health.SetInvulnerable(false);
    }
}

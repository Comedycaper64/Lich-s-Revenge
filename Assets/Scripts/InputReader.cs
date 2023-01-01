using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue {get; private set;}
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action AbsorbEvent;
    public bool isAttacking {get; private set;}
    public bool isAiming {get; private set;}
    public bool isBlocking {get; private set;}
    public bool isHealing {get; private set;}
    public bool isFireballing {get; private set;}

    public event Action KeyboardAndMouseInput;
    public event Action XboxGamepadInput;
    public event Action PlaystationGamepadInput;

    private Controls controls;

    void Awake()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();       
        InputSystem.onActionChange += InputActionChangeCallback; 
    }

    private void InputActionChangeCallback(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
            {
                InputAction receivedInputAction = (InputAction) obj;
                InputDevice lastDevice = receivedInputAction.activeControl.device;
 
                if(lastDevice.name.Equals("Keyboard") || lastDevice.name.Equals("Mouse"))
                {
                    KeyboardAndMouseInput?.Invoke();
                    //Debug.Log("Using Keyboard / Mouse");
                }
                else if (lastDevice.name.Equals("XInputControllerWindows"))
                {
                    XboxGamepadInput?.Invoke();
                    //Debug.Log("Using Xbox Controller");
                }
                else if (lastDevice.name.Equals("DualShock4GamepadHID"))
                {
                    PlaystationGamepadInput?.Invoke();
                    //Debug.Log("Using Playstation Controller");
                }
            }
    }

    public void ToggleCameraMovement(bool enable)
    {
        if (enable)
        {
            controls.Player.Look.Enable();
        }
        else
        {
            controls.Player.Look.Disable();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) {return;}

        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) {return;}

        DodgeEvent?.Invoke();
    }

    public void OnAbsorb(InputAction.CallbackContext context)
    {
        if (!context.performed) {return;}

        AbsorbEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {

    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAttacking = true;
        }
        else if (context.canceled)
        {
            isAttacking = false;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            isAiming = false;
        }
        else if (context.performed)
        {
            isAiming = true;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isBlocking = true;
        }
        else if (context.canceled)
        {
            isBlocking = false;
        }
    }

    public void OnFireball(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            isFireballing = true;
        }
        else if (context.canceled)
        {
            isFireballing = false;
        }
        //FireballEvent?.Invoke();
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isHealing = true;
        }
        else if (context.canceled)
        {
            isHealing = false;
        }
    }
}

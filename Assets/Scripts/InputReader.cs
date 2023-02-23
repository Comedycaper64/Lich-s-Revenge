using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public static bool controllerBeingUsed;

    public Vector2 MovementValue {get; private set;}
    public event Action JumpEvent;
    public event Action InteractEvent;
    public event Action DodgeEvent;
    public event Action AbsorbEvent;
    public event Action MenuEvent;
    public event Action MineEvent;
    public bool isAttacking {get; private set;}
    public bool isAiming {get; private set;}
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
                    controllerBeingUsed = false;
                    //Debug.Log("Using Keyboard / Mouse");
                }
                else if (lastDevice.name.Equals("XInputControllerWindows"))
                {
                    XboxGamepadInput?.Invoke();
                    controllerBeingUsed = true;
                    //Debug.Log("Using Xbox Controller");
                }
                else if (lastDevice.name.Equals("DualShock4GamepadHID"))
                {
                    PlaystationGamepadInput?.Invoke();
                    controllerBeingUsed = true;
                    //Debug.Log("Using Playstation Controller");
                }
                else
                {
                    XboxGamepadInput?.Invoke();
                    controllerBeingUsed = true;
                }
            }
    }

    // public void ToggleCameraMovement(bool enable)
    // {
    //     if (enable)
    //     {
    //         controls.Player.Look.Enable();
    //     }
    //     else
    //     {
    //         controls.Player.Look.Disable();
    //     }
    // }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (MenuManager.gameIsPaused || DialogueManager.Instance.inConversation) {return;}

        if (!context.performed) {return;}

        JumpEvent?.Invoke();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) {return;}

        InteractEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (MenuManager.gameIsPaused || DialogueManager.Instance.inConversation) {return;}

        if (!context.performed) {return;}

        DodgeEvent?.Invoke();
    }

    public void OnAbsorb(InputAction.CallbackContext context)
    {
        if (MenuManager.gameIsPaused || DialogueManager.Instance.inConversation) {return;}

        if (!context.performed) {return;}

        AbsorbEvent?.Invoke();
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) {return;}

        MenuEvent?.Invoke();
    }

    public void OnSetMine(InputAction.CallbackContext context)
    {
        if (MenuManager.gameIsPaused || DialogueManager.Instance.inConversation) {return;}

        if (!context.performed) {return;}

        MineEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (MenuManager.gameIsPaused || DialogueManager.Instance.inConversation) 
        {
            MovementValue = Vector2.zero;
            return;
        }

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
        if (MenuManager.gameIsPaused || DialogueManager.Instance.inConversation) 
        {
            isAttacking = false;
            return;
        }

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
        if (MenuManager.gameIsPaused || DialogueManager.Instance.inConversation) 
        {
            isAiming = false;
            return;
        }

        if (context.canceled)
        {
            isAiming = false;
        }
        else if (context.performed)
        {
            isAiming = true;
        }
    }

    public void OnFireball(InputAction.CallbackContext context)
    {
        if (MenuManager.gameIsPaused || DialogueManager.Instance.inConversation) 
        {
            isFireballing = false;
            return;
        }

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
        if (MenuManager.gameIsPaused || DialogueManager.Instance.inConversation) 
        {
            isHealing = false;
            return;
        }

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

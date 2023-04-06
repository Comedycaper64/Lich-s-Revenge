using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Script for dealing with player input. Unity InputManager runs methods depending on what the player has pressed. 
//Information about these inputs is accessed using InputAction.CallbackContext
public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public static bool controllerBeingUsed;

    //Various variables that other scripts use to figure out what the player's input is. 
    //Bools are used for actions that can be held, while events are for the instance of the player pressing the button

    public Vector2 MovementValue {get; private set;}
    public event Action JumpEvent;
    public event Action InteractEvent;
    public event Action DodgeEvent;
    public event Action AbsorbEvent;
    public event Action MenuEvent;
    public event Action MineEvent;
    public event Action AegisEvent;
    public bool isAttacking {get; private set;}
    public bool isAiming {get; private set;}
    public bool isHealing {get; private set;}
    public bool isFireballing {get; private set;}

    //Events for changing what button prompts the UI uses

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

    //On each input, test what device is causing it. Invoke an event accordingly.
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
                }
                else if (lastDevice.name.Equals("XInputControllerWindows"))
                {
                    XboxGamepadInput?.Invoke();
                    controllerBeingUsed = true;
                }
                else if (lastDevice.name.Equals("DualShock4GamepadHID"))
                {
                    PlaystationGamepadInput?.Invoke();
                    controllerBeingUsed = true;
                }
                else
                {
                    XboxGamepadInput?.Invoke();
                    controllerBeingUsed = true;
                }
            }
    }

    //After ensuring that the action is valid (actions are invalid if the game is paused / the player is in dialogue), trigger the relevant event or toggle a bool.
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

    public void OnAegis(InputAction.CallbackContext context)
    {
        if (MenuManager.gameIsPaused || DialogueManager.Instance.inConversation) {return;}

        if (!context.performed) {return;}

        AegisEvent?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;

    [SerializeField] private Transform abilityUIContainer;
    [SerializeField] private GameObject crosshairUI;
    [SerializeField] GameObject dashAbilityUI;
    [SerializeField] GameObject fireboltAbilityUI;
    [SerializeField] GameObject fireballAbilityUI;
    [SerializeField] GameObject aimAbilityUI;

    private void Awake() 
    {
        playerStateMachine = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        playerStateMachine.OnSwitchState += UpdateUI;
    }

    public void UpdateUI(object sender, State currentState)
    {
        if (currentState.GetStateName() == "FreeLookState")
        {
            ClearAbilityUIs();
            Instantiate(dashAbilityUI, abilityUIContainer);
            Instantiate(aimAbilityUI, abilityUIContainer);
            crosshairUI.SetActive(false);
            
        }
        else if (currentState.GetStateName() == "AimingState")
        {
            ClearAbilityUIs();
            Instantiate(dashAbilityUI, abilityUIContainer);
            Instantiate(fireboltAbilityUI, abilityUIContainer);
            Instantiate(fireballAbilityUI, abilityUIContainer);
            crosshairUI.SetActive(true);
        }
    }

    private void OnDestroy() 
    {
        playerStateMachine.OnSwitchState -= UpdateUI;
    }

    private void ClearAbilityUIs()
    {
        foreach (Transform abilityUI in abilityUIContainer)
        {
            Destroy(abilityUI.gameObject);
        }
    }
}

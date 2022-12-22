using System.Collections;
using System.Collections.Generic;
using Units.Player;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    private PlayerCooldowns playerCooldowns;

    [SerializeField] private Transform abilityUIContainer;
    [SerializeField] private GameObject crosshairUI;
    [SerializeField] GameObject dashAbilityUI;
    [SerializeField] GameObject fireboltAbilityUI;
    [SerializeField] GameObject fireballAbilityUI;
    [SerializeField] GameObject aimAbilityUI;
    [SerializeField] GameObject blockAbilityUI;
    [SerializeField] GameObject healAbilityUI;
    [SerializeField] GameObject absorbAbilityUI;

    private AbilityUI fireboltSlider;
    private AbilityUI fireballSlider;
    private AbilityUI aimSlider;
    private AbilityUI blockSlider;
    private AbilityUI dashSlider;

    private Color activeColour = new Color(255f, 255f, 255f, 1);
    private Color inactiveColour = new Color(80f, 80f, 80f, 0.1f);

    private void Awake() 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
        playerCooldowns = player.GetComponent<PlayerCooldowns>();
        playerStateMachine.OnSwitchState += UpdateUI;
        ClearAbilityUIs();
        fireboltSlider = Instantiate(fireboltAbilityUI, abilityUIContainer).GetComponent<AbilityUI>();
        fireballSlider = Instantiate(fireballAbilityUI, abilityUIContainer).GetComponent<AbilityUI>();
        aimSlider = Instantiate(aimAbilityUI, abilityUIContainer).GetComponent<AbilityUI>();
        dashSlider = Instantiate(dashAbilityUI, abilityUIContainer).GetComponent<AbilityUI>();
        Instantiate(healAbilityUI, abilityUIContainer);
        blockSlider = Instantiate(blockAbilityUI, abilityUIContainer).GetComponent<AbilityUI>();
        Instantiate(absorbAbilityUI, abilityUIContainer);
    }

    private void Update() 
    {
        if (!playerCooldowns.IsFireboltReady())
        {
            fireboltSlider.SetSliderValue(playerCooldowns.GetFireboltCooldownNormalised());
        }
        if (!playerCooldowns.IsFireballReady())
        {
            fireballSlider.SetSliderValue(playerCooldowns.GetFireballCooldownNormalised());
        }
        if (!playerCooldowns.IsDodgeReady())
        {
            dashSlider.SetSliderValue(playerCooldowns.GetDodgeCooldownNormalised());
        }
        if (!playerCooldowns.IsAegisReady())
        {
            blockSlider.SetSliderValue(playerCooldowns.GetAegisCooldownNormalised());
        }
    }

    public void UpdateUI(object sender, State currentState)
    {
        if (currentState.GetStateName() == "FreeLookState")
        {
            fireboltSlider.SetImageColour(inactiveColour);
            fireballSlider.SetImageColour(inactiveColour);
            aimSlider.SetImageColour(activeColour);
            blockSlider.SetImageColour(activeColour);

            crosshairUI.SetActive(false);
            
        }
        else if (currentState.GetStateName() == "AimingState")
        {
            fireboltSlider.SetImageColour(activeColour);
            fireballSlider.SetImageColour(activeColour);
            aimSlider.SetImageColour(inactiveColour);
            blockSlider.SetImageColour(inactiveColour);

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

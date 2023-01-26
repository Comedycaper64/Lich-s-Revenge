using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using TMPro;
using Units.Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    private PlayerCooldowns playerCooldowns;
    private Mana playerMana;
    private LichBones playerBones;
    private LichStats playerStats;
    private FireballStats fireballStats;
    private FireboltStats fireboltStats;
    private InputReader inputReader;

    [SerializeField] private Transform abilityUIContainer;
    [SerializeField] private Transform menuUITransform;
    [SerializeField] private GameObject crosshairUI;
    [SerializeField] GameObject dashAbilityUI;
    [SerializeField] GameObject fireboltAbilityUI;
    [SerializeField] GameObject fireballAbilityUI;
    [SerializeField] GameObject aimAbilityUI;
    [SerializeField] GameObject blockAbilityUI;
    [SerializeField] GameObject healAbilityUI;
    [SerializeField] GameObject absorbAbilityUI;
    [SerializeField] GameObject menuButtonUI;

    [Header("Lich References")]
    [SerializeField] private Image healthImage;
    [SerializeField] private Image manaImage;
    [SerializeField] private TextMeshProUGUI boneText;

    private AbilityUI fireboltSlider;
    private AbilityUI fireballSlider;
    private AbilityUI aimSlider;
    private AbilityUI blockSlider;
    private AbilityUI dashSlider;
    private AbilityUI healSlider;
    private AbilityUI absorbSlider;
    private AbilityUI menuSlider;

    private List<AbilityUI> sliders = new List<AbilityUI>();

    private string currentInput;

    private StateEnum currentState;

    private void Awake() 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
        playerCooldowns = player.GetComponent<PlayerCooldowns>();
        playerMana = player.GetComponent<Mana>();
        playerBones = player.GetComponent<LichBones>();
        playerStats = player.GetComponent<LichStats>();
        fireboltStats = player.GetComponent<FireboltStats>();
        fireballStats = player.GetComponent<FireballStats>();
        inputReader = player.GetComponent<InputReader>();
        playerStateMachine.OnSwitchState += UpdateUI;
        ClearAbilityUIs();
        sliders.Add(fireboltSlider = Instantiate(fireboltAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        sliders.Add(fireballSlider = Instantiate(fireballAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        sliders.Add(aimSlider = Instantiate(aimAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        sliders.Add(dashSlider = Instantiate(dashAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        sliders.Add(healSlider = Instantiate(healAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        sliders.Add(blockSlider = Instantiate(blockAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        sliders.Add(absorbSlider = Instantiate(absorbAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        sliders.Add(menuSlider = Instantiate(menuButtonUI, menuUITransform).GetComponent<AbilityUI>());
        currentState = StateEnum.FreeLook;

        inputReader.KeyboardAndMouseInput += OnKeyboardInput;
        inputReader.XboxGamepadInput += OnXboxInput;
        inputReader.PlaystationGamepadInput += OnPlaystationInput;
    }

    private void Update() 
    {
        if (!playerCooldowns.IsFireboltReady())
        {
            fireboltSlider.SetCooldownSliderValue(playerCooldowns.GetFireboltCooldownNormalised());
        }
        if (!playerCooldowns.IsFireballReady())
        {
            fireballSlider.SetCooldownSliderValue(playerCooldowns.GetFireballCooldownNormalised());
        }
        if (!playerCooldowns.IsDodgeReady())
        {
            dashSlider.SetCooldownSliderValue(playerCooldowns.GetDodgeCooldownNormalised());
        }
        if (!playerCooldowns.IsAegisReady())
        {
            blockSlider.SetCooldownSliderValue(playerCooldowns.GetAegisCooldownNormalised());
        }

        if (!playerMana.HasMana(playerStats.GetLichDodgeManaCost()))
        {
            dashSlider.SetManaSliderValue(1 - (playerMana.GetMana() / playerStats.GetLichDodgeManaCost()));
        }
        if (!playerMana.HasMana(fireboltStats.GetFireboltSpellManaCost()))
        {
            fireboltSlider.SetManaSliderValue(1 - (playerMana.GetMana() / fireboltStats.GetFireboltSpellManaCost()));
        }
        if (!playerMana.HasMana(fireballStats.GetFireballSpellManaCost()))
        {
            fireballSlider.SetManaSliderValue(1 - (playerMana.GetMana() / fireballStats.GetFireballSpellManaCost()));
        }

        if (playerBones.GetBones() < 1)
        {
            healSlider.SetManaSliderValue(1f);
            absorbSlider.SetManaSliderValue(1f);
        }
        else
        {
            healSlider.SetManaSliderValue(0f);
            absorbSlider.SetManaSliderValue(0f);
        }

    }

    public void UpdateUI(object sender, State state)
    {
        if (state.GetStateName() == "FreeLookState")
        {
            currentState = StateEnum.FreeLook;
            
        }
        else if (state.GetStateName() == "AimingState")
        {
            currentState = StateEnum.Aiming;
        }
        crosshairUI.SetActive(!IsFreeLookState());
        
        fireboltSlider.SetImageActive(!IsFreeLookState());
        fireballSlider.SetImageActive(!IsFreeLookState());
        aimSlider.SetImageActive(IsFreeLookState());
        blockSlider.SetImageActive(IsFreeLookState());
        menuSlider.SetImageActive(IsFreeLookState());
    }

    private bool IsFreeLookState()
    {
        if (currentState == StateEnum.FreeLook)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnPlaystationInput()
    {
        if (currentInput == "Playstation") {return;}

        foreach(AbilityUI slider in sliders)
        {
            slider.SetPlaystationUI();
        }
        currentInput = "Playstation";
    }

    private void OnXboxInput()
    {
        if (currentInput == "Xbox") {return;}

        foreach(AbilityUI slider in sliders)
        {
            slider.SetXboxUI();
        }
        currentInput = "Xbox";
    }

    private void OnKeyboardInput()
    {
        if (currentInput == "Keyboard") {return;}

        foreach(AbilityUI slider in sliders)
        {
            slider.SetKeyboardUI();
        }
        currentInput = "Keyboard";
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

    public Image GetHealthBar()
    {
        return healthImage;
    }

    public Image GetManaBar()
    {
        return manaImage;
    }

    public TextMeshProUGUI GetBoneText()
    {
        return boneText;
    }
}

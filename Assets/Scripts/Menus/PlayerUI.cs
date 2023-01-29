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
    private Health playerHealth;
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
    //[SerializeField] GameObject blockAbilityUI;
    [SerializeField] GameObject healAbilityUI;
    [SerializeField] GameObject absorbAbilityUI;
    [SerializeField] GameObject menuButtonUI;

    [Header("UI VFX")]
    [SerializeField] private GameObject uiDamageFlash;
    [SerializeField] private GameObject uiHealFlash;
    [SerializeField] private GameObject uiBoneFlash;

    [Header("Lich References")]
    [SerializeField] private Image healthImage;
    [SerializeField] private Image manaImage;
    [SerializeField] private TextMeshProUGUI boneText;

    private AbilityUI fireboltUI;
    private AbilityUI fireballUI;
    private AbilityUI aimUI;
    //private AbilityUI blockUI;
    private AbilityUI dashUI;
    private AbilityUI healUI;
    private AbilityUI absorbUI;
    private AbilityUI menuUI;

    [SerializeField] private List<AbilityUI> abilityUIs = new List<AbilityUI>();

    private string currentInput;

    private StateEnum currentState;

    private void Awake() 
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerStateMachine = player.GetComponent<PlayerStateMachine>();
        playerCooldowns = player.GetComponent<PlayerCooldowns>();
        playerHealth = player.GetComponent<Health>();
        playerMana = player.GetComponent<Mana>();
        playerBones = player.GetComponent<LichBones>();
        playerStats = player.GetComponent<LichStats>();
        fireboltStats = player.GetComponent<FireboltStats>();
        fireballStats = player.GetComponent<FireballStats>();
        inputReader = player.GetComponent<InputReader>();
        playerStateMachine.OnSwitchState += UpdateUI;
        ClearAbilityUIs();
        abilityUIs.Add(fireboltUI = Instantiate(fireboltAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(fireballUI = Instantiate(fireballAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(aimUI = Instantiate(aimAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(dashUI = Instantiate(dashAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(healUI = Instantiate(healAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        //abilityUIs.Add(blockUI = Instantiate(blockAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(absorbUI = Instantiate(absorbAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(menuUI = Instantiate(menuButtonUI, menuUITransform).GetComponent<AbilityUI>());
        currentState = StateEnum.FreeLook;

        playerHealth.OnTakeDamage += UITakeDamage;
        playerHealth.OnHeal += UIHeal;
        playerBones.OnBoneGet += UIBoneGet;

        inputReader.KeyboardAndMouseInput += OnKeyboardInput;
        inputReader.XboxGamepadInput += OnXboxInput;
        inputReader.PlaystationGamepadInput += OnPlaystationInput;
    }

    private void Update() 
    {
        if (!playerCooldowns.IsFireboltReady())
        {
            fireboltUI.SetCooldownSliderValue(playerCooldowns.GetFireboltCooldownNormalised());
        }
        else
        {
            fireboltUI.SetCooldownSliderValue(0f);
        }

        if (!playerCooldowns.IsFireballReady())
        {
            fireballUI.SetCooldownSliderValue(playerCooldowns.GetFireballCooldownNormalised());
        }
        else
        {
            fireballUI.SetCooldownSliderValue(0f);
        }

        if (!playerCooldowns.IsDodgeReady())
        {
            dashUI.SetCooldownSliderValue(playerCooldowns.GetDodgeCooldownNormalised());
        }
        else
        {
            dashUI.SetCooldownSliderValue(0f);
        }

        if (!playerCooldowns.IsAegisReady())
        {
            absorbUI.SetCooldownSliderValue(playerCooldowns.GetAegisCooldownNormalised());
        }
        else
        {
            absorbUI.SetCooldownSliderValue(0f);
        }


        if (!playerMana.HasMana(playerStats.GetLichDodgeManaCost()))
        {
            dashUI.SetManaSliderValue(1 - (playerMana.GetMana() / playerStats.GetLichDodgeManaCost()));
        }
        if (!playerMana.HasMana(fireboltStats.GetFireboltSpellManaCost()))
        {
            fireboltUI.SetManaSliderValue(1 - (playerMana.GetMana() / fireboltStats.GetFireboltSpellManaCost()));
        }
        if (!playerMana.HasMana(fireballStats.GetFireballSpellManaCost()))
        {
            fireballUI.SetManaSliderValue(1 - (playerMana.GetMana() / fireballStats.GetFireballSpellManaCost()));
        }
        if (!playerMana.HasMana(playerStats.GetLichAbsorbManaCost()))
        {
            absorbUI.SetManaSliderValue(1 - (playerMana.GetMana() / playerStats.GetLichAbsorbManaCost()));
        }

        if (playerBones.GetBones() < 1)
        {
            healUI.SetCooldownSliderValue(1f);
            //absorbUI.SetCooldownSliderValue(1f);
        }
        else
        {
            healUI.SetCooldownSliderValue(0f);
            //absorbUI.SetCooldownSliderValue(0f);
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
        
        fireboltUI.SetImageActive(!IsFreeLookState());
        fireballUI.SetImageActive(!IsFreeLookState());
        aimUI.SetImageActive(IsFreeLookState());
        //blockUI.SetImageActive(IsFreeLookState());
        menuUI.SetImageActive(IsFreeLookState());
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

    private void UIHeal()
    {
        GameObject healFlash = Instantiate(uiHealFlash, healthImage.transform);
        Destroy(healFlash, 3f);
    }

    private void UITakeDamage()
    {
        GameObject damageFlash = Instantiate(uiDamageFlash, healthImage.transform);
        Destroy(damageFlash, 3f);
    }

    private void UIBoneGet()
    {
        GameObject boneFlash = Instantiate(uiBoneFlash, boneText.transform);
        Destroy(boneFlash, 3f);
    }

    private void OnPlaystationInput()
    {
        if (currentInput == "Playstation") {return;}

        foreach(AbilityUI ui in abilityUIs)
        {
            ui.SetPlaystationUI();
        }
        currentInput = "Playstation";
    }

    private void OnXboxInput()
    {
        if (currentInput == "Xbox") {return;}

        foreach(AbilityUI ui in abilityUIs)
        {
            ui.SetXboxUI();
        }
        currentInput = "Xbox";
    }

    private void OnKeyboardInput()
    {
        if (currentInput == "Keyboard") {return;}

        foreach(AbilityUI ui in abilityUIs)
        {
            ui.SetKeyboardUI();
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

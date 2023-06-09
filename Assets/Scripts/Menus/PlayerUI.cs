using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using TMPro;
using Units.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//An important script that controls the UI elements visible during gameplay
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
    [SerializeField] GameObject jumpAbilityUI;
    [SerializeField] GameObject dashAbilityUI;
    [SerializeField] GameObject fireboltAbilityUI;
    [SerializeField] GameObject fireballAbilityUI;
    [SerializeField] GameObject aimAbilityUI;
    [SerializeField] GameObject mineAbilityUI;
    [SerializeField] GameObject healAbilityUI;
    [SerializeField] GameObject absorbAbilityUI;
    [SerializeField] GameObject blockAbilityUI;
    [SerializeField] GameObject menuButtonUI;

    [Header("UI VFX")]
    [SerializeField] private GameObject uiDamageFlash;
    [SerializeField] private GameObject uiHealFlash;
    [SerializeField] private GameObject uiBoneFlash;

    [Header("Lich References")]
    [SerializeField] private Image healthImage;
    [SerializeField] private Image manaImage;
    [SerializeField] private TextMeshProUGUI boneText;

    public AbilityUI jumpUI;
    public AbilityUI fireboltUI;
    public AbilityUI fireballUI;
    public AbilityUI aimUI;
    public AbilityUI mineUI;
    public AbilityUI dashUI;
    public AbilityUI healUI;
    public AbilityUI absorbUI;
    public AbilityUI blockUI;
    public AbilityUI menuUI;

    [SerializeField] private List<AbilityUI> abilityUIs = new List<AbilityUI>();

    private string currentInput;

    private StateEnum currentState;

    private void Awake() 
    {
        //Many components are required
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

        //At the start of the scene, creates all of the ability UIs in the bottom right of the screen.
        abilityUIs.Add(jumpUI = Instantiate(jumpAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(fireboltUI = Instantiate(fireboltAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(fireballUI = Instantiate(fireballAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(aimUI = Instantiate(aimAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(mineUI = Instantiate(mineAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(dashUI = Instantiate(dashAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(healUI = Instantiate(healAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(absorbUI = Instantiate(absorbAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
        abilityUIs.Add(menuUI = Instantiate(menuButtonUI, menuUITransform).GetComponent<AbilityUI>());

        //If it's the first level (the tutorial) or any of the scenarios, the ability UIs are hidden
        if ((SceneManager.GetActiveScene().buildIndex == 1) || SceneManager.GetActiveScene().buildIndex > 5)
        {
            foreach (AbilityUI abilityUI in abilityUIs)
            {
                abilityUI.gameObject.SetActive(false);
            }
            //Ability UI 0 is the menu button in the top right of the screen, so it stays active
            abilityUIs[0].gameObject.SetActive(true);
            menuUI.gameObject.SetActive(true);
        }
        
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
        //Recalculates the cooldown slider of each applicable ability UI during every frame
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

        if (!playerCooldowns.IsMineReady())
        {
            mineUI.SetCooldownSliderValue(playerCooldowns.GetMineCooldownNormalised());
        }
        else
        {
            mineUI.SetCooldownSliderValue(0f);
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

        //Recalculates the mana slider of each applicable ability UI during every frame
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

        //If the player has no bones, the heal and mine abilities are set to look unavailable
        if (playerBones.GetBones() < 1)
        {
            healUI.SetManaSliderValue(1f);
            mineUI.SetManaSliderValue(1f);
        }
        else
        {
            healUI.SetManaSliderValue(0f);
            mineUI.SetManaSliderValue(0f);
        }

    }

    //Activates a disabled ability. Used primarily in the tutorial when the player is unlocking their abilities.
    public void ActivateAbility(LichSkill skill)
    {
        switch(skill)
        {
            case LichSkill.aim:
                aimUI.gameObject.SetActive(true);
                break;
            case LichSkill.firebolt:
                fireboltUI.gameObject.SetActive(true);
                break;
            case LichSkill.fireball:
                fireballUI.gameObject.SetActive(true);
                break;
            case LichSkill.jump:
                jumpUI.gameObject.SetActive(true);
                break;
            case LichSkill.dash:
                dashUI.gameObject.SetActive(true);
                break;
            case LichSkill.heal:
                healUI.gameObject.SetActive(true);
                break;
            case LichSkill.mine:
                mineUI.gameObject.SetActive(true);
                break;
            case LichSkill.absorb:
                absorbUI.gameObject.SetActive(true);
                break;
            case LichSkill.block:
                abilityUIs.Add(blockUI = Instantiate(blockAbilityUI, abilityUIContainer).GetComponent<AbilityUI>());
                break;
        }
    }

    //UI is updated based on what state the player is in. Certain abilities cannot be used while aiming and vice versa
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
        mineUI.SetImageActive(IsFreeLookState());
        menuUI.SetImageActive(IsFreeLookState());
    }

    //Helper script for Update UI
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

    //Particle effects spawn in the UI upon healing, taking damage, or collecting a bone
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

    //Changes the button prompts in the ability UIs if the player starts using a different input method
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

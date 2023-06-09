using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//A resource used by the player for abilities
public class Mana : MonoBehaviour
{
    private float maxMana;
    [SerializeField] public Image manaBar;
    private float mana;
    private float manaRegenRate;

    private void Update() 
    {
        if (mana < maxMana)
        {
            mana += manaRegenRate * Time.deltaTime;
            UpdateManaBar();
        }
    }

    public void SetMaxMana(float newMana)
    {
        maxMana = newMana;
        mana = maxMana;
        UpdateManaBar();
    }

    public void SetManaRegenRate(float newRegenRate)
    {
        manaRegenRate = newRegenRate;
    }

    private float GetManaNormalised()
    {
        return mana / maxMana;
    }

    private void UpdateManaBar()
    {
        manaBar.fillAmount = GetManaNormalised();
    }

    public bool TryUseMana(float manaUse)
    {
        if (manaUse > mana) {return false;}
        
        mana -= manaUse;
        UpdateManaBar();
        return true;
    }

    public bool HasMana(float manaAmount)
    {
        return manaAmount < mana;
    }

    public float GetMana()
    {
        return mana;
    }

    public void UseMana(float manaUse)
    {
        mana = Mathf.Max(mana - manaUse, 0);
        UpdateManaBar();
    }
}

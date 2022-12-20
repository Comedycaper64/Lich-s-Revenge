using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichAegis : MonoBehaviour
{
    [SerializeField] private Mana lichMana;
    [SerializeField] private PlayerCooldowns cooldowns;
    //[SerializeField] private float parryWindow;
    //[SerializeField] private Material parryMaterial;
    //[SerializeField] private Material blockMaterial;

    //private MeshRenderer aegisRenderer;

    public bool canEnable = true;
    //private bool parrying = false;

    private void Awake() 
    {
        //aegisRenderer = GetComponent<MeshRenderer>();
    }

    public void ToggleCanEnable(bool canEnable)
    {
        this.canEnable = canEnable;
    }

    public void ToggleAegis(bool enable)
    {
        if (canEnable)
        {
            gameObject.SetActive(enable);
            //parrying = enable;
            // if (enable == true)
            // {
            //     StartCoroutine(AegisActivating());
            // }
        }
    }

    // public bool IsParrying()
    // {
    //     return parrying;
    // }

    // private IEnumerator AegisActivating()
    // {
    //     aegisRenderer.material = parryMaterial;
    //     yield return new WaitForSeconds(parryWindow);
    //     parrying = false;
    //     aegisRenderer.material = blockMaterial;
    // }

    public void DamageAegis(float damage)
    {
        if (!lichMana.TryUseMana(damage))
        {
            lichMana.UseMana(damage);
            ToggleAegis(false);
            ToggleCanEnable(false);
            cooldowns.SetAegisCooldown();
        }
    }
}

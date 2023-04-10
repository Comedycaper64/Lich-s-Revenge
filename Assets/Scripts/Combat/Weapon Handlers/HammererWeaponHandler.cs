using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that handles the Hammerer's attacks
public class HammererWeaponHandler : MonoBehaviour
{
    [SerializeField] private HammererWeapon weaponLogic;
    [SerializeField] private HammererSlam slamLogic;
    [SerializeField] public AudioClip attackSFX;
    private float slamRadius;
    private int playerLayerMask = 1 << 8;

    private void Start() 
    {
        weaponLogic.SetHandler(this); 
    }

    //Same as miner
    public void EnableWeapon()
    {
        weaponLogic.gameObject.SetActive(true);
        if (SoundManager.Instance)
        {
            AudioSource.PlayClipAtPoint(attackSFX, transform.position, SoundManager.Instance.GetSoundEffectVolume());
        }
    }

    public void DisableWeapon()
    {
        weaponLogic.gameObject.SetActive(false);
    }

    //To calculate if the player is within the range of the Slam attack (which happens at the end of a leap),
    //two objects are checked. To replicate the shape of a short cylinder, this method checks if the player is both within
    // a box and a sphere.
    public void Slam()
    {
        if (Physics.CheckBox(transform.position, new Vector3(slamRadius, 1f, slamRadius), Quaternion.identity, playerLayerMask))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, slamRadius, playerLayerMask);
            foreach (Collider collider in colliders)
            {
                weaponLogic.TrySlamAttack(collider);
            }
        }
    }

    public void SetSlamRadius(float slamRadius)
    {
        this.slamRadius = slamRadius;
    }

    public void SetAttack(float attack, float attackKnockback) => weaponLogic.SetAttack(attack, attackKnockback);

    public void EnableSlamVisual(bool enable) => slamLogic.EnableSlamVisual(enable);
    public void SetSlamVisualLocation(Vector3 newLocation) => slamLogic.SetSlamVisualLocation(newLocation);
    public void SetupSlamVisual(float slamRadius) => slamLogic.SetupSlamVisual(slamRadius);
}

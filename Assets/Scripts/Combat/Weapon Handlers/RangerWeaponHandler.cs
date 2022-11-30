using System.Collections;
using System.Collections.Generic;
using Units.Enemy.Ranger;
using UnityEngine;

public class RangerWeaponHandler : MonoBehaviour
{
    private DwarfRangerStateMachine stateMachine;

    private Vector3 playerDirection;

    [SerializeField] private GameObject rangerProjectile;
    [SerializeField] private Transform projectileEmitter;
    [SerializeField] private LineRenderer aimRenderer;
    [SerializeField] private Material aimingMaterial;
    [SerializeField] private Material firingMaterial;

    private void Awake() 
    {
        stateMachine = GetComponent<DwarfRangerStateMachine>();    
    }

    public void AimWeapon()
    {
        aimRenderer.enabled = true;
        aimRenderer.material = aimingMaterial;
        playerDirection = (stateMachine.Player.transform.position - stateMachine.transform.position);
        Vector3 playerPosition = stateMachine.Player.transform.position;
        playerPosition.y += 0.9f;
        Vector3[] positionArray = new Vector3[3] {projectileEmitter.position, playerPosition, playerPosition + (playerPosition - projectileEmitter.position)};
        aimRenderer.SetPositions(positionArray);
    }

    public void FireWeapon()
    {
        RangerWeapon projectile = Instantiate(rangerProjectile, projectileEmitter.position, Quaternion.LookRotation(playerDirection)).GetComponent<RangerWeapon>();
        projectile.SetCollider(stateMachine.GetComponent<CharacterController>());
        projectile.SetAttack(Mathf.RoundToInt(stateMachine.Stats.GetDwarfRangerAttack()), 5f);
        projectile.SetSpeed(15f);
        aimRenderer.material = firingMaterial;
    }

    public void StowWeapon()
    {
        aimRenderer.enabled = false;
    }
}

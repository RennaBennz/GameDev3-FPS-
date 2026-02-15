using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] float range;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform rangeOrigin;

    [Header("Firing")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate;

    [Header("Caliber Stats")]
    [SerializeField] int damage;
    [SerializeField] float bulletSpeed;

    [Header("Aiming")]
    [SerializeField] Transform turretHead;
    [SerializeField] float turnSpeed;

    float fireTimer;
    Transform playerTarget;

    void Update()
    {
        Vector3 origin = rangeOrigin != null ? rangeOrigin.position : transform.position;

        Collider[] hits = Physics.OverlapSphere(origin, range, playerLayer);
        playerTarget = hits.Length > 0 ? hits[0].transform : null;

        if (playerTarget == null)
            return;

        Aim();
        HandleFire();
    }

    void Aim()
    {
        if (turretHead == null) return;

        Vector3 dir = playerTarget.position - turretHead.position;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        turretHead.rotation = Quaternion.Lerp(
            turretHead.rotation,
            targetRot,
            Time.deltaTime * turnSpeed
        );
    }

    void HandleFire()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 1f;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            shootPos.position,
            shootPos.rotation
        );

        Damage dmg = bullet.GetComponent<Damage>();
        if (dmg != null)
        {
            dmg.SetDamage(damage);
        }
        Debug.Log("Fired at: " + shootPos.position);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 origin = rangeOrigin != null ? rangeOrigin.position : transform.position;
        Gizmos.DrawWireSphere(origin, range);
    }

}
using UnityEngine;

public class Turret : MonoBehaviour
{
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
    bool playerInTrigger;

    void Update()
    {
        if (!playerInTrigger)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = other.transform;
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            playerTarget = null;
        }
    }

    void HandleFire()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);

        Damage dmg = bullet.GetComponent<Damage>();
        if (dmg != null)
        {
            dmg.SetDamage(damage);
        }
        Debug.Log("Fired at: " + shootPos.position);
    }
}
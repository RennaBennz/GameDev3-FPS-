using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] float bulletSpeed;
    [SerializeField] float lifeTime;

    [SerializeField] Rigidbody rb;

    void Start()
    {
        if (rb != null)
            rb.linearVelocity = transform.forward * bulletSpeed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
            dmg.takeDamage(damageAmount);

        Destroy(gameObject);
    }

    // Lets each turret set its own stats
    public void SetDamage(int amount) => damageAmount = amount;

    public void SetSpeed(float speed)
    {
        bulletSpeed = speed;
        if (rb != null)
            rb.linearVelocity = transform.forward * bulletSpeed;
    }
}
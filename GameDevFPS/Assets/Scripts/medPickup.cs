using UnityEngine;

public class medPickup : MonoBehaviour
{
    [SerializeField] medStats med;

    bool pickedUp;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MEDPICKUP HIT: " + gameObject.name + " at " + transform.position);

        if (pickedUp)
            return;

        if (!other.CompareTag("Player"))
            return;

        IPickup pik = other.GetComponent<IPickup>();
        if (pik != null && med != null)
        {
            pickedUp = true;
            pik.getMedStats(med);
            Destroy(gameObject);
        }
    }
}

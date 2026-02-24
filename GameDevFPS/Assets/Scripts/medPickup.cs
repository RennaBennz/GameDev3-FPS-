using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] medStats med;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pik = other.GetComponent<IPickup>();

        if (pik != null)
        {
            pik.getMedStats(med);
            Destroy(gameObject);
        }
    }
}

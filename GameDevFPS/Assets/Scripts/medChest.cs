using UnityEngine;

public class medChest : MonoBehaviour
{
    [Header("Interact")]
    [SerializeField] string interactButton = "Interact";

    [Header("Spawn")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject[] medkitPickups; // world pickup prefabs (small/medium/large)
    [SerializeField] bool openOnce = true;

    [Header("Optional Visuals")]
    [SerializeField] Animator anim;
    [SerializeField] string openTrigger = "Open";

    bool playerInRange;
    bool opened;

    void Update()
    {
        if (!playerInRange)
            return;

        if (Input.GetButtonDown(interactButton))
        {
            Open();
        }
    }

    void Open()
    {
        if (openOnce && opened)
            return;

        opened = true;

        if (anim != null)
            anim.SetTrigger(openTrigger);

        if (spawnPoint == null || medkitPickups == null || medkitPickups.Length == 0)
            return;

        int randIndex = Random.Range(0, medkitPickups.Length);
        Instantiate(medkitPickups[randIndex], spawnPoint.position, spawnPoint.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}

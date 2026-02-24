using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // When the player touches this platform, tell the GameManager they won
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.YouWin();
        }
    }
}

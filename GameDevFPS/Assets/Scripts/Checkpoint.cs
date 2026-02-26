using System.Collections;
using UnityEngine;
public class CheckPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gamemanager.instance.playerSpawnPos.transform.position != transform.position)
        {
            gamemanager.instance.playerSpawnPos.transform.position = transform.position;
            StartCoroutine(showPop());
        }
    }




    IEnumerator showPop()
    {
        gamemanager.instance.checkPointPopup.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gamemanager.instance.checkPointPopup.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonfunctions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void resume()
    {
        gamemanager.instance.stateUnpuase();
    }

    public void resart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.stateUnpuase();
    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
   Application.Quit();
#endif
    }
}



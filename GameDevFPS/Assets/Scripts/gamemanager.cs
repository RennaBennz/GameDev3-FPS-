using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text gameGoalCountText;
    public Image playerHPBar;
    public GameObject playerDamageFlash;

    public GameObject player;
    public PlayerController playerscript;
    public bool isPuased;
    public GameObject playerSpawnPos;
    public GameObject checkPointPopup;

    float timeScaleOrig;

    int gameGoalCount;

    bool gameOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;

        player = GameObject.FindWithTag("Player");
        playerscript = player.GetComponent<PlayerController>();


        playerSpawnPos = GameObject.FindWithTag("Play Spawn Pos");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpuase();
            }
        }
    }

    public void statePause()
    {
        isPuased = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnpuase()
    {
        isPuased = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;

    }

    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;

        if (gameGoalCount < 0)
            gameGoalCount = 0;

        gameGoalCountText.text = gameGoalCount.ToString("F0");

    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);

    }


    public void YouWin()
    {
        // Prevent this from running multiple times
        if (gameOver)
            return;

        gameOver = true;

        Debug.Log("YOU WIN!");

        // Stop the game
        Time.timeScale = 0f;

        // you win
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

}

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
    public GameObject Player;
    public PlayerController PlayerScript;
    public bool isPaused;

    float timeScaleOrig;
    int gameGoalCount;

    bool gameOver;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        gameGoalCount = 0;
        timeScaleOrig = Time.timeScale;

        Player = GameObject.FindWithTag("Player");
        PlayerScript = Player.GetComponent<PlayerController>();

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
                stateUnpause();
            }
        }
    }

    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnpause()
    {
        isPaused = false;
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


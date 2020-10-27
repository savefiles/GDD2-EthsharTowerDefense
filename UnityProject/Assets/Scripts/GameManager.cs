using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Simple simpleton instance.
    public static GameManager instance;

    // Managers
    public EnemyManager enemyManager;
    public TowerManager towerManager;
    public LevelManager levelManager;
    public UIManager    uiManager;
    
    // Points
    public float points;
    private GameObject pointCounter;

    // Mana
    private GameObject manaCounter;

    // Pause menu things
    public GameObject pauseMenu;
    public GameObject resumeButton;

    // Game over menu
    public GameObject gameOverMenu;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure there's only one instance of this class.
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Instantiate the managers.
        enemyManager = new EnemyManager();
        towerManager = new TowerManager();
        levelManager = new LevelManager();
        uiManager    = new UIManager();

        // Get a reference to the point counter.
        pointCounter = GameObject.Find("Point_Counter");
        manaCounter = GameObject.Find("Mana_Counter");

        // Set the onclick of the buttons and scale pause menu.
        resumeButton.GetComponent<Button>().onClick.AddListener(() => { UnpauseGame(); });
        pauseMenu.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadSceneAsync(0); });

        // Make sure the game starts unpaused.
        UnpauseGame();

    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        enemyManager.Update(dt);
        uiManager.Update(dt);
        towerManager.Update();

        // Update point counter and mana counter
        pointCounter.GetComponent<Text>().text = "Points: " + points;
        manaCounter.GetComponent<Text>().text = "Wizard Components: " + towerManager.manaCurr;

        // Check if the user (un)paused the game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu.activeInHierarchy == false)
                PauseGame();
            else
                UnpauseGame();
        }

        // If the player took too much town damage
        if(enemyManager.townHealth <= 0)
        {
            if(isGameOver == false)
            {
                GameOver();
                isGameOver = true;
            }

        }
    }


    public void PauseGame()
    {
        // Stop time, show the pause menu.
        pauseMenu.SetActive(true);
        Time.timeScale = 0.000001f;
    }

    public void UnpauseGame()
    {
        // Hide the pause menu, start time again.
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        // Stop time, update point counter, show menu.
        gameOverMenu.SetActive(true);
        gameOverMenu.transform.GetChild(1).GetComponent<Text>().text = "Points: " + points;
        gameOverMenu.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(()=>{ SceneManager.LoadSceneAsync(0); });
        Time.timeScale = 0.000001f;
    }
}

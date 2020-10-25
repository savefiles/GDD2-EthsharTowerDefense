using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Pause menu things
    public GameObject pauseMenu;
    public GameObject resumeButton;

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

        // Set the onclick of the buttons and scale pause menu.
        resumeButton.GetComponent<Button>().onClick.AddListener(() => { UnpauseGame(); });


    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        enemyManager.Update(dt);
        uiManager.Update(dt);
        towerManager.Update();

        // Update point counter
        pointCounter.GetComponent<Text>().text = "Points: " + points;

        // Check if the user (un)paused the game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu.activeInHierarchy == false)
                PauseGame();
            else
                UnpauseGame();
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

}

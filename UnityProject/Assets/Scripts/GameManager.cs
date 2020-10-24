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
    }

}

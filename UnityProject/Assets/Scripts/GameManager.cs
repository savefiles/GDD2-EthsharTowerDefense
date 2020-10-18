using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Simple simpleton instance.
    public static GameManager instance;

    // Managers
    public EnemyManager enemyManager;
    public TowerManager towerManager;
    public LevelManager levelManager;
    public UIManager    uiManager;

    public int round = 0;

    public float timer;

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

        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        timer += dt;
        enemyManager.Update(dt);
        round = enemyManager.roundCounter;

        uiManager.Update();
    }

}

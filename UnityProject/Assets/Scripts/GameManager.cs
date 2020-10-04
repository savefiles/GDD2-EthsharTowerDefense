using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public EnemyManager enemyManager;
    public TowerManager towerManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = new EnemyManager();
        towerManager = new TowerManager();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        enemyManager.Update(dt);
    }
}

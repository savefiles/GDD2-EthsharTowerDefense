using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Manager class
// - Dummy container for data regarding each enemy.
// - The enemies will do the calculations themselves.

public class EnemyManager : MonoBehaviour
{
    private List<Enemy> enemies;        // List of all the current enemies on screen.
    private int roundCounter;           // Determines which enemies to spawn.

    public List<Vector3> enemyPath;     // The path of points that the enemies LERP between.

    void Start()
    {
        // Set default values.
        enemies.Clear();
        roundCounter = 0;

    }

    // Update is called once per frame
    void Update()
    {
        foreach(Enemy enemy in enemies) { enemy.Update(Time.deltaTime); }
    }
}

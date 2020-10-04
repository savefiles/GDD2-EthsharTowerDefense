using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Manager class
// - Dummy container for data regarding each enemy.
// - The enemies will do the calculations themselves.

public class EnemyManager
{
    private List<Enemy> enemies = new List<Enemy>();        // List of all the current enemies on screen.
    private int roundCounter = 0;                           // Determines which enemies to spawn.

    public List<Vector3> enemyPath;     // The path of points that the enemies LERP between.

    public EnemyManager()
    {
        Reset();
    }

    // Update is called from GameManager every frame.
    public void Update(float dt)
    {
        foreach (Enemy enemy in enemies) { enemy.Update(dt); }
    }

    public void Reset()
    {
        // Set/reset default values.
        enemies.Clear();
        roundCounter = 0;
    }


}

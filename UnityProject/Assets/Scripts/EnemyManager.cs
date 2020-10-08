using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy Manager class
// - Dummy container for data regarding each enemy.
// - The enemies will do the calculations themselves.

public class EnemyManager
{
    public List<Enemy> enemies = new List<Enemy>();         // List of all the current enemies on screen.
    public List<GameObject> enemyPrefabs;                   // All of the enemy prefabs (ordered by the enemy type struct).
    public List<Vector3> enemyPath;                         // The path of points that the enemies LERP between.

    private int roundCounter = 0;                           // Determines which enemies to spawn.
    private float timeSinceLastSpawn = 0.0f;                // The amount of seconds since the last enemy spawn.

    public EnemyManager()
    {
        Reset();

        // Add the enemy prefabs to the list.
        enemyPrefabs = new List<GameObject>();
        enemyPrefabs.Add(Resources.Load<GameObject>("Prefabs/TestEnemy"));
    }

    // Update is called from GameManager every frame.
    public void Update(float dt)
    {
        timeSinceLastSpawn += dt;
        if(timeSinceLastSpawn > 2.0f) 
        { 
            timeSinceLastSpawn = 0.0f; 
            enemies.Add(new Enemy(EnemyType.Normal, this));
        } 
        for (int i = 0; i < enemies.Count; i++) 
        { 
            enemies[i].Update(dt);
            if(enemies[i].markedForDeletion == true)
            {
                enemies.RemoveAt(i);
                i--;
            }
        }
    }

    public void Reset()
    {
        // Set/reset default values.
        enemies.Clear();
        roundCounter = 0;
    }


}

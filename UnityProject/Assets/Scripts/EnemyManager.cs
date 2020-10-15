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

    public int roundCounter = 0;                           // Determines which enemies to spawn.
    private float timeSinceLastSpawn = 0.0f;                // The amount of seconds since the last enemy spawn.
    private float timeSinceWaveStart = 0.0f;                // The amount of seconds since the wave started.

    private int enemiesLeftToSpawn;                         // The number of enemies left to spawn in this wave.
    private List<EnemyType> enemyTypeToSpawn;               // The enemy type to spawn in the coming waves.
    private float spawnRate;                                // The calculated amount of time it takes to spawn each enemy.

    private const float TOTALWAVETIME = 8.0f;               // The amount of time from the start of spawning until next wave.
    private const float SPAWNINGTIMEPERWAVE = 4.0f;         // The amount of time it takes to spawn all enemies in a wave.
    private const int   ROUNDSPERBOSS = 3;                  // How many rounds are there until a boss spawn (fixed).



    public EnemyManager()
    {
        enemyTypeToSpawn = new List<EnemyType>();

        // Reset the enemy manager.
        Reset();

        // Add the enemy prefabs to the list.
        enemyPrefabs = new List<GameObject>();
        enemyPrefabs.Add(Resources.Load<GameObject>("Prefabs/Enemy_Normal"));
        enemyPrefabs.Add(Resources.Load<GameObject>("Prefabs/Enemy_Fast"));
        enemyPrefabs.Add(Resources.Load<GameObject>("Prefabs/Enemy_Tank"));
        enemyPrefabs.Add(Resources.Load<GameObject>("Prefabs/Enemy_Boss"));
    }

    // Update is called from GameManager every frame.
    public void Update(float dt)
    {
        // Update the timers.
        timeSinceLastSpawn += dt;
        timeSinceWaveStart += dt;

        // Spawn the enemies (if applicable).
        if (enemiesLeftToSpawn > 0)
        {
            if (timeSinceLastSpawn > spawnRate)
            {
                timeSinceLastSpawn = 0.0f;
                enemies.Add(new Enemy(enemyTypeToSpawn[0], this));
                enemiesLeftToSpawn -= 1;
            }
        }

        // Update the existing enemies.
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Update(dt);
            if (enemies[i].markedForDeletion == true)
            {
                enemies.RemoveAt(i);
                i--;
            }
        }

        // Start the next wave (if applicable).
        if (timeSinceWaveStart > TOTALWAVETIME)
        {
            // Generate the wave.
            enemyTypeToSpawn.RemoveAt(0);
            roundCounter += 1;
            GenerateWave(roundCounter, enemyTypeToSpawn[0]);

            // Reset the timers and such.
            timeSinceLastSpawn = 0.0f;
            timeSinceWaveStart = 0.0f;

            // Get the enemy type of the next wave.
            if(roundCounter % ROUNDSPERBOSS == 0)
                enemyTypeToSpawn.Add(EnemyType.Boss);
            else
                enemyTypeToSpawn.Add((EnemyType) Random.Range(0, 3));
        }

    }

    public void Reset()
    {
        // Set/reset default values.
        enemies.Clear();
        roundCounter = 0;

        // Generate the first few waves.
        for(int i = 0; i < 3; i++)
        {
            enemyTypeToSpawn.Add((EnemyType)Random.Range(0, 3));
        }
    }

    // Method that generates a wave depending on wave number and enemy type.
    // - Wave number determines difficulty.
    public void GenerateWave(int waveNumber, EnemyType enemyType)
    {
        // Generate the difficulty based on a equation (use Desmos to see the graph).
        float difficultyScalar = 1.0f + (0.01f * Mathf.Pow((float) waveNumber, 2));
        float enemyAmountScalar = 1.0f;
        switch(enemyType)
        {
            case EnemyType.Normal:
            case EnemyType.Fast:
                enemyAmountScalar = 2.0f;
                break;
            case EnemyType.Tank:
                enemyAmountScalar = 0.3f;
                break;
            case EnemyType.Boss:
                enemyAmountScalar = 0.01f;
                break;
        }


        // Scale the number of enemies by the difficulty scalar (maybe also scale health?).
        enemiesLeftToSpawn = Mathf.CeilToInt(10 * difficultyScalar * enemyAmountScalar);

        spawnRate = SPAWNINGTIMEPERWAVE / (float) enemiesLeftToSpawn;
    }


}

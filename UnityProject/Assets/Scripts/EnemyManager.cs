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

    public int roundCounter = 0;                            // Determines which enemies to spawn.
    private float timeSinceWaveStart = 0.0f;                // The amount of seconds since the wave started.

    private const float TOTALWAVETIME = 8.0f;               // The amount of time from the start of spawning until next wave.
    private const float SPAWNINGTIMEPERWAVE = 4.0f;         // The amount of time it takes to spawn all enemies in a wave.
    private const int   ROUNDSPERBOSS = 3;                  // How many rounds are there until a boss spawn (fixed).

    private List<EnemyWave> currentlySpawnedWaves;          // List of waves that are currently on the screen.
    private List<EnemyWave> upcomingWaves;                  // List of waves that are after the currently spawned waves.

    private bool hasGameStarted = false;                    // Don't actually start anything until the button is pressed for first time.
    private int townHealth = 10;                            // The amount of hits a town can take.

    // Only allow three waves to spawn, keep track of the number of enemies/type to spawn.

    public EnemyManager()
    {

        currentlySpawnedWaves = new List<EnemyWave>();
        upcomingWaves = new List<EnemyWave>();

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
        // Don't start updating until the game starts.
        if(hasGameStarted == false)
            return;

        // Update the timers.
        timeSinceWaveStart += dt;
        
        // Spawn enemies from the active waves if applicable.
        AttemptEnemySpawn(dt);
           

        // Update the existing enemies.
        for (int i = 0; i < enemies.Count; i++) {
            if (enemies[i].markedForDeletion == true)
            {
                enemies.RemoveAt(i);
                i--;
            }
            else
                enemies[i].Update(dt);
        }

        // Start the next wave (if applicable).
        if (timeSinceWaveStart > TOTALWAVETIME)
        {
            // Spawn the first wave from upcoming waves.
            SpawnNextWave();
        }
    }

    public void Reset()
    {
        // Set/reset default values.
        enemies.Clear();
        roundCounter = 0;

        // Generate the first few waves (just normal enemies).
        for (int i = 0; i < 3; i++)
        {
            // Get the enemy type of the next wave.
            EnemyType newWaveEnemyType = EnemyType.Normal;
            upcomingWaves.Add(GenerateWave(roundCounter + i + 1, newWaveEnemyType));
        }
    }


    // Helper method that spawns enemies from active waves.
    private void AttemptEnemySpawn(float dt)
    {
        // Loop through all of the active waves.
        for(int i = 0; i < currentlySpawnedWaves.Count; i++)
        {
            // Spawn the enemies (if applicable).
            EnemyWave enemyWave = currentlySpawnedWaves[i];
            enemyWave.timeSinceLastSpawn += dt;
            if (enemyWave.enemiesLeftToSpawn > 0)
            {
                if (enemyWave.timeSinceLastSpawn > enemyWave.spawnRate)
                {
                    enemyWave.timeSinceLastSpawn = 0.0f;
                    enemies.Add(new Enemy(enemyWave.enemyTypeToSpawn, this));
                    enemyWave.enemiesLeftToSpawn -= 1;
                }
            }
            // If there are no more enemies to spawn, remove the wave.
            else
            {
                currentlySpawnedWaves.Remove(enemyWave);
                i--;
            }
        }

    }

    // Helper method that is called from either a timer or button press.
    // - Spawns the first wave in the upcoming waves list.
    public void SpawnNextWave()
    {
        // Reset the timers and such.
        timeSinceWaveStart = 0.0f;

        // Swap the wave to the currently spawned waves list.
        currentlySpawnedWaves.Add(upcomingWaves[0]);
        upcomingWaves.RemoveAt(0);

        // Get the enemy type of the next wave.
        EnemyType newWaveEnemyType;
        if (roundCounter % ROUNDSPERBOSS == 0)
            newWaveEnemyType = EnemyType.Boss;
        else
            newWaveEnemyType = (EnemyType)Random.Range(0, 3);

        // Generate the wave, add to the back of list (won't be spawned for a few rounds).
        roundCounter += 1;
        upcomingWaves.Add(GenerateWave(roundCounter + upcomingWaves.Count, newWaveEnemyType));

        // If the game hasn't started yet, start it.
        if (hasGameStarted == false)
            hasGameStarted = true;
    }



    // Method that generates a wave depending on wave number and enemy type.
    // - Wave number determines difficulty.
    public EnemyWave GenerateWave(int waveNumber, EnemyType enemyType)
    {
        EnemyWave enemyWave = new EnemyWave();

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
        enemyWave.enemiesLeftToSpawn = Mathf.CeilToInt(5 * difficultyScalar * enemyAmountScalar);
        enemyWave.spawnRate = SPAWNINGTIMEPERWAVE / (float) enemyWave.enemiesLeftToSpawn;
        enemyWave.enemyTypeToSpawn = enemyType;
        enemyWave.timeSinceLastSpawn = 0.0f;

        return enemyWave;
    }

    // Deal damage to the town, meaning an enemy has reached the end.
    // - Bosses deal more damage, so we have the boss boolean
    public void TakeTownDamage(bool boss)
    {
        if(boss)
            townHealth -= 3;
        else
            townHealth -= 1;
    }


    // Class to hold all of the information about a wave.
    public class EnemyWave
    {
        public int enemiesLeftToSpawn;                         // The number of enemies left to spawn in this wave.
        public EnemyType enemyTypeToSpawn;                     // The enemy type to spawn for this wave.
        public float spawnRate;                                // The calculated amount of time it takes to spawn each enemy.

        public float timeSinceLastSpawn;                       // The amount of time since an enemy has been spawned from this wave.
    }
}

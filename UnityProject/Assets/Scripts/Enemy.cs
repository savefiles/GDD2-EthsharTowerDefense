using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy class
// - Contains all the information on each enemy
// - Does the path calculations locally using an internal timer.

public class Enemy
{
    private readonly EnemyManager enemyManager;
    private float timer;                // The amount of time this enemy has been alive.

    private int health;                 // Heath of the unit
    private Vector3 position;           // Current position on the screen.
    private int targetPositionIndex;    // The index in the position list of the target position.
    private float timePerPosition;      // The amount of time it takes for the enemy to go from each position.
    
    public Enemy(EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
    }


    // Call every frame from enemy manager.
    public void Update(float dt)
    {
        timer += dt;            // Update the current timer.
        Move();                 // Move the enemy.
    }

    // Move the enemy along the path in the enemy manager.
    private void Move()
    {
        float percent = (timer - timePerPosition * (targetPositionIndex - 1)) / timer;      // Calculate the percentage between the last and current target postion
        position = Vector3.Lerp(enemyManager.enemyPath[targetPositionIndex - 1],            // LERP between the last and current target position.
                                enemyManager.enemyPath[targetPositionIndex],
                                percent);
        
        // If the enemy is sufficiently close to the target position, switch target position.
        if(Vector3.SqrMagnitude(position - enemyManager.enemyPath[targetPositionIndex]) < 1e-3)
        {
            targetPositionIndex += 1;
        }
    }
}

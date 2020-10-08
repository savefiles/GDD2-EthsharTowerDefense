using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy class
// - Contains all the information on each enemy
// - Does the path calculations locally using an internal timer.

public class Enemy
{
    private readonly EnemyManager enemyManager;
    private EnemyType type;             // The type of enemy (normal, fast, etc).
    private float timer;                // The amount of time this enemy has been alive.

    private int health;                 // Heath of the unit
    private Vector3 position;           // Current position on the screen.
    private int targetPositionIndex;    // The index in the position list of the target position.
    private float speed;                // The inverse of the amount of time it takes for the enemy to go from each position.

    private GameObject gameObject;      // The game object that the enemy is represented by in the scene.

    public bool markedForDeletion;      // Flag that tells the EnemyManager that this enemy has reached the end or died.
    
    public Enemy(EnemyType type, EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
        this.type = type;

        // Instantiate the variables.
        timer = 0.0f;
        health = 10;
        targetPositionIndex = 1;
        position = enemyManager.enemyPath[0];
        speed = 1.0f;
        markedForDeletion = false;

        // Create a new object for this enemy based on type.
        gameObject = GameObject.Instantiate(enemyManager.enemyPrefabs[(int)type], position, Quaternion.identity);
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
        float timeSpendOnThisPoint = timer - ((1/speed) * (targetPositionIndex - 1));       
        float percent = timeSpendOnThisPoint * speed;                                       // Calculate the percentage between the last and current target postion

        position = Vector3.Lerp(enemyManager.enemyPath[targetPositionIndex - 1],            // LERP between the last and current target position.
                                enemyManager.enemyPath[targetPositionIndex],
                                percent);
        gameObject.transform.position = position;                                           // Set the postiion of the game object.
        
        // If the enemy is sufficiently close to the target position, switch target position.
        if(Vector3.SqrMagnitude(position - enemyManager.enemyPath[targetPositionIndex]) < 1e-3)
        {
            targetPositionIndex += 1;
        }

        // If the enemy has reached the end of the path, destroy the game object.
        if(targetPositionIndex == enemyManager.enemyPath.Count)
        {
            GameObject.Destroy(gameObject);
            markedForDeletion = true;
            //enemyManager.enemies.Remove(this);
        }
    }
}

public enum EnemyType
{
    Normal = 0
}

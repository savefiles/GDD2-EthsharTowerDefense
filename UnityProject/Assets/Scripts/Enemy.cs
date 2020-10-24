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
    public float timeSinceLastPoint { get; private set; }   // The amount of time that has passed since the enemy left the last point.

    private float health;                                   // Current health of the unit
    private float maxHealth;                                // The amount of health this unit started with.
    private Vector3 position;                               // Current position on the screen.
    public int targetPositionIndex { get; private set; }    // The index in the position list of the target position.
    private float speed;                                    // The inverse of the amount of time it takes for the enemy to go from each position.
    private float distanceToNextPosition;                   // The distance between the last target and the current target.

    public GameObject gameObject;      // The game object that the enemy is represented by in the scene.
    private Transform healthBar;       // Reference to the health bar transform (used to shorten names)
    private float healthBarScale;      // The default scale of the health bar (used to show damage taken).
    private float widthToPos;          // Based on the health bar scale, convert from width to position.

    public bool markedForDeletion;     // Flag that tells the EnemyManager that this enemy has reached the end or died.

    // Getters/Setters

    public Enemy(EnemyType type, EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
        this.type = type;

        // Instantiate the variables.
        health = 10;
        targetPositionIndex = 1;
        position = enemyManager.enemyPath[0];
        markedForDeletion = false;
        distanceToNextPosition = Vector3.Distance(enemyManager.enemyPath[targetPositionIndex - 1], enemyManager.enemyPath[targetPositionIndex]);

        // Change default speed/health depending on enemy type.
        switch (type)
        {
            case EnemyType.Normal:
                health = 15.0f;
                speed = 2.0f;
                break;
            case EnemyType.Fast:
                health = 6.0f;
                speed = 3.5f;
                break;
            case EnemyType.Tank:
                health = 30.0f;
                speed = 1.2f;
                break;
            case EnemyType.Boss:
                health = 100.0f;
                speed = 0.6f;
                break;
        }
        maxHealth = health;

        // Create a new object for this enemy based on type.
        gameObject = GameObject.Instantiate(enemyManager.enemyPrefabs[(int)type], position, Quaternion.identity);
        gameObject.GetComponent<EnemyMB>().enemyRef = this;
        healthBar = gameObject.transform.GetChild(1).transform;
        healthBarScale = healthBar.localScale.x;
        widthToPos = 0.221f / healthBarScale;
    }


    // Call every frame from enemy manager.
    public void Update(float dt)
    {
        timer += dt;            // Update the current timer.
        timeSinceLastPoint += dt;
        Move();                 // Move the enemy.

        // Update the health bar based on the enemy's current health.
        healthBar.localScale = new Vector3((health / maxHealth) * healthBarScale,
                                           healthBar.localScale.y,
                                           healthBar.localScale.z);
        healthBar.localPosition = new Vector3((0.5f - (health / (2 * maxHealth))) * healthBarScale * -widthToPos,
                                              healthBar.localPosition.y,
                                              healthBar.localPosition.z);
    }

    // Move the enemy along the path in the enemy manager.
    private void Move()
    {
        float percent = timeSinceLastPoint / (distanceToNextPosition / speed);              // Calculate the percentage between the last and current target postion

        position = Vector3.Lerp(enemyManager.enemyPath[targetPositionIndex - 1],            // LERP between the last and current target position.
                                enemyManager.enemyPath[targetPositionIndex],
                                percent);
        gameObject.transform.position = position;                                           // Set the postiion of the game object.
        
        // If the enemy is sufficiently close to the target position, switch target position.
        if(Vector3.SqrMagnitude(position - enemyManager.enemyPath[targetPositionIndex]) < 1e-3)
        {
            targetPositionIndex += 1;
            timeSinceLastPoint = 0.0f;
            // If the enemy reached the end of the path
            if (targetPositionIndex == enemyManager.enemyPath.Count)
            {
                GameObject.Destroy(gameObject);
                markedForDeletion = true;
                // Deal damage to town
                enemyManager.TakeTownDamage(type == EnemyType.Boss);
            }
            else
            {
                // CHANGE DIRECTION THAT THE ENEMY IS FACING.
                Vector3 newForwardVector = Vector3.Normalize(enemyManager.enemyPath[targetPositionIndex] - enemyManager.enemyPath[targetPositionIndex - 1]);
                gameObject.transform.GetChild(0).transform.right = (Vector2) newForwardVector;

                distanceToNextPosition = Vector3.Distance(enemyManager.enemyPath[targetPositionIndex - 1], enemyManager.enemyPath[targetPositionIndex]);
            }
        }
    }

    // Function that deals damage to the current enemy. Returns if the damage dealt a killing blow.
    public bool TakeDamage(float damage)
    {
        health -= damage;
        // Check if the enemy is dead.
        if(health <= 0.1f)
        {
            GameObject.Destroy(gameObject);
            markedForDeletion = true;
            return true;
        }
        return false;
    }
}

public enum EnemyType
{
    Normal = 0,
    Fast = 1,
    Tank = 2,
    Boss = 3
}

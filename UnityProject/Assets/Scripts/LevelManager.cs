using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Level Manager class
// - Holds references for everything level related.
// - Level class itself holds the data for the level.
public class LevelManager
{

    private List<Sprite> levelList;
    public Level currentLevel;
    public Sprite currentLevelSprite;
    public GameObject levelGameObject;

    private Level level;

    public LevelManager()
    {
        // Get references to game objects in the scene.
        levelGameObject = GameObject.Find("Level Map");

        // Load the demo level.
        level = new Level(this);
        level.LoadLevel();
        currentLevel = level;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

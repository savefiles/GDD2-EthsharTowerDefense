using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private List<Sprite> levelList;
    public int currentLevel;
    public Sprite currentLevelSprite;
    public GameObject levelGameObject;

    private Level level0;

    void Start()
    {
        level0 = new Level("", this);
        level0.LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

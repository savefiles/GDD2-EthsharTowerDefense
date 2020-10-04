using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private List<Sprite> levelList;
    public int currentLevel;
    public Sprite currentLevelSprite;
    public GameObject levelGameObject;

    public void LoadLevel()
    {
        levelGameObject.GetComponent<SpriteRenderer>().sprite = currentLevelSprite;
    }

    void Start()
    {
        int currentLevel = 0;
        levelList = new List<Sprite>();
        levelList.Add(Resources.Load<Sprite>("Levels/level0.png"));
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

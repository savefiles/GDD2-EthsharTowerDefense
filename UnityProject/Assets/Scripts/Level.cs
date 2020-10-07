using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dummy container for the properties of a level.
// - Set and used from the LevelManager.
// - Takes in a path to a folder, which contains data for the level.
public class Level
{
    private readonly LevelManager lm;
    private readonly SpriteRenderer sr;
    private List<Vector2> path;             // Collection of points that represent the path from start to end.
    private Sprite levelBackground;         // The image that represents the background.


    public Level(string folderPath, LevelManager lm)
    {
        this.lm = lm;
        this.sr = lm.levelGameObject.GetComponent<SpriteRenderer>();

        // For testing, we hardcode points.
        path = new List<Vector2>();
        path.Add(new Vector2(0.0f, 0.195f));
        path.Add(new Vector2(0.225f, 0.195f));
        path.Add(new Vector2(0.225f, 0.395f));

        levelBackground = Resources.Load<Sprite>("Levels/Level0/level0");
    }

    public void LoadLevel()
    {
        sr.sprite = levelBackground;
    }

    private Vector3 GetTopLeftCorner()
    {
        Bounds spriteBounds = sr.sprite.bounds;
        Vector3 topLeft = new Vector3(spriteBounds.center.x - spriteBounds.extents.x,
                                      spriteBounds.center.y + spriteBounds.extents.y,
                                      spriteBounds.center.z);
        return topLeft;
    }
}

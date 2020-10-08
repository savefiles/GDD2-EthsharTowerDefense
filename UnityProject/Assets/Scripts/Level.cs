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
    private List<Vector2> relativePath;          // Collection of poitns that represent the path from start to end (relative to the image).
    private List<Vector3> worldPath;             // Collection of points that represent the path from start to end (world coords).
    private Sprite levelBackground;              // The image that represents the background.


    public Level(string folderPath, LevelManager lm)
    {
        this.lm = lm;
        this.sr = lm.levelGameObject.GetComponent<SpriteRenderer>();

        // For testing, we hardcode points.
        relativePath = new List<Vector2>();
        relativePath.Add(new Vector2(0.0f, 0.195f));
        relativePath.Add(new Vector2(0.225f, 0.195f));
        relativePath.Add(new Vector2(0.225f, 0.395f));

        levelBackground = Resources.Load<Sprite>("Levels/Level0/level0");
    }

    public void LoadLevel()
    {
        sr.sprite = levelBackground;                                     // Set the actual sprite to this level's sprite.

        worldPath = new List<Vector3>();
        foreach (Vector2 point in relativePath) { worldPath.Add(ConvertToWorldCoords(point)); }
        GameManager.instance.enemyManager.enemyPath = worldPath;         // Update the enemy manager's path to this level's path.
    }

    private Vector3 ConvertToWorldCoords(Vector2 point)
    {
        Bounds spriteBounds = sr.sprite.bounds;
        Vector2 topLeft = new Vector2(spriteBounds.center.x - spriteBounds.extents.x,
                                      spriteBounds.center.y + spriteBounds.extents.y);
        Vector3 worldPoint = new Vector3(topLeft.x + point.x * spriteBounds.size.x,
                                         topLeft.y - point.y * spriteBounds.size.y,
                                         -1.0f);
        return worldPoint;
    }
}

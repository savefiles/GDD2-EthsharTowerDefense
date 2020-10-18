using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI Manager class
// - Handles all of the user input.
// - Draws the UI to the screen (switching scenes?)


public class UIManager
{
    // Prefabs
    GameObject button_spawnArcher;
    GameObject button_spawnBomb;
    GameObject button_spawnMage;

    GameObject canvas;

    bool isSpawningUIActive;


    public UIManager()
    {
        // Find the proper prefabs.
        button_spawnArcher = Resources.Load<GameObject>("Prefabs/Button_SpawnArcher");
        button_spawnBomb   = Resources.Load<GameObject>("Prefabs/Button_SpawnBomb");
        button_spawnMage   = Resources.Load<GameObject>("Prefabs/Button_SpawnMage");

        // Set the canvas object reference.
        canvas = GameObject.Find("Canvas");
    }

    // Called from Game Manager
    public void Update()
    {
        CheckClicks();
    }

    // Check if the user has clicked on the screen (only check in game scene).
    private void CheckClicks()
    {
        // If they press right click on the screen.
        if (Input.GetMouseButtonDown(1))
        {
            // Check if the location they clicked on the screen

            // If so, make the UI pop up
            Vector3 mousePos = Input.mousePosition;
            SpawnTowerPlacementButtons(mousePos);
        }
    }

    private void SpawnTowerPlacementButtons(Vector3 mousePos)
    {
        // Three relative position vectors for the buttons (in pixels)
        Vector3 archerButtonPos = mousePos + new Vector3(-35f, 10f, -9.0f);
        Vector3 mageButtonPos   = mousePos + new Vector3(  0f, 30f, -9.0f);
        Vector3 bombButtonPos   = mousePos + new Vector3( 35f, 10f, -9.0f);

        GameObject.Instantiate(button_spawnArcher, archerButtonPos, Quaternion.identity, canvas.transform);
        GameObject.Instantiate(button_spawnMage,   mageButtonPos,   Quaternion.identity, canvas.transform);
        GameObject.Instantiate(button_spawnBomb,   bombButtonPos,   Quaternion.identity, canvas.transform);
    }
}

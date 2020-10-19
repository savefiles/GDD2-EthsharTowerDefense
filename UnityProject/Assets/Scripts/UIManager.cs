using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI Manager class
// - Handles all of the user input.
// - Draws the UI to the screen (switching scenes?)


public class UIManager
{
    // Prefabs
    GameObject button_spawnArcher;
    GameObject button_spawnBomb;
    GameObject button_spawnMage;


    GameObject canvas;      // Canvas object
    GameObject camera;      // Viewing camera
    TowerManager tm;        // Tower manager



    List<GameObject> createdTowerButtons = new List<GameObject>();          // List to hold the dynamically created buttons.
    bool isSpawningUIActive = false;                                        // Should the user be allowed to spawn the tower ui


    public UIManager()
    {
        // Find the proper prefabs.
        button_spawnArcher = Resources.Load<GameObject>("Prefabs/Button_SpawnArcher");
        button_spawnBomb   = Resources.Load<GameObject>("Prefabs/Button_SpawnBomb");
        button_spawnMage   = Resources.Load<GameObject>("Prefabs/Button_SpawnMage");

        
        canvas = GameObject.Find("Canvas");
        camera = GameObject.Find("Main Camera");
        tm = GameManager.instance.towerManager;
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
            // Check the validity of the location they clicked on the screen
            Vector3 mousePos = Input.mousePosition;
            if(tm.PathCheck(camera.GetComponent<Camera>().ScreenToWorldPoint(mousePos), 0))       // No idea what the size should be
                return;

            // If the tower spawn UI is already up, cull the previous buttons.
            if (isSpawningUIActive == true)
            {
                foreach (GameObject button in createdTowerButtons)
                    GameObject.Destroy(button);
                createdTowerButtons.Clear();
            }




            // If so, make the UI pop up
            SpawnTowerPlacementButtons(mousePos);
        }
    }

    private void SpawnTowerPlacementButtons(Vector3 mousePos)
    {
        // Three relative position vectors for the buttons (in pixels)
        Vector3 archerButtonPos = mousePos + new Vector3(-35f, 10f, -9.0f);
        Vector3 mageButtonPos   = mousePos + new Vector3(  0f, 30f, -9.0f);
        Vector3 bombButtonPos   = mousePos + new Vector3( 35f, 10f, -9.0f);

        // Instantiate the game objects
        GameObject achrButton = GameObject.Instantiate(button_spawnArcher, archerButtonPos, Quaternion.identity, canvas.transform);
        GameObject mageButton = GameObject.Instantiate(button_spawnMage,   mageButtonPos,   Quaternion.identity, canvas.transform);
        GameObject bombButton = GameObject.Instantiate(button_spawnBomb,   bombButtonPos,   Quaternion.identity, canvas.transform);

        // Assign the onclick functions (call a function from tower manager with params)
        achrButton.GetComponent<Button>().onClick.AddListener(() => { SpawnTower(0); });
        mageButton.GetComponent<Button>().onClick.AddListener(() => { SpawnTower(1); });
        bombButton.GetComponent<Button>().onClick.AddListener(() => { SpawnTower(2); });

        // Add the buttons to the list.
        createdTowerButtons.Add(achrButton);
        createdTowerButtons.Add(mageButton);
        createdTowerButtons.Add(bombButton);

        // The buttons are currently displayed.
        isSpawningUIActive = true;
    }

    private void SpawnTower(int type)
    {
        // Call function from tower manager (type 0 = archer, type 1 = mage, type 2 = bomb)

        // Set spawning active to false, destroy buttons.
        isSpawningUIActive = false;
        foreach(GameObject button in createdTowerButtons)
            GameObject.Destroy(button);
        createdTowerButtons.Clear();
    }
}

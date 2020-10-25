using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI Manager class
// - Handles all of the user input.
// - Draws the UI to the screen (switching scenes?)

public class UIManager {
    //  Manager Variables
    TowerManager tm;
    EnemyManager em;

    // Canvas game object
    GameObject canvas;

    //  Spawn Variables
    private bool towerSpawnActive;
    private bool towerUpgradeActive;

    private TowerType towerSpawnType;
    private const float towerSpawnSize = 0.3f;

    //  Tower Variables
    private TowerMB towerSelected;

    //  UI Variables
    private GameObject buttons_NextWave;
    private GameObject buttons_Spawn;

    private GameObject buttons_UpgradeArcher;
    private GameObject buttons_UALvl2;
    private GameObject buttons_UALvl3a;
    private GameObject buttons_UALvl3b;
    private GameObject buttons_UALvl4a;
    private GameObject buttons_UALvl4b;

    private GameObject buttons_UpgradeMagic;
    private GameObject buttons_UMLvl2;
    private GameObject buttons_UMLvl3a;
    private GameObject buttons_UMLvl3b;
    private GameObject buttons_UMLvl4a;
    private GameObject buttons_UMLvl4b;

    private GameObject buttons_UpgradeSiege;
    private GameObject buttons_USLvl2;
    private GameObject buttons_USLvl3a;
    private GameObject buttons_USLvl3b;
    private GameObject buttons_USLvl4a;
    private GameObject buttons_USLvl4b;

    private GameObject towerIcon;

    // Variables for the bottom ticker
    private List<GameObject> tickerPrefabs;
    private List<GameObject> spawnedTickers;
    private EnemyType previousWave;
    private Vector3 redBarLoc;
    private Vector3 boxLength;
    private Vector3 speed;

    public UIManager() {
        //  Part - Manager Setup
        tm = GameManager.instance.towerManager;
        em = GameManager.instance.enemyManager;
        canvas = GameObject.Find("Canvas");

        //  Part - Spawn Setup
        towerSpawnActive = false;

        //  Part - UI Setup
        buttons_NextWave = GameObject.Find("Button_NextWave");
        buttons_NextWave.GetComponent<Button>().onClick.AddListener(() => { em.SpawnNextWave(); });

        //  >> SubPart - Spawn Button Setup
        buttons_Spawn = GameObject.Find("Buttons_Spawn");

        buttons_Spawn.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { UISpawnTower(0); });   //  Archer Spawn Button
        buttons_Spawn.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { UISpawnTower(1); });   //  Magic Spawn Button
        buttons_Spawn.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { UISpawnTower(2); });   //  Siege Spawn Button
        buttons_Spawn.SetActive(true);

        //  >> SubPart - Archer Button Setup
        buttons_UpgradeArcher = GameObject.Find("Buttons_UpgradeArcher");

        buttons_UALvl2 = buttons_UpgradeArcher.transform.GetChild(0).gameObject;
        buttons_UALvl2.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_2); ResetIconDisplay();  });

        buttons_UALvl3a = buttons_UpgradeArcher.transform.GetChild(1).gameObject;
        buttons_UALvl3a.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_3a); ResetIconDisplay(); });

        buttons_UALvl3b = buttons_UpgradeArcher.transform.GetChild(2).gameObject;
        buttons_UALvl3b.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_3b); ResetIconDisplay(); });

        buttons_UALvl4a = buttons_UpgradeArcher.transform.GetChild(3).gameObject;
        buttons_UALvl4a.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_4a); ResetIconDisplay(); });

        buttons_UALvl4b = buttons_UpgradeArcher.transform.GetChild(4).gameObject;
        buttons_UALvl4b.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_4b); ResetIconDisplay(); });

        buttons_UpgradeArcher.SetActive(false);

        //  >> SubPart - Magic Button Setup
        buttons_UpgradeMagic = GameObject.Find("Buttons_UpgradeMagic");

        buttons_UMLvl2 = buttons_UpgradeMagic.transform.GetChild(0).gameObject;
        buttons_UMLvl2.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_2); ResetIconDisplay(); });

        buttons_UMLvl3a = buttons_UpgradeMagic.transform.GetChild(1).gameObject;
        buttons_UMLvl3a.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_3a); ResetIconDisplay(); });

        buttons_UMLvl3b = buttons_UpgradeMagic.transform.GetChild(2).gameObject;
        buttons_UMLvl3b.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_3b); ResetIconDisplay(); });

        buttons_UMLvl4a = buttons_UpgradeMagic.transform.GetChild(3).gameObject;
        buttons_UMLvl4a.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_4a); ResetIconDisplay(); });

        buttons_UMLvl4b = buttons_UpgradeMagic.transform.GetChild(4).gameObject;
        buttons_UMLvl4b.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_4b); ResetIconDisplay(); });

        buttons_UpgradeMagic.SetActive(false);

        //  >> SubPart - Siege Button Setup
        buttons_UpgradeSiege = GameObject.Find("Buttons_UpgradeSiege");

        buttons_USLvl2 = buttons_UpgradeSiege.transform.GetChild(0).gameObject;
        buttons_USLvl2.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_2); ResetIconDisplay(); });

        buttons_USLvl3a = buttons_UpgradeSiege.transform.GetChild(1).gameObject;
        buttons_USLvl3a.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_3a); ResetIconDisplay(); });

        buttons_USLvl3b = buttons_UpgradeSiege.transform.GetChild(2).gameObject;
        buttons_USLvl3b.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_3b); ResetIconDisplay(); });

        buttons_USLvl4a = buttons_UpgradeSiege.transform.GetChild(3).gameObject;
        buttons_USLvl4a.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_4a); ResetIconDisplay(); });

        buttons_USLvl4b = buttons_UpgradeSiege.transform.GetChild(4).gameObject;
        buttons_USLvl4b.GetComponent<Button>().onClick.AddListener(() => { towerSelected.towerRef.UpgradeTower_Main(TowerLevel.Level_4b); ResetIconDisplay(); });

        buttons_UpgradeSiege.SetActive(false);

        //  >> SubPart - Tower Icon Setup
        towerIcon = GameObject.Find("Tower_Icon");
        towerIcon.SetActive(false);

        // Get references to the ticker prefabs.
        tickerPrefabs = new List<GameObject>();
        tickerPrefabs.Add(Resources.Load<GameObject>("Prefabs/Ticker_Normal"));
        tickerPrefabs.Add(Resources.Load<GameObject>("Prefabs/Ticker_Fast"));
        tickerPrefabs.Add(Resources.Load<GameObject>("Prefabs/Ticker_Tank"));
        tickerPrefabs.Add(Resources.Load<GameObject>("Prefabs/Ticker_Boss"));
        tickerPrefabs.Add(Resources.Load<GameObject>("Prefabs/Ticker_Blank"));

        // Set the default positions.
        //redBarLoc = new Vector3(-364.0f, -475.0f, 0.0f);
        //boxLength = new Vector3(195.0f, 0.0f, 0.0f);
        redBarLoc = new Vector3(-3.38f, -4.4f, 0.0f);
        boxLength = new Vector3(1.75f, 0.0f, 0.0f);
        speed = boxLength / em.totalWaveTime;

        // Spawn tickers based on the first few waves.
        spawnedTickers = new List<GameObject>();
        spawnedTickers.Add(GameObject.Instantiate(tickerPrefabs[4], redBarLoc - 2 * boxLength, Quaternion.identity, canvas.transform));
        spawnedTickers.Add(GameObject.Instantiate(tickerPrefabs[4], redBarLoc - 1 * boxLength, Quaternion.identity, canvas.transform));
        for (int i = 0; i < em.upcomingWaves.Count; i++)
        {
            spawnedTickers.Add(GameObject.Instantiate(tickerPrefabs[(int)em.upcomingWaves[i].enemyTypeToSpawn], 
                               redBarLoc + i * boxLength, 
                               Quaternion.identity, 
                               canvas.transform));
        }
    }
    
    public void Update(float dt) {
        //  Part - Tower Icon Control
        if (towerSpawnActive == true) {
            TowerIconControl();
        }

        //  Part - Left Click
        if (Input.GetMouseButtonDown(0)) {
            bool mouseOnMap = Camera.main.ScreenToWorldPoint(Input.mousePosition).x > -5 && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 5;

            //  SubPart - Spawn Tower (If towerSpawnActive is true and not selecting path or tower)
            if (towerSpawnActive == true && tm.SpawnCheck(towerSpawnSize) == true) {
                tm.SpawnTower(towerSpawnType);

                towerSpawnActive = false;
                towerIcon.SetActive(false);
            }

            //  SubPart - Upgrade Tower (If towerSpawnActive is false and selecting a tower)
            else if (towerSpawnActive == false && towerUpgradeActive == false && tm.SpawnTowerCheck(towerSpawnSize) == false) {
                towerUpgradeActive = true;

                UIUpgradeTower();
            }

            //  SubPart - Reset Icons (If upgrades displayed and clicking elsewhere)
            else if (mouseOnMap == true && towerSpawnActive == false && towerUpgradeActive == true && tm.SpawnTowerCheck(towerSpawnSize) == true) {
                ResetIconDisplay();
            }
        }


        // Part - Bottom Ticker
        if(em.hasGameStarted == false)
            return;
        for(int i = 0; i < spawnedTickers.Count; i++)
        {
            // Move the ticker to the left.
            spawnedTickers[i].transform.position -= speed * dt;
        }


    }

    // Function that gets called from enemy manager when a wave is spawned.
    public void OnWaveSpawn()
    {
        // Skip the first instance of this function call.
        if (em.hasGameStarted == true)
        { 
            // Shift the third box in list to the ticker mark, move all boxes.
            for (int i = 0; i < spawnedTickers.Count; i++)
            {
                spawnedTickers[i].transform.localPosition = canvas.transform.worldToLocalMatrix * (redBarLoc + (i - 3) * boxLength);
            }

            // Remove the first box in list.
            GameObject.Destroy(spawnedTickers[0]);
            spawnedTickers.RemoveAt(0);
        }


        // Add newest wave to end of list.
        spawnedTickers.Add(GameObject.Instantiate(tickerPrefabs[(int) em.upcomingWaves[em.upcomingWaves.Count - 1].enemyTypeToSpawn],
                   redBarLoc + (spawnedTickers.Count - 2) * boxLength,
                   Quaternion.identity,
                   canvas.transform));
    }



    #region Tower Functions
    private void TowerIconControl() {
        towerIcon.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1);
        towerIcon.GetComponent<SpriteRenderer>().color = tm.SpawnCheck(towerSpawnSize) ? Color.green : Color.red;
    }

    private void UISpawnTower(int pType) {
        if (pType == 0) {
            towerSpawnType = TowerType.Type_Archer;
        }

        else if (pType == 1) {
            towerSpawnType = TowerType.Type_Siege;
        }

        else if (pType == 2) {
            towerSpawnType = TowerType.Type_Magic;
        }

        towerSpawnActive = true;
        towerIcon.SetActive(true);
    }

    private void UIUpgradeTower() {
        //  Part - Select Tower
        foreach(TowerMB tower in tm.TowerList) {
            if (Vector2.Distance(tower.towerRef.towerActual.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < towerSpawnSize) {
                towerSelected = tower;
                break;
            }
        }

        if (towerSelected.towerRef.towerLevel != TowerLevel.Level_4a && towerSelected.towerRef.towerLevel != TowerLevel.Level_4b) {
            DisplayUpgrades(towerSelected.towerRef.TowerType, towerSelected.towerRef.TowerLevel);
        }
    }

    private void DisplayUpgrades(TowerType pType, TowerLevel pLevel) {
        buttons_Spawn.SetActive(false);

        switch (pType) {
            //  Part - Archer Upgrades
            case TowerType.Type_Archer:
                buttons_UpgradeArcher.SetActive(true);

                switch (pLevel) {
                    //  SubPart - Archer Level 1 (Display Level 2)
                    case TowerLevel.Level_1:
                        buttons_UALvl2.SetActive(true);
                        buttons_UALvl3a.SetActive(false);
                        buttons_UALvl3b.SetActive(false);
                        buttons_UALvl4a.SetActive(false);
                        buttons_UALvl4b.SetActive(false);
                        break;

                    //  SubPart - Archer Level 2 (Display Level 3a and 3b)
                    case TowerLevel.Level_2:
                        buttons_UALvl2.SetActive(false);
                        buttons_UALvl3a.SetActive(true);
                        buttons_UALvl3b.SetActive(true);
                        buttons_UALvl4a.SetActive(false);
                        buttons_UALvl4b.SetActive(false);
                        break;

                    //  SubPart - Archer Level 3a (Display Level 4a)
                    case TowerLevel.Level_3a:
                        buttons_UALvl2.SetActive(false);
                        buttons_UALvl3a.SetActive(false);
                        buttons_UALvl3b.SetActive(false);
                        buttons_UALvl4a.SetActive(true);
                        buttons_UALvl4b.SetActive(false);
                        break;

                    //  SubPart - Archer Level 3b (Display Level 4b)
                    case TowerLevel.Level_3b:
                        buttons_UALvl2.SetActive(false);
                        buttons_UALvl3a.SetActive(false);
                        buttons_UALvl3b.SetActive(false);
                        buttons_UALvl4a.SetActive(false);
                        buttons_UALvl4b.SetActive(true);
                        break;

                }
                break;

            //  Part - Magic Upgrades
            case TowerType.Type_Magic:
                buttons_UpgradeMagic.SetActive(true);

                switch (pLevel) {
                    //  SubPart - Magic Level 1 (Display Level 2)
                    case TowerLevel.Level_1:
                        buttons_UMLvl2.SetActive(true);
                        buttons_UMLvl3a.SetActive(false);
                        buttons_UMLvl3b.SetActive(false);
                        buttons_UMLvl4a.SetActive(false);
                        buttons_UMLvl4b.SetActive(false);
                        break;

                    //  SubPart - Magic Level 2 (Display Level 3a and 3b)
                    case TowerLevel.Level_2:
                        buttons_UMLvl2.SetActive(false);
                        buttons_UMLvl3a.SetActive(true);
                        buttons_UMLvl3b.SetActive(true);
                        buttons_UMLvl4a.SetActive(false);
                        buttons_UMLvl4b.SetActive(false);
                        break;

                    //  SubPart - Magic Level 3a (Display Level 4a)
                    case TowerLevel.Level_3a:
                        buttons_UMLvl2.SetActive(false);
                        buttons_UMLvl3a.SetActive(false);
                        buttons_UMLvl3b.SetActive(false);
                        buttons_UMLvl4a.SetActive(true);
                        buttons_UMLvl4b.SetActive(false);
                        break;

                    //  SubPart - Magic Level 3b (Display Level 4b)
                    case TowerLevel.Level_3b:
                        buttons_UMLvl2.SetActive(false);
                        buttons_UMLvl3a.SetActive(false);
                        buttons_UMLvl3b.SetActive(false);
                        buttons_UMLvl4a.SetActive(false);
                        buttons_UMLvl4b.SetActive(true);
                        break;

                }
                break;

            //  Part - Siege Upgrades
            case TowerType.Type_Siege:
                buttons_UpgradeSiege.SetActive(true);

                switch (pLevel) {
                    //  SubPart - Siege Level 1 (Display Level 2)
                    case TowerLevel.Level_1:
                        buttons_USLvl2.SetActive(true);
                        buttons_USLvl3a.SetActive(false);
                        buttons_USLvl3b.SetActive(false);
                        buttons_USLvl4a.SetActive(false);
                        buttons_USLvl4b.SetActive(false);
                        break;

                    //  SubPart - Siege Level 2 (Display Level 3a and 3b)
                    case TowerLevel.Level_2:
                        buttons_USLvl2.SetActive(false);
                        buttons_USLvl3a.SetActive(true);
                        buttons_USLvl3b.SetActive(true);
                        buttons_USLvl4a.SetActive(false);
                        buttons_USLvl4b.SetActive(false);
                        break;

                    //  SubPart - Siege Level 3a (Display Level 4a)
                    case TowerLevel.Level_3a:
                        buttons_USLvl2.SetActive(false);
                        buttons_USLvl3a.SetActive(false);
                        buttons_USLvl3b.SetActive(false);
                        buttons_USLvl4a.SetActive(true);
                        buttons_USLvl4b.SetActive(false);
                        break;

                    //  SubPart - Siege Level 3b (Display Level 4b)
                    case TowerLevel.Level_3b:
                        buttons_USLvl2.SetActive(false);
                        buttons_USLvl3a.SetActive(false);
                        buttons_USLvl3b.SetActive(false);
                        buttons_USLvl4a.SetActive(false);
                        buttons_USLvl4b.SetActive(true);
                        break;

                }
                break;
        }
    }

    private void ResetIconDisplay() {
        towerSelected = null;
        towerSpawnActive = false;
        towerUpgradeActive = false;

        buttons_Spawn.SetActive(true);
        buttons_UpgradeArcher.SetActive(false);
        buttons_UpgradeMagic.SetActive(false);
        buttons_UpgradeSiege.SetActive(false);
    }
    #endregion
}
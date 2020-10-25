using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager {

    //  _Unity Variables
    private GameObject objTowerPrefab;
    private GameObject objTowerSpawn;

    private GameObject levelMap;

    //  Audio Variables
    public AudioClip sound_Archer;
    public AudioClip sound_Magic;
    public AudioClip sound_Siege;

    //  Image Variables
    public Sprite towerBase_Level1;
    public Sprite towerBase_Level2;
    public Sprite towerBase_Level3;
    //public Sprite towerBase_Level4;

    public Sprite towerUpper_Archer;
    public Sprite towerUpper_Magic;
    public Sprite towerUpper_Siege;

    //  Tower Variables
    private List<TowerMB> towerList;
    public List<TowerMB> TowerList => towerList;

    public int manaCurr;

    public int costArcher1 = 100;
    public int costArcher2 = 200;
    public int costArcher3a = 350;
    public int costArcher3b = 400;
    public int costArcher4a = 500;
    public int costArcher4b = 600;

    public int costMagic1 = 100;
    public int costMagic2 = 200;
    public int costMagic3a = 350;
    public int costMagic3b = 400;
    public int costMagic4a = 500;
    public int costMagic4b = 600;

    public int costSiege1 = 100;
    public int costSiege2 = 200;
    public int costSiege3a = 350;
    public int costSiege3b = 400;
    public int costSiege4a = 500;
    public int costSiege4b = 600;

    public TowerManager() {
        //  Part - _Unity Setup
        objTowerPrefab = Resources.Load<GameObject>("Prefabs/Tower");
        objTowerSpawn = GameObject.Find("Tower_Spawn");

        levelMap = GameObject.Find("Level Map");

        //  Part - Audio Setup
        sound_Archer = Resources.Load<AudioClip>("Audio/Sound_Archer");
        sound_Magic = Resources.Load<AudioClip>("Audio/Sound_Magic");
        sound_Siege = Resources.Load<AudioClip>("Audio/Sound_Siege");

        //  Part - Image Setup
        towerBase_Level1 = Resources.Load<Sprite>("Sprites/TowerBase_Level1");
        towerBase_Level2 = Resources.Load<Sprite>("Sprites/TowerBase_Level2");
        towerBase_Level3 = Resources.Load<Sprite>("Sprites/TowerBase_Level3");
        //towerBase_Level4 = Resources.Load<Sprite>("Sprites/TowerBase_Level4");

        towerUpper_Archer = Resources.Load<Sprite>("Sprites/TowerUpper_Archer");
        towerUpper_Magic = Resources.Load<Sprite>("Sprites/TowerUpper_Magic");
        towerUpper_Siege = Resources.Load<Sprite>("Sprites/TowerUpper_Siege");

        //  Part - Tower Setup
        towerList = new List<TowerMB>();

        manaCurr = 500;
    }

    public void Update() {
        foreach(TowerMB tower in towerList) {
            tower.towerRef.Update();
        }
    }

    public bool SpawnCheck(float pSize) {
        return SpawnLevelCheck() && SpawnTowerCheck(pSize) && SpawnPathCheck(pSize);
    }

    public bool SpawnLevelCheck() {
        Vector3 mousePos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, levelMap.transform.position.z);
        return levelMap.GetComponent<Renderer>().bounds.Contains(mousePos);
    }

    public bool SpawnTowerCheck(float pSize) {
        foreach(TowerMB tower in towerList) {
            if (Vector2.Distance(tower.towerRef.towerActual.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < pSize) {
                return false;
            }
        }

        return true;
    }

    private bool SpawnPathCheck(float pSize) {
        List<Vector3> pathPoints = GameManager.instance.levelManager.currentLevel.worldPath;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < pathPoints.Count - 1; i++) {
            //  Part - Horizontal Section
            if (pathPoints[i].y == pathPoints[i + 1].y) {
                Rect horzPath = Rect.zero;

                //  SubPart - Right Path
                if (pathPoints[i].x < pathPoints[i + 1].x) {
                    horzPath = new Rect(pathPoints[i].x, pathPoints[i].y - (pSize / 2), Math.Abs(pathPoints[i].x - pathPoints[i + 1].x) + pSize, 3 * (pSize / 2));
                }

                //  SubPart - Left Path
                else if (pathPoints[i].x > pathPoints[i + 1].x) {
                    horzPath = new Rect(pathPoints[i + 1].x, pathPoints[i + 1].y - (pSize / 2), Math.Abs(pathPoints[i].x - pathPoints[i + 1].x) + pSize, 3 * (pSize / 2));
                }

                if (horzPath.Contains(mousePos)) {
                    return false;
                }
            }

            //  Part - Vertical Section
            else if (pathPoints[i].x == pathPoints[i + 1].x) {
                Rect vertPath = Rect.zero;

                //  SubPart - Bottom Path
                if (pathPoints[i].y < pathPoints[i + 1].y) {
                    vertPath = new Rect(pathPoints[i].x - (pSize / 2), pathPoints[i].y, 3 * (pSize / 2), Math.Abs(pathPoints[i].y - pathPoints[i + 1].y) + pSize);
                }

                //  SubPart - Top Path
                else if (pathPoints[i].y > pathPoints[i + 1].y) {
                    vertPath = new Rect(pathPoints[i + 1].x - (pSize / 2), pathPoints[i + 1].y, 3 * (pSize / 2), Math.Abs(pathPoints[i].y - pathPoints[i + 1].y) + pSize);
                }

                if (vertPath.Contains(mousePos)) {
                    return false;
                }
            }
        }

        return true;
    }

    public void SpawnTower(TowerType pType) {
        //  Part - Instantiate Tower
        Vector3 spawnPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1);
        GameObject towerObj = GameObject.Instantiate(objTowerPrefab, spawnPos, Quaternion.identity, objTowerSpawn.transform);

        towerObj.GetComponent<TowerMB>().towerRef = new Tower();

        //  Part - Setup Tower
        towerObj.GetComponent<TowerMB>().towerRef.SetupTower(towerObj, pType);
        towerObj.GetComponent<TowerMB>().towerRef.UpgradeTower_Main(TowerLevel.Level_1, this);
        towerObj.GetComponent<TowerMB>().towerRef.InitializeTower();

        //  Part - Add Tower to towerList
        towerList.Add(towerObj.GetComponent<TowerMB>());
    }
}
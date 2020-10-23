using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {

    //  _Unity Variables
    private GameObject objTowerPrefab;
    private GameObject objTowerSpawn;

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

    public TowerManager() {
        //  Part - _Unity Setup
        objTowerPrefab = Resources.Load<GameObject>("Prefabs/Tower");
        objTowerSpawn = GameObject.Find("Tower_Spawn");

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
    }

    public void Update() {
        foreach(TowerMB tower in towerList) {
            tower.towerRef.Update();
        }
    }

    public bool SpawnCheck(float pSize) {
        return SpawnTowerCheck(pSize) && SpawnPathCheck();
    }

    public bool SpawnTowerCheck(float pSize) {
        foreach(TowerMB tower in towerList) {
            if (Vector2.Distance(tower.towerRef.towerActual.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < pSize) {
                return false;
            }
        }

        return true;
    }

    private bool SpawnPathCheck() {
        return true;
    }

    public void SpawnTower(TowerType pType) {
        //  Part - Instantiate Tower
        Vector3 spawnPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, -1);
        GameObject towerObj = GameObject.Instantiate(objTowerPrefab, spawnPos, Quaternion.identity, objTowerSpawn.transform);

        towerObj.GetComponent<TowerMB>().towerRef = new Tower();

        //  Part - Setup Tower
        towerObj.GetComponent<TowerMB>().towerRef.SetupTower(towerObj, pType);
        towerObj.GetComponent<TowerMB>().towerRef.UpgradeTower_Main(TowerLevel.Level_1);
        towerObj.GetComponent<TowerMB>().towerRef.InitializeTower();

        //  Part - Add Tower to towerList
        towerList.Add(towerObj.GetComponent<TowerMB>());
    }
}
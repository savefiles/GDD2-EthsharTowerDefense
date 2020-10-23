using System.Collections.Generic;
using UnityEngine;

public enum TowerType {
    Type_Archer,
    Type_Magic,
    Type_Siege
}

public enum TowerLevel {
    Level_1,
    Level_2,
    Level_3a,
    Level_3b,
    Level_4a,
    Level_4b
}

public enum AttackType {
    Type_Hitscan,
    Type_Projectile
}

public class Tower {
    //  Search and select target
    //  Fire on target

    //  ~Tower Variables
    public GameObject towerActual;

    public TowerType towerType;            //  Type of Tower (Archer, Magic, Siege)
    public TowerType TowerType => towerType;

    public TowerLevel towerLevel;          //  Level of Tower (1, 2, 3a, 3b, 4a, 4b)
    public TowerLevel TowerLevel => towerLevel;

    //  Combat Variables
    private float towerRange;
    private float towerImpactRange = 0.2f;

    private int towerFireRate;              //  Shots per minute of tower
    private float towerCoolBase;            //  Reset value of cooldown
    private float towerCoolCurr;            //  Current value of cooldown

    private AttackType towerAttack;         //  Type of Attack (Hitscan, Projectile)
    private int towerDamage;                //  Amount of Damage

    //  Enemy Variables
    private List<Transform> enemyList;      //  Grabs all enemies in range
    private Transform towerTarget;          //  Current tower target

    public void SetupTower(GameObject pTower, TowerType pType) {
        towerActual = pTower;

        switch (pType) {
            //  Part - Archer Tower Setup
            case TowerType.Type_Archer:
                towerType = TowerType.Type_Archer;
                towerAttack = AttackType.Type_Hitscan;
                break;

            //  Part - Magisc Tower Setup
            case TowerType.Type_Magic:
                towerType = TowerType.Type_Magic;
                towerAttack = AttackType.Type_Hitscan;
                break;

            //  Part - Archer Tower Setup
            case TowerType.Type_Siege:
                towerType = TowerType.Type_Siege;
                towerAttack = AttackType.Type_Projectile;
                break;
        }

        //  Part - Enemy Setup
        enemyList = new List<Transform>();
    }

    public void InitializeTower() {
        //  Part - Tower Base
        switch (towerLevel) {
            case TowerLevel.Level_1:
                towerActual.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.towerManager.towerBase_Level1;
                break;

            case TowerLevel.Level_2:
                towerActual.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.towerManager.towerBase_Level2;
                break;

            case TowerLevel.Level_3a:
            case TowerLevel.Level_3b:
                towerActual.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.towerManager.towerBase_Level3;
                break;

            case TowerLevel.Level_4a:
            case TowerLevel.Level_4b:
                //transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.towerManager.towerBase_Level4;
                break;
        }

        //  Part - Tower Upper
        switch (towerType) {
            case TowerType.Type_Archer:
                towerActual.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.towerManager.towerUpper_Archer;
                break;

            case TowerType.Type_Magic:
                towerActual.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.towerManager.towerUpper_Magic;
                break;

            case TowerType.Type_Siege:
                towerActual.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.instance.towerManager.towerUpper_Siege;
                break;
        }
    }

    public void Update() {
        TowerTarget();

        if (towerCoolCurr <= 0) {
            if (towerTarget != null) {
                TowerAttack();
            }

            towerCoolCurr = towerCoolBase;
        }

        else {
            towerCoolCurr -= Time.deltaTime;
        }
    }

    //  SubMethod of Update - Tower Target
    private void TowerTarget() {
        //  Part - Reset Target List
        if (towerTarget == null) {
            enemyList.Clear();

            foreach (Enemy enemy in GameManager.instance.enemyManager.enemies) {
                if (Vector2.Distance(enemy.gameObject.transform.position, towerActual.transform.position) <= towerRange) {
                    enemyList.Add(enemy.gameObject.transform);
                }
            }

            //  Part - Target Enemy
            for (int i = 0; i < enemyList.Count; i++) {
                //  MinorPart - First Enemy
                if (i == 0) {
                    towerTarget = enemyList[i];
                }

                //  SubPart - Enemy Ahead (Further Waypoint)
                else if (enemyList[i].GetComponent<EnemyMB>().enemyRef.targetPositionIndex > towerTarget.GetComponent<EnemyMB>().enemyRef.targetPositionIndex) {
                    towerTarget = enemyList[i];
                }

                //  SubPart - Enemy Ahead (Same Waypoint)
                else if (enemyList[i].GetComponent<EnemyMB>().enemyRef.timeSinceLastPoint > towerTarget.GetComponent<EnemyMB>().enemyRef.timeSinceLastPoint) {
                    towerTarget = enemyList[i];
                }
            }
        }

        //  Part - Tower Rotate
        if (towerTarget != null) {
            Vector3 diff = towerTarget.position - towerActual.transform.position;

            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            towerActual.transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }

    //  SubMethod of Update - Tower Attack
    private void TowerAttack() {
        //  Part - Hitscan Tower (Archer, Magic)
        if (towerAttack == AttackType.Type_Hitscan) {
            towerTarget.GetComponent<EnemyMB>().enemyRef.TakeDamage(towerDamage);

            if (towerTarget.GetComponent<EnemyMB>().enemyRef.markedForDeletion == true) {
                towerTarget = null;
            }
        }

        //  Part - Projectile Tower (Siege)
        else if (towerAttack == AttackType.Type_Projectile) {
            Vector2 impactPos = towerTarget.transform.position;

            foreach (Enemy enemy in GameManager.instance.enemyManager.enemies) {
                if (Vector2.Distance(enemy.gameObject.transform.position, impactPos) <= towerImpactRange) {
                    enemy.gameObject.GetComponent<EnemyMB>().enemyRef.TakeDamage(towerDamage);

                    if (towerTarget.GetComponent<EnemyMB>().enemyRef.markedForDeletion == true) {
                        towerTarget = null;
                    }
                }
            }
        }
    }

    //  MainMethod - Upgrade Tower Main (param Tower Level)
    public void UpgradeTower_Main(TowerLevel pLevel) {
        towerLevel = pLevel;

        switch (towerType) {
            //  Part - Archer Tower Upgrade
            case TowerType.Type_Archer:
                switch (towerLevel) {
                    //  SubPart - Archer Tower Level 1
                    case TowerLevel.Level_1:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Archer Tower Level 2
                    case TowerLevel.Level_2:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Archer Tower Level 3a
                    case TowerLevel.Level_3a:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Archer Tower Level 3b
                    case TowerLevel.Level_3b:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Archer Tower Level 4a
                    case TowerLevel.Level_4a:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Archer Tower Level 4b
                    case TowerLevel.Level_4b:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;
                }
                break;

            //  Part - Magic Tower Upgrade
            case TowerType.Type_Magic:
                switch (towerLevel) {
                    //  SubPart - Magic Tower Level 1
                    case TowerLevel.Level_1:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Magic Tower Level 2
                    case TowerLevel.Level_2:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Magic Tower Level 3a
                    case TowerLevel.Level_3a:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Magic Tower Level 3b
                    case TowerLevel.Level_3b:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Magic Tower Level 4a
                    case TowerLevel.Level_4a:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Magic Tower Level 4b
                    case TowerLevel.Level_4b:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;
                }
                break;

            //  Part - Siege Tower Upgrade
            case TowerType.Type_Siege:
                switch (towerLevel) {
                    //  SubPart - Siege Tower Level 1
                    case TowerLevel.Level_1:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Siege Tower Level 2
                    case TowerLevel.Level_2:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Siege Tower Level 3a
                    case TowerLevel.Level_3a:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Siege Tower Level 3b
                    case TowerLevel.Level_3b:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Siege Tower Level 4a
                    case TowerLevel.Level_4a:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;

                    //  SubPart - Siege Tower Level 4b
                    case TowerLevel.Level_4b:
                        UpgradeTower_Sub(2f, 60, 10);
                        break;
                }
                break;
        }

        InitializeTower();
    }

    //  SubMethod of UpgradeTower_Main - Upgrade Tower Sub (param Fire Rate, Damage)
    private void UpgradeTower_Sub(float pRange, int pRate, int pDamage) {
        //  Part - Combat Setup
        towerRange = pRange;

        towerFireRate = pRate;
        towerCoolBase = 60 / towerFireRate;
        towerCoolCurr = 0;

        towerDamage = pDamage;
    }
}
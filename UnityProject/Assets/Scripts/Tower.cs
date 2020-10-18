using System.Collections.Generic;
using UnityEngine;

public enum AttackType {
    typeHitscan,
    typeProjectile,
}

public enum TowerType {
    archer,
    magic,
    siege
}

public enum TowerLevel {
    archer1,
    archer2,
    archer3a,
    archer3b,
    archer4a,
    archer4b,

    magic1,
    magic2,
    magic3a,
    magic3b,
    magic4a,
    magic4b,

    siege1,
    siege2,
    siege3a,
    siege3b,
    siege4a,
    siege4b,
}

public class Tower : MonoBehaviour {
    //  ~Tower Variables
    private TowerType towerType;
    private TowerLevel towerLevel;

    //  Combat Variables
    private int towerRange;
    private int towerRate;
    private AttackType towerAttack;
    private int towerDamage;

    private float towerCoolBase;
    private float towerCoolCurr;

    private Transform towerTarget;

    //  Creation Variables
    public int towerSize;

    void Start() {
    }

    //  Constructor

    //  MainMethod - Tower Creation
    //  Purpose : Sets up tower variables, used for creation and upgrades
    public void TowerCreation(string pTower) {
        switch (pTower) {
            //  Part - Archer Level 1 Tower
            case "archer":
                towerType = TowerType.archer;
                towerLevel = TowerLevel.archer1;

                TowerCreationSub(5, 5, AttackType.typeHitscan, 5);

                towerSize = 2;
                break;

            //  Part - Magic Level 1 Tower
            case "magic":
                towerType = TowerType.magic;
                towerLevel = TowerLevel.magic1;

                TowerCreationSub(5, 5, AttackType.typeHitscan, 5);

                towerSize = 2;
                break;

            //  Part - Siege Level 1 Tower
            case "siege":
                towerType = TowerType.siege;
                towerLevel = TowerLevel.siege1;

                TowerCreationSub(5, 5, AttackType.typeHitscan, 5);

                towerSize = 2;
                break;
        }
    }

    //  SubMethod of TowerCreation - Tower Creation Sub
    private void TowerCreationSub(int pRange, int pRate, AttackType pType, int pDamage) {
        towerRange = pRange;

        towerRate = pRate;
        towerCoolBase = 60 / towerRate;
        towerCoolCurr = towerCoolBase;

        towerAttack = pType;
        towerDamage = pDamage;
    }

    //  MainMethod - Update
    //  Purpose : Handles cooldown and calls TowerAttack
    void Update() {
        if (towerCoolCurr <= 0) {
            TowerAttack();
            towerCoolCurr = towerCoolBase;
        }

        else {
            towerCoolCurr -= Time.deltaTime;
        }
    }

    //  SubMethod of Update - Tower Attack
    //  Purpose : Targets enemy and attacks
    private void TowerAttack() {
        //  Part - Enemy Target
        if (Vector3.Distance(towerTarget.transform.position, transform.position) > towerRange) {
            //  SubPart - Reset Target List
            List<Transform> towerEnemies = new List<Transform>();
            foreach (Enemy enemy in GameManager.instance.enemyManager.enemies) {
                if (Vector3.Distance(enemy.gameObject.transform.position, transform.position) <= towerRange) {
                    towerEnemies.Add(enemy.gameObject.transform);
                }
            }

            //  SubPart - Target Enemy
            for (int i = 0; i < towerEnemies.Count; i++) {
                //  MinorPart - First Enemy
                if (i == 0) {
                    towerTarget = towerEnemies[i];
                }

                //  MinorPart - Enemy Ahead (Further Waypoint)
                else if (towerEnemies[i].GetComponent<Enemy>().targetPositionIndex > towerTarget.GetComponent<Enemy>().targetPositionIndex) {
                    towerTarget = towerEnemies[i];
                }

                //  MinorPart - Enemy Ahead (Same Waypoint)
                else if (towerEnemies[i].GetComponent<Enemy>().timeSinceLastPoint > towerTarget.GetComponent<Enemy>().timeSinceLastPoint) {
                    towerTarget = towerEnemies[i];
                }
            }
        }

        //  Part - Enemy Attack
        else {
            //  SubPart - Hitscan Tower (Archer, Magic)
            if (towerAttack == AttackType.typeHitscan) {
                towerTarget.GetComponent<Enemy>().TakeDamage(towerDamage);
            }

            //  SubPart - Projectile Tower (Siege)
            else if (towerAttack == AttackType.typeProjectile) {

            }
        }
    }

    //  MainMethod - Tower Upgrade
    public void TowerUpgrade(int pSide) {
        //  Part - Level 1 Tower
        if (towerLevel == TowerLevel.archer1 || towerLevel == TowerLevel.magic1 || towerLevel == TowerLevel.siege1) {
            switch (towerType) {
                //  SubPart - Archer Level 1
                case TowerType.archer:
                    towerLevel = TowerLevel.archer2;
                    ArcherUpgrade();
                    break;

                //  SubPart - Magic Level 1
                case TowerType.magic:
                    towerLevel = TowerLevel.magic2;
                    MagicUpgrade();
                    break;

                //  SubPart - Siege Level 1
                case TowerType.siege:
                    towerLevel = TowerLevel.siege2;
                    SiegeUpgrade();
                    break;
            }
        }

        else if (pSide == 1) {
            switch (towerType) {
                case TowerType.archer:
                    switch (towerLevel) {
                        //  SubPart - Archer Level 2
                        case TowerLevel.archer2:
                            towerLevel = TowerLevel.archer3a;
                            ArcherUpgrade();
                            break;

                        //  SubPart - Archer Level 3a
                        case TowerLevel.archer3a:
                            towerLevel = TowerLevel.archer4a;
                            ArcherUpgrade();
                            break;
                    }
                    break;

                case TowerType.magic:
                    switch (towerLevel) {
                        //  SubPart - Magic Level 2
                        case TowerLevel.magic2:
                            towerLevel = TowerLevel.magic3a;
                            MagicUpgrade();
                            break;

                        //  SubPart - Magic Level 3a
                        case TowerLevel.archer3a:
                            towerLevel = TowerLevel.magic4a;
                            MagicUpgrade();
                            break;
                    }
                    break;

                case TowerType.siege:
                    switch (towerLevel) {
                        //  SubPart - Siege Level 2
                        case TowerLevel.siege2:
                            towerLevel = TowerLevel.siege3a;
                            SiegeUpgrade();
                            break;

                        //  SubPart - Siege Level 3a
                        case TowerLevel.archer3a:
                            towerLevel = TowerLevel.siege4a;
                            SiegeUpgrade();
                            break;
                    }
                    break;
            }
        }

        else if (pSide == 2) {
            switch (towerType) {
                case TowerType.archer:
                    switch (towerLevel) {
                        //  SubPart - Archer Level 2
                        case TowerLevel.archer2:
                            towerLevel = TowerLevel.archer3b;
                            ArcherUpgrade();
                            break;

                        //  SubPart - Archer Level 3b
                        case TowerLevel.archer3b:
                            towerLevel = TowerLevel.archer4b;
                            ArcherUpgrade();
                            break;
                    }
                    break;

                case TowerType.magic:
                    switch (towerLevel) {
                        //  SubPart - Magic Level 2
                        case TowerLevel.magic2:
                            towerLevel = TowerLevel.magic3b;
                            MagicUpgrade();
                            break;

                        //  SubPart - Magic Level 3b
                        case TowerLevel.magic3b:
                            towerLevel = TowerLevel.magic4b;
                            MagicUpgrade();
                            break;
                    }
                    break;

                case TowerType.siege:
                    switch (towerLevel) {
                        //  SubPart - Siege Level 2
                        case TowerLevel.siege2:
                            towerLevel = TowerLevel.siege3b;
                            SiegeUpgrade();
                            break;

                        //  SubPart - Siege Level 3b
                        case TowerLevel.siege3b:
                            towerLevel = TowerLevel.siege4b;
                            SiegeUpgrade();
                            break;
                    }
                    break;
            }
        }
    }

    //  SubMethod of TowerUpgrade - Archer Upgrade
    private void ArcherUpgrade() {
        switch (towerLevel) {
            case TowerLevel.archer2:
                break;
            case TowerLevel.archer3a:
                break;
            case TowerLevel.archer3b:
                break;
            case TowerLevel.archer4a:
                break;
            case TowerLevel.archer4b:
                break;
        }
    }

    //  SubMethod of TowerUpgrade - Magic Upgrade
    private void MagicUpgrade() {
        switch (towerLevel) {
            case TowerLevel.magic2:
                break;
            case TowerLevel.magic3a:
                break;
            case TowerLevel.magic3b:
                break;
            case TowerLevel.magic4a:
                break;
            case TowerLevel.magic4b:
                break;
        }
    }

    //  SubMethod of TowerUpgrade - Siege Upgrade
    private void SiegeUpgrade() {
        switch (towerLevel) {
            case TowerLevel.siege2:
                break;
            case TowerLevel.siege3a:
                break;
            case TowerLevel.siege3b:
                break;
            case TowerLevel.siege4a:
                break;
            case TowerLevel.siege4b:
                break;
        }
    }
}

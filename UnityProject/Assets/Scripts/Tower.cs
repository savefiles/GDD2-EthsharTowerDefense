using System.Collections.Generic;
using UnityEngine;

public enum AttackType {
    typeHitscan,
    typeProjectile,
}

public enum TowerType {
    archer1,
}

public class Tower : MonoBehaviour {
    //  ~Tower Variables
    private TowerType towerType;

    //  Combat Variables
    private int towerRange;
    private int towerRate;
    private AttackType towerAttack;
    private int towerDamage;

    private float towerCoolBase;
    private float towerCoolCurr;

    private Enemy towerTarget;

    //  Creation Variables
    public int towerSize;

    void Start() {
    }

    //  Constructor

    //  MainMethod - Tower Creation
    //  Purpose : Sets up tower variables, used for creation and upgrades
    public void TowerCreation(string pType) {
        switch (pType) {
            //  Part - Archer Level 1 Tower
            case "archer1":
                towerType = TowerType.archer1;

                //  SubPart - Combat Setup
                towerRange = 5;
                towerRate = 5;
                towerAttack = AttackType.typeHitscan;
                towerDamage = 5;

                towerCoolBase = 60 / 5;
                towerCoolCurr = towerCoolBase;

                //  SubPart - Creation Setup
                towerSize = 2;
                break;
        }
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
        if (Vector3.Distance(towerTarget.gameObject.transform.position, transform.position) > towerRange) {
            //  SubPart - Reset Target List
            List<Enemy> towerEnemies = new List<Enemy>();

            foreach (Enemy enemy in GameManager.instance.enemyManager.enemies) {
                if (Vector3.Distance(enemy.gameObject.transform.position, transform.position) <= towerRange) {
                    towerEnemies.Add(enemy);
                }
            }

            //  SubPart - Target Enemy
            for (int i = 0; i < towerEnemies.Count; i++) {
                if (i == 0 || (towerEnemies[i].targetPositionIndex > towerTarget.targetPositionIndex) || ((towerEnemies[i].targetPositionIndex > towerTarget.targetPositionIndex) && towerEnemies[i].timeSinceLastPoint > towerTarget.timeSinceLastPoint)) {
                    towerTarget = towerEnemies[i];
                }
            }
        }

        else {
            if (towerAttack == AttackType.typeHitscan) {
                towerTarget.health -= towerDamage;
            }
        }
    }
}

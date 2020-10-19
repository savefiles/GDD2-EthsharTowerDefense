using System.Collections.Generic;
using UnityEngine;

public class TowerManager{
    //  Mana Variables
    private int manaCurr;
    public int ManaCurr => manaCurr;

    private Dictionary<string, int> towerCost;
    private Dictionary<string, int> towerSize;

    //  Tower Variables
    private List<Transform> towers;

    public Transform towerPrefab;

    private Sprite mapMask;

    public TowerManager() {
        //  Part - Mana Setup
        manaCurr = 0;

        towerCost = new Dictionary<string, int>();
        towerCost.Add("archer1", 250);  // Arbitrary Values

        towerSize = new Dictionary<string, int>();
        towerSize.Add("archer1", 5);  // Arbitrary Values

        //  Part - Tower Setup
        towers = new List<Transform>();
    }

    void Update() {
        //   Possible Mana Recovery
    }

    public void TowerCreation(Vector3 pPos, string pTower) {
        //  Part - Tower Location Check
        bool towerPos = true;
        int tempSize = towerSize[pTower];

        //  >> SubPart - Path Location Check
        towerPos = PathCheck(pPos, tempSize);

        //  >> SubPart - Other Towers Check
        if (towerPos == true) {
            foreach (Transform tower in towers) {
                if (Vector3.Distance(tower.transform.position, pPos) < (tower.GetComponent<Tower>().towerSize + tempSize)) {
                    towerPos = false;
                    break;
                }
            }
        }

        //  Part - Tower Mana Check
        bool tempCost = (manaCurr >= towerCost[pTower]);

        //  Part - Tower Creation
        if (towerPos == true && tempCost == true) {
            Transform tempTower = GameObject.Instantiate(towerPrefab, pPos, Quaternion.identity);
            tempTower.GetComponent<Tower>().TowerCreation(pTower);
            towers.Add(tempTower);
        }
    }

    public bool PathCheck(Vector3 pPos, int pSize) {
        List<Vector3> levelPath = GameManager.instance.levelManager.currentLevel.worldPath;

        bool tempBool = true;
        for (int i = 0; i < levelPath.Count - 1; i++) {
            //  Part - Horizontal Path
            if (levelPath[i].y == levelPath[i + 1].y) {
                if (Mathf.Abs(pPos.y - levelPath[i].y) < pSize + 2.5) {
                    tempBool = false;
                    break;
                }
            }

            //  Part - Vertical Path
            else if (levelPath[i].x == levelPath[i + 1].x) {
                if (Mathf.Abs(pPos.x - levelPath[i].x) < pSize + 2.5) {
                    tempBool = false;
                    break;
                }
            }
        }

        return tempBool;
    }
}

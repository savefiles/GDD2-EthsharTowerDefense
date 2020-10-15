using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    //  Mana Variables
    private int manaCurr;
    public int ManaCurr => manaCurr;

    private Dictionary<string, int> towerCost;
    private Dictionary<string, int> towerSize;

    //  Tower Variables
    private List<Transform> towers;

    public Transform towerPrefab;

    void Start() {
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
        // Part - Tower Location Check
        bool towerPos = true;
        int tempSize = towerSize[pTower];
        foreach (Transform tower in towers) {
            if (Vector3.Distance(tower.transform.position, pPos) < (tower.GetComponent<Tower>().towerSize + tempSize)) {
                towerPos = false;
                break;
            }
        }

        //  Part - Tower Mana Check
        bool tempCost = (manaCurr >= towerCost[pTower]);
        
        //  Part - Tower Creation
        if (towerPos == true && tempCost == true) {
            Transform tempTower = Instantiate(towerPrefab, pPos, Quaternion.identity, transform);
            tempTower.GetComponent<Tower>().TowerCreation(pTower);
            towers.Add(tempTower);
        }
    }
}

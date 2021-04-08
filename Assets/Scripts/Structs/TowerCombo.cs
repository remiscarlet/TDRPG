using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Structs {
    public struct TowerCombo {
        public TowerCombo(GameObject firstTower, GameObject secondTower) {
            towersIncluded = new List<GameObject>();
            AddTowerToCombo(firstTower);
            AddTowerToCombo(secondTower);
        }

        private List<GameObject> towersIncluded;
        public List<GameObject> GetTowers {
            get => towersIncluded;
        }

        public void AddTowerToCombo(GameObject tower) {
            towersIncluded.Add(tower);
        }
    }
}

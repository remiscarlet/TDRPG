using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void DrawIndicators() {
            List<GameObject> towers = GetTowers;
            GameObject firstTower = towers.First();
            GameObject secondTower = towers.Last();
            DrawIndicator(firstTower, secondTower);
            if (towers.Count > 2) {
                for (int i = 1; i < towers.Count; i++) {
                    secondTower = towers[i];
                    DrawIndicator(firstTower, secondTower);
                    firstTower = secondTower;
                }
            }
        }

        private void DrawIndicator(GameObject tower1, GameObject tower2) {
            TowerController tower1Controller = ReferenceManager.TowerManagerComponent.towerToController[tower1];
            TowerController tower2Controller = ReferenceManager.TowerManagerComponent.towerToController[tower2];
        }

        public void AddTowerToCombo(GameObject tower) {
            towersIncluded.Add(tower);
        }
    }
}

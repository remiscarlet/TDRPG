using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Structs {
    // Maybe a full class and not a struct :-/
    public struct TowerCombo {
        public TowerCombo(GameObject firstTower, GameObject secondTower) {
            towersIncluded = new List<GameObject>();
            AddTowerToCombo(firstTower);
            AddTowerToCombo(secondTower);

            DetermineComboType();
        }

        private void DetermineComboType() {
            // ComboManager?
        }

        public void ComboFixedUpdate() {
            // Generate projectiles for combo type
        }

        private List<GameObject> towersIncluded;
        public List<GameObject> GetTowers {
            get => towersIncluded;
        }

        /// <summary>
        /// Draw the *line* between each combo ring indicator.
        ///
        /// The rings themselves are drawn by the <c>TowerController</c> while this method only
        /// draws the lines that connect each combo ring.
        /// </summary>
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

        /// <summary>
        /// Draw rings and line
        /// </summary>
        /// <param name="tower1"></param>
        /// <param name="tower2"></param>
        private void DrawIndicator(GameObject tower1, GameObject tower2) {
            TowerController tower1Controller = ReferenceManager.TowerManagerComponent.towerToController[tower1];
            TowerController tower2Controller = ReferenceManager.TowerManagerComponent.towerToController[tower2];
        }

        /// <summary>
        /// Add an additional tower to this combo if valid.
        /// </summary>
        /// <param name="tower"></param>
        /// <returns>Bool representing if addition was successful</returns>
        public bool AddTowerToCombo(GameObject tower) {
            // TODO: Invalid additions should return false. Ie, need ComboManager.
            towersIncluded.Add(tower);

            return true;
        }
    }
}

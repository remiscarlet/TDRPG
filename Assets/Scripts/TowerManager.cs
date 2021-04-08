using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Structs;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    private List<GameObject> towers;
    private Dictionary<GameObject, TowerController> towerToController;
    private Dictionary<TowerController, GameObject> controllerToTower;
    private Dictionary<GameObject, TowerCombo> towerToCombo;

    private List<TowerCombo> combos;
    private List<TowerController> towersBeingCombod; // Only ever be len 1?

    private GameObject towerPrefab;
    private PlayerState playerState;
    private void Start() {
        towersBeingCombod = new List<TowerController>();
        towerPrefab = ReferenceManager.Prefabs.Tower;
        playerState = ReferenceManager.PlayerStateComponent;
        towers = new List<GameObject>();
        combos = new List<TowerCombo>();

        towerToCombo = new Dictionary<GameObject, TowerCombo>();
        towerToController = new Dictionary<GameObject, TowerController>();
        controllerToTower = new Dictionary<TowerController, GameObject>();
        // Auto search for premade towers?
    }

    public TowerCombo CreateCombo(GameObject tower1, GameObject tower2) {
        TowerCombo combo = new TowerCombo(tower1, tower2);
        combos.Add(combo);

        towerToCombo.Add(tower1, combo);
        towerToCombo.Add(tower2, combo);

        return combo;
    }

    public void CreateTower(Spell spell, Transform towerSlotTransform) {
        int towerPrice = spell.TowerPrice;
        print(playerState);
        print(towerPrice);
        if (!playerState.CanAffordPurchase(towerPrice)) {
            return;
        }

        playerState.DeductPoints(towerPrice);
        GameObject tower = Instantiate(towerPrefab, towerSlotTransform);
        tower.layer = Layers.Friendly;
        tower.GetComponent<TowerController>().SetAbility(spell);
        TowerController towerController = tower.GetComponent<TowerController>();

        towers.Add(tower);
        towerToController.Add(tower, towerController);
        controllerToTower.Add(towerController, tower);
    }

    public bool IsMidComboCreation() {
        return towersBeingCombod.Count > 0;
    }

    public void AddToTowersBeingCombod(TowerController tower) {
        towersBeingCombod.Add(tower);
        tower.IsBeingCombod = true;

        if (towersBeingCombod.Count > 1) {
            GameObject tower0 = controllerToTower[towersBeingCombod[0]];
            GameObject tower1 = controllerToTower[towersBeingCombod[1]];

            GameObject additionalTower;
            TowerCombo combo = CreateCombo(tower0, tower1);
            for (int i = 2; i < towersBeingCombod.Count; i++) {
                additionalTower = controllerToTower[towersBeingCombod[i]];
                combo.AddTowerToCombo(additionalTower);
            }
            print($"TowerCombo: {combo}");

            ClearTowersBeingCombod(false);
        }
    }

    public void ClearTowersBeingCombod(bool clearComboRing) {
        if (clearComboRing) {
            foreach (TowerController tower in towersBeingCombod) {
                tower.IsBeingCombod = false;
            }
        }

        towersBeingCombod.Clear();
    }

    public bool IsTowerInCombo(TowerController towerController) {
        GameObject tower = controllerToTower[towerController];
        return towerToCombo.ContainsKey(tower);
    }

    public bool IsTowerMidComboCreation(TowerController towerController) {
        return towersBeingCombod.Contains(towerController);
    }
}

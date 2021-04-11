using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Structs;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    private List<GameObject> towers;

    [System.NonSerialized]
    public Dictionary<GameObject, TowerController> towerToController;
    [System.NonSerialized]
    public Dictionary<TowerController, GameObject> controllerToTower;
    [System.NonSerialized]
    public Dictionary<GameObject, TowerCombo> towerToCombo;

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

    private void Update() {
        DrawComboGroupIndicators();
    }

    /// <summary>
    /// Handles all physics related updates for towers including combo towers.
    ///
    /// We do this to simplify differentiation of combo vs non-combo towers. The individual <c>TowerController</c>s
    /// don't update anything in <c>FixedUpdate()</c> as everything is handled from here.
    /// </summary>
    /// <exception cref="Exception">Reached an impossible state</exception>
    private void FixedUpdate() {
        // Update combo towers
        Dictionary<GameObject, bool> towersAlreadyUpdated = new Dictionary<GameObject, bool>();
        foreach (GameObject tower in towers) {
            towersAlreadyUpdated.Add(tower, false);
        }

        foreach (TowerCombo combo in combos) {
            combo.ComboFixedUpdate();
            List<GameObject> towersInCombo = combo.GetTowers;
            foreach (GameObject towerInCombo in towersInCombo) {
                if (!towersAlreadyUpdated.ContainsKey(towerInCombo)) {
                    throw new Exception(
                        $"Encountered tower in combo that was not known to TowerManager. This should be impossible. Got: `{towerInCombo}`");
                }
                towersAlreadyUpdated[towerInCombo] = true;
            }
        }

        // Update non-combo towers
        List<GameObject> towersNotInCombos = (
            from tup in towersAlreadyUpdated
            where !tup.Value
            select tup.Key
            ).ToList(); // :-/ This tuple unpacking is kinda gross

        foreach (GameObject tower in towersNotInCombos) {
            TowerController towerController = towerToController[tower];
            UpdateTower(towerController);
        }
    }

    /// <summary>
    /// Invoke the physics update funcs on <c>TowerController</c>
    /// </summary>
    /// <param name="controller"></param>
    private void UpdateTower(TowerController controller) {
        controller.AimAtClosestEnemyInRange();
        controller.ShootIfAimIsClose();
    }

    /// <summary>
    /// Draw the yellow combo indicator rings
    /// </summary>
    private void DrawComboGroupIndicators() {
        foreach (TowerCombo combo in combos) {
            combo.DrawIndicators();
        }
    }

    /// <summary>
    /// Given two towers, will return a <c>TowerCombo</c> with just these two towers.
    /// Subsequent towers for 2+ combos should be added using TowerCombo.AddTowerToCombo()
    /// </summary>
    /// <param name="tower1"></param>
    /// <param name="tower2"></param>
    /// <returns>The newly created <c>TowerCombo</c></returns>
    public TowerCombo CreateCombo(GameObject tower1, GameObject tower2) {
        TowerCombo combo = new TowerCombo(tower1, tower2);
        combos.Add(combo);

        towerToCombo.Add(tower1, combo);
        towerToCombo.Add(tower2, combo);

        return combo;
    }

    /// <summary>
    /// Given a transform and material, will recursively apply <c>spellMaterial</c> to any and
    /// all renderer components on transform and its children transforms.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="spellMaterial"></param>
    private void UpdateTowerColor(Transform transform, Material spellMaterial) {
        Renderer renderer = transform.GetComponent<Renderer>();
        if (renderer != null) {
            renderer.material = spellMaterial;
        }

        foreach (Transform child in transform) {
            UpdateTowerColor(child, spellMaterial);
        }
    }

    /// <summary>
    /// Attempts to create a new tower. Will return bool representing success.
    /// </summary>
    /// <param name="spell"></param>
    /// <param name="towerSlotTransform"></param>
    /// <returns>True if tower was created. False if something failed like not being able to afford the price/</returns>
    public bool CreateTower(Spell spell, Transform towerSlotTransform) {
        int towerPrice = spell.TowerPrice;
        print(playerState);
        print(towerPrice);
        if (!playerState.CanAffordPurchase(towerPrice)) {
            return false;
        }

        playerState.DeductPoints(towerPrice);
        GameObject tower = Instantiate(towerPrefab, towerSlotTransform);
        tower.layer = Layers.Friendly;

        Material spellMaterial = spell.InstancePrefab.GetComponent<Renderer>().sharedMaterial;
        UpdateTowerColor(tower.transform, spellMaterial);

        TowerController towerController = tower.GetComponent<TowerController>();
        towerController.SetAbility(spell);

        towers.Add(tower);
        towerToController.Add(tower, towerController);
        controllerToTower.Add(towerController, tower);

        return true;
    }

    /// <summary>
    /// Are we in the middle of creating a combo right now?
    /// </summary>
    /// <returns></returns>
    public bool IsMidComboCreation() {
        return towersBeingCombod.Count > 0;
    }

    /// <summary>
    /// Attempt to add a non-combo tower to a new combo.
    ///
    /// If the # of non-combo towers selected is >=2, will create a new combo automatically and clear mid-combo state.
    /// </summary>
    /// <param name="tower"></param>
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

    /// <summary>
    /// Clear out/"Cancel" our mid-combo creation.
    /// </summary>
    /// <param name="clearComboRing"></param>
    public void ClearTowersBeingCombod(bool clearComboRing) {
        if (clearComboRing) {
            foreach (TowerController tower in towersBeingCombod) {
                tower.IsBeingCombod = false;
            }
        }

        towersBeingCombod.Clear();
    }

    /// <summary>
    /// Is this <c>TowerController</c> a part of an already existing combo?
    /// </summary>
    /// <param name="towerController"></param>
    /// <returns></returns>
    public bool IsTowerInCombo(TowerController towerController) {
        GameObject tower = controllerToTower[towerController];
        return towerToCombo.ContainsKey(tower);
    }

    /// <summary>
    /// Is this <c>TowerController</c> currently selected as part of a new potential combo?
    /// </summary>
    /// <param name="towerController"></param>
    /// <returns></returns>
    public bool IsTowerMidComboCreation(TowerController towerController) {
        return towersBeingCombod.Contains(towerController);
    }

}

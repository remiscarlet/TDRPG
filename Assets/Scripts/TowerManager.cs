using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    private List<TowerController> towerControllers;

    [System.NonSerialized]
    public Dictionary<GameObject, TowerController> towerToController;
    [System.NonSerialized]
    public Dictionary<TowerController, GameObject> controllerToTower;
    [System.NonSerialized]
    public Dictionary<TowerController, ComboTowerController> controllerToCombo;

    private List<ComboTowerController> combos;
    private List<TowerController> towersBeingCombod; // Only ever be len 1?

    private ComboTypeManager comboTypeManager;
    private GameObject towerPrefab;
    private PlayerState playerState;
    private void Start() {
        towersBeingCombod = new List<TowerController>();
        comboTypeManager = ReferenceManager.ComboTypes;
        towerPrefab = ReferenceManager.Prefabs.Tower;
        playerState = ReferenceManager.PlayerStateComponent;
        towerControllers = new List<TowerController>();
        combos = new List<ComboTowerController>();

        controllerToCombo = new Dictionary<TowerController, ComboTowerController>();
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
        Dictionary<TowerController, bool> towerControllersAlreadyUpdated = new Dictionary<TowerController, bool>();
        foreach (TowerController towerController in towerControllers) {
            towerControllersAlreadyUpdated.Add(towerController, false);
        }

        foreach (ComboTowerController combo in combos) {
            combo.ComboFixedUpdate();
            List<TowerController> controllersInCombo = combo.GetTowerControllers;
            foreach (TowerController controllerInCombo in controllersInCombo) {
                if (!towerControllersAlreadyUpdated.ContainsKey(controllerInCombo)) {
                    throw new Exception(
                        $"Encountered tower in combo that was not known to TowerManager. This should be impossible. Got: `{controllerInCombo}`");
                }
                towerControllersAlreadyUpdated[controllerInCombo] = true;
            }
        }

        // Update non-combo towers
        List<TowerController> towersControllersNotInCombos = (
            from tup in towerControllersAlreadyUpdated
            where !tup.Value
            select tup.Key
            ).ToList(); // :-/ This tuple unpacking is kinda gross

        foreach (TowerController towerController in towersControllersNotInCombos) {
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
        foreach (ComboTowerController combo in combos) {
            combo.DrawIndicators();
        }
    }

    /// <summary>
    /// Given two towers, will return a <c>ComboTowerController</c> with just these two towers.
    /// Subsequent towers for 2+ combos should be added using ComboTowerController.AddTowerToCombo()
    /// </summary>
    /// <param name="towerController1"></param>
    /// <param name="towerController2"></param>
    /// <returns>The newly created <c>ComboTowerController</c></returns>
    [CanBeNull]
    private ComboTowerController CreateCombo(TowerController towerController1, TowerController towerController2) {
        ComboTowerController combo = new ComboTowerController(towerController1, towerController2);
        combos.Add(combo);

        controllerToCombo.Add(towerController1, combo);
        controllerToCombo.Add(towerController2, combo);

        return combo;
    }

    /// <summary>
    /// Given a transform and material, will recursively apply <c>spellMaterial</c> to any and
    /// all renderer components on <c>transform</c> and its children transforms.
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

        towerControllers.Add(towerController);
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
            TowerController towerController0 = towersBeingCombod[0];
            TowerController towerController1 = towersBeingCombod[1];

            TowerController additionalTower;
            ComboTowerController? combo = CreateCombo(towerController0, towerController1);
            if (combo == null) {
                // Invalid combo atm. Continue keeping things in "mid combo" state.
                // TODO: How to handle first two towers being valid but subsequent towers after 2nd tower making invalid?
                return;
            }

            for (int i = 2; i < towersBeingCombod.Count; i++) {
                additionalTower = towersBeingCombod[i];
                combo.AddTowerToCombo(additionalTower);
            }
            print($"ComboTowerController: {combo}");

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
        return controllerToCombo.ContainsKey(towerController);
    }

    /// <summary>
    /// Is this <c>GameObject</c> a part of an already existing combo?
    /// </summary>
    /// <param name="towerController"></param>
    /// <returns></returns>
    public bool IsTowerInCombo(GameObject tower) {
        TowerController towerController = towerToController[tower];
        return IsTowerInCombo(towerController);
    }

    /// <summary>
    /// Is this <c>TowerController</c> currently selected as part of a new combo?
    /// </summary>
    /// <param name="towerController"></param>
    /// <returns></returns>
    public bool IsTowerMidComboCreation(TowerController towerController) {
        return towersBeingCombod.Contains(towerController);
    }

}

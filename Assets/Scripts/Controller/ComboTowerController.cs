using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations;

public class ComboTowerController {
    private ComboTypeManager comboTypeManager;
    private ComboType comboType;

    public ComboTowerController(TowerController firstController, TowerController secondController) {
        comboTypeManager = ReferenceManager.ComboTypes;
        controllersIncluded = new List<TowerController>();
        AddTowerToCombo(firstController);
        AddTowerToCombo(secondController);

        DetermineComboType();
    }

    private void DetermineComboType() {
        // ComboManager?
        comboType = comboTypeManager.DetermineComboType(controllersIncluded);
    }

    public void ComboFixedUpdate() {
        // Generate projectiles for combo type
    }

    private List<TowerController> controllersIncluded;
    public List<TowerController> GetTowerControllers {
        get => controllersIncluded;
    }

    /// <summary>
    /// Draw the *line* between each combo ring indicator.
    ///
    /// The rings themselves are drawn by the <c>TowerController</c> while this method only
    /// draws the lines that connect each combo ring.
    /// </summary>
    public void DrawIndicators() {
        List<TowerController> towers = GetTowerControllers;
        TowerController firstTower = towers.First();
        TowerController secondTower = towers.Last();
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
    private void DrawIndicator(TowerController towerController1, TowerController towerController2) {
    }

    /// <summary>
    /// Add an additional tower to this combo if valid.
    /// </summary>
    /// <param name="tower"></param>
    /// <returns>Bool representing if addition was successful</returns>
    public bool AddTowerToCombo(TowerController towerController) {
        // TODO: Invalid additions should return false. Ie, need ComboManager.
        controllersIncluded.Add(towerController);

        return true;
    }
}

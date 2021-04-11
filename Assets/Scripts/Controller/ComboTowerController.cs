using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using JetBrains.Annotations;
using Unity.Jobs.LowLevel.Unsafe;

public class ComboTowerController : MonoBehaviour {
    private ComboTypeManager comboTypeManager;
    private ComboType comboType;
    private GameObject comboTowerTurret;
    private Collider comboTowerTurretCollider;

    private List<TowerController> controllersIncluded;
    public List<TowerController> GetTowerControllers {
        get => controllersIncluded;
    }

    private void Awake() {
        comboTypeManager = ReferenceManager.ComboTypes;
        controllersIncluded = new List<TowerController>();
        comboTowerTurret = transform.Find("Cone").gameObject;
        comboTowerTurretCollider = comboTowerTurret.GetComponent<Collider>();
    }

    public void AddFirstTwoTowers(TowerController firstController, TowerController secondController) {
        AddTowerToCombo(firstController);
        AddTowerToCombo(secondController);

        DetermineComboType();
    }

    private void DetermineComboType() {
        // ComboManager?
        comboType = comboTypeManager.DetermineComboType(controllersIncluded);
    }

    public void ComboFixedUpdate() {
        if (comboType == null) {
            throw new Exception("Called ComboFixedUpdate() before comboType was defined!");
        }
        print($"Running ComboFixedUpdate() on combo {comboType}");
        Shoot();
        // Generate projectiles for combo type
    }

    private float lastShotAt = 0.0f;
    private float GetTimeSinceLastShot() {
        return Time.time - lastShotAt;
    }

    private Vector3 projectilePosOffset = Vector3.zero; //new Vector3(-0.039f, -1.15f, 0.05f);
    private void Shoot() {
        print(":thinking:");
        print(comboType.GetWaitTimeBetweenShots());
        print(GetTimeSinceLastShot());
        if (comboType.GetWaitTimeBetweenShots() < GetTimeSinceLastShot()) {
            GameObject enemy = TargetingUtils.GetClosestEnemyInRange(transform, comboType.TowerShotRange);
            comboType.SpawnBaseProjectile(transform, transform.rotation, comboTowerTurretCollider);
            lastShotAt = Time.time;
        }
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

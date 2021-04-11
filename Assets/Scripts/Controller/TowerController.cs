#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.CodeEditor;
using UnityEngine;

public class TowerController : Interactable
{
    private float shootForce;
    public float ShootForce {
        get => shootForce;
        set => shootForce = value;
    }

    private float damagePerHit;
    public float DamagePerHit {
        get => damagePerHit;
        set => damagePerHit = value;
    }

    private int shotsPerMinute = 240;

    public int ShotsPerMinute {
        get => shotsPerMinute;
        set => shotsPerMinute = value;
    }

    private float towerRange;
    public float TowerRange {
        get => towerRange;
        set => towerRange = value;
    }

    private float maxUpwardAngleCorrection;
    public float MaxUpwardAngleCorrection {
        get => maxUpwardAngleCorrection;
        set => maxUpwardAngleCorrection = value;
    }

    private bool isBeingCombod;

    public bool IsBeingCombod {
        get => isBeingCombod;
        set => isBeingCombod = value;
    }

    private Spell towerSpell;

    public Spell TowerSpell {
        get => towerSpell;
    }

    public const float TurretSpinSpeed = 2.0f;
    public GameObject projectilePrefab;
    private SpawnManager spawnManager;
    private TowerManager towerManager;
    private GameObject towerObject;
    private GameObject towerHead;
    private Collider towerHeadCollider;
    private GameObject towerTurret;
    private GameObject comboRing;

    private void Start() {
        spawnManager = ReferenceManager.SpawnManagerComponent;
        towerManager = ReferenceManager.TowerManagerComponent;

        towerObject = gameObject;
        comboRing = transform.Find("ComboRing").gameObject;
        towerHead = transform.Find("TowerHead").gameObject;
        towerHeadCollider = towerHead.GetComponent<Collider>();
        towerTurret = towerHead.transform.Find("TowerTurret").gameObject;
    }

    public void SetAbility(Spell ability) {
        // Set ability here

        towerSpell = ability;
        projectilePrefab = ability.InstancePrefab;

        MaxUpwardAngleCorrection = ability.MaxUpwardAngleCorrection;
        ShootForce = ability.ShootForce * 1.5f;
        DamagePerHit = ability.DamagePerHit;
        ShotsPerMinute = ability.ShotsPerMinute;
        TowerRange = ability.TowerShotRange;
    }

    public override void Activate() {
        if (Input.GetKey(KeyCode.F) && !towerManager.IsTowerInCombo(this) && !towerManager.IsTowerMidComboCreation(this)) {
            towerManager.AddToTowersBeingCombod(this);
        }
    }

    public void AimAtClosestEnemyInRange() {
        GameObject? closestEnemy = TargetingUtils.GetClosestEnemyInRange(towerHead.transform, TowerRange);
        if (closestEnemy == null) {
            return;
        }

        float singleStep = TurretSpinSpeed * Time.deltaTime;
        Vector3 targetPos = TargetingUtils.GetTargetPosWithCompensation(towerHead.transform, closestEnemy, TowerRange, MaxUpwardAngleCorrection);
        //print($"targetPos: {targetPos}");
        targetPos = TargetingUtils.ApplySpellTowerTurretOffset(towerSpell.SpellTowerTurretOffset, targetPos,
            towerHead.transform.position, TowerRange);
        //print($"Offset targetPos: {targetPos}");
        Vector3 newDirection = Vector3.RotateTowards(towerHead.transform.forward, targetPos, singleStep, 0.0f);
        Debug.DrawRay(towerHead.transform.position, newDirection, Color.red);

        towerHead.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private float maxAngleDeltaToShootFrom = 3.0f;
    public void ShootIfAimIsClose() {
        GameObject? closestEnemy = TargetingUtils.GetClosestEnemyInRange(towerHead.transform, TowerRange);
        if (closestEnemy == null) {
            return;
        }

        Vector3 targetPos = TargetingUtils.GetTargetPosWithCompensation(towerHead.transform, closestEnemy, TowerRange, MaxUpwardAngleCorrection);
        targetPos = TargetingUtils.ApplySpellTowerTurretOffset(towerSpell.SpellTowerTurretOffset, targetPos,
            towerHead.transform.position, TowerRange);
        float angleToTarget = Vector3.Angle(towerHead.transform.forward, targetPos);
        if (angleToTarget < maxAngleDeltaToShootFrom) {
            Shoot(closestEnemy);
        }
    }

    private void Update() {
        UpdateComboRings();
    }

    private void FixedUpdate() {
        // Tower physics related updates (ie, aiming and shooting) are handled by TowerManager to account for combos
    }

    private void UpdateComboRings() {
        comboRing.SetActive(IsBeingCombod);
    }

    private float GetWaitTimeBetweenShots() {
        return 60.0f / ShotsPerMinute; // Seconds
    }

    private float lastShotAt = 0.0f;
    private float GetTimeSinceLastShot() {
        return Time.time - lastShotAt;
    }

    private Vector3 projectilePosOffset = Vector3.zero; //new Vector3(-0.039f, -1.15f, 0.05f);
    private void Shoot(GameObject enemy) {
        if (GetWaitTimeBetweenShots() < GetTimeSinceLastShot()) {
            towerSpell.SpawnBaseProjectile(towerTurret.transform, towerHead.transform.rotation, towerHeadCollider);
            lastShotAt = Time.time;
        }
    }
}

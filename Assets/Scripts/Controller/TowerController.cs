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
        get { return shootForce; }
        set {shootForce = value; }
    }

    private float damagePerHit;
    public float DamagePerHit {
        get { return damagePerHit; }
        set { damagePerHit = value; }
    }

    private int shotsPerMinute = 240;

    public int ShotsPerMinute {
        get { return shotsPerMinute; }
        set { shotsPerMinute = value; }
    }

    private float towerRange;
    public float TowerRange {
        get { return towerRange; }
        set { towerRange = value; }
    }

    private float maxUpwardAngleCorrection;
    public float MaxUpwardAngleCorrection {
        get { return maxUpwardAngleCorrection; }
        set { maxUpwardAngleCorrection = value; }
    }

    private bool isBeingCombod;

    public bool IsBeingCombod {
        get { return isBeingCombod; }
        set { isBeingCombod = value; }
    }

    private Spell towerSpell;

    public float turretSpinSpeed = 2.0f;
    public GameObject? projectilePrefab;
    private SpawnManager? spawnManager = null;
    private GameObject? towerHead = null;
    private Collider towerHeadCollider;
    private GameObject? towerTurret = null;
    private GameObject comboRing;
    // Start is called before the first frame update
    private void Start() {
        spawnManager = ReferenceManager.SpawnManagerComponent;

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

    private void Update() {
        UpdateComboRings();
    }

    private void UpdateComboRings() {
        comboRing.SetActive(IsBeingCombod);
    }

    public override void Activate() {
        if (Input.GetKey(KeyCode.C)) {
            IsBeingCombod = true;
            ReferenceManager.PlayerStateComponent.AddToTowerBeingCombod(this);
        }
    }

    private void FixedUpdate() {
        AimAtClosestEnemyInRange();
        ShootIfAimIsClose();
    }

    private void AimAtClosestEnemyInRange() {
        GameObject? closestEnemy = TargetingUtils.GetClosestEnemyInRange(towerHead.transform, TowerRange);
        if (closestEnemy == null) {
            return;
        }

        float singleStep = turretSpinSpeed * Time.deltaTime;
        Vector3 targetPos = TargetingUtils.GetTargetPosWithCompensation(towerHead.transform, closestEnemy, TowerRange, MaxUpwardAngleCorrection);
        print($"targetPos: {targetPos}");
        targetPos = TargetingUtils.ApplySpellTowerTurretOffset(towerSpell.SpellTowerTurretOffset, targetPos,
            towerHead.transform.position, TowerRange);
        print($"Offset targetPos: {targetPos}");
        Vector3 newDirection = Vector3.RotateTowards(towerHead.transform.forward, targetPos, singleStep, 0.0f);
        Debug.DrawRay(towerHead.transform.position, newDirection, Color.red);

        towerHead.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private float maxAngleDeltaToShootFrom = 3.0f;
    private void ShootIfAimIsClose() {
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


    private float GetWaitTimeBetweenShots() {
        return 60.0f / ShotsPerMinute; // Seconds
    }

    private float lastShotAt = 0.0f;
    private float GetTimeSinceLastShot() {
        return Time.time - lastShotAt;
    }

    private Vector3 projectilePosOffset = new Vector3(-0.039f, -1.15f, 0.05f);
    private void Shoot(GameObject enemy) {
        if (GetWaitTimeBetweenShots() < GetTimeSinceLastShot()) {
            towerSpell.SpawnBaseProjectile(towerTurret.transform, towerHead.transform.rotation, towerHeadCollider);
            lastShotAt = Time.time;
        }
    }
}

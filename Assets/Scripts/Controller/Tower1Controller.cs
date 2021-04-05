#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower1Controller : MonoBehaviour
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

    private Spell towerSpell;

    public float turretSpinSpeed = 2.0f;
    public GameObject? projectilePrefab;
    private SpawnManager? spawnManager = null;
    private GameObject? towerHead = null;
    private Collider towerHeadCollider;
    private GameObject? towerTurret = null;
    // Start is called before the first frame update
    private void Start() {
        spawnManager = ReferenceManager.SpawnManagerComponent;

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
        print($"Just set tower damage to: {ability.DamagePerHit}");
        ShotsPerMinute = ability.ShotsPerMinute;
        TowerRange = ability.TowerShotRange;
    }


    // Update is called once per frame
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

        //float angleToTarget = Vector3.Angle(towerHead.transform.position, closestEnemy.transform.position);
        Vector3 targetPos = TargetingUtils.GetTargetPosWithCompensation(towerHead.transform, closestEnemy, TowerRange, MaxUpwardAngleCorrection);
        float angleToTarget = Vector3.Angle(towerHead.transform.forward, targetPos);
        if (angleToTarget < maxAngleDeltaToShootFrom) {
            //print("Shooting at target");
            Shoot(closestEnemy);
        } else {
            //print($"Not shooting: Angle too far. Angle is: {angleToTarget}");
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
            //Vector3 spawnLocation = towerTurret.transform.position + towerHead.transform.forward * 1.4f;
            towerSpell.SpawnBaseProjectile(towerTurret.transform, towerHead.transform.rotation, towerHeadCollider);
                /*
            var controller = Instantiate(
                          projectilePrefab,
                          spawnLocation,
                          towerHead.transform.rotation,
                          towerTurret.transform)
                          .GetComponent<ProjectileController>();
            controller.ProjectileDamage = DamagePerHit;
            controller.ProjectileSpell = towerSpell;
            */

            lastShotAt = Time.time;
        }
    }
}

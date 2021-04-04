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

    public float turretSpinSpeed = 2.0f;
    public GameObject? projectilePrefab;
    private SpawnManager? spawnManager = null;
    private GameObject? towerHead = null;
    private GameObject? towerTurret = null;
    // Start is called before the first frame update
    private void Start() {
        spawnManager = ReferenceManager.SpawnManagerComponent;

        towerHead = transform.Find("TowerHead").gameObject;
        towerTurret = towerHead.transform.Find("TowerTurret").gameObject;
    }

    public void SetAbility(PlayerAbility ability) {
        // Set ability here

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
        GameObject? closestEnemy = GetClosestEnemyInRange();
        if (closestEnemy == null) {
            return;
        }

        float singleStep = turretSpinSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(towerHead.transform.forward, GetTargetDirection(closestEnemy), singleStep, 0.0f);
        Debug.DrawRay(towerHead.transform.position, newDirection, Color.red);

        towerHead.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private float GetDistanceToEnemy(GameObject enemy) {
        return Vector3.Distance(enemy.transform.position, transform.position);
    }

    private GameObject? GetClosestEnemyInRange() {
        List<GameObject> enemiesInRange = new List<GameObject>();
        List<GameObject> enemiesAlive = spawnManager.GetAliveEnemies();
        enemiesInRange = (from enemy in enemiesAlive
                         where GetDistanceToEnemy(enemy) < TowerRange
                         select enemy)
                         .ToList();

        if (enemiesInRange.Count == 0) {
            return null;
        }

        return enemiesInRange
            .OrderBy(enemy => GetDistanceToEnemy(enemy))
            .FirstOrDefault();
    }


    private Vector3 GetTargetDirection(GameObject enemy) {
        float upwardCorrectionAngle = (MaxUpwardAngleCorrection * (Vector3.Distance(enemy.transform.position, transform.position)) / TowerRange);
        Vector3 angleOffset = new Vector3(0.0f, upwardCorrectionAngle, 0.0f);
        return enemy.transform.position - towerHead.transform.position + angleOffset;
    }

    private float maxAngleDeltaToShootFrom = 3.0f;
    private void ShootIfAimIsClose() {
        GameObject? closestEnemy = GetClosestEnemyInRange();
        if (closestEnemy == null) {
            return;
        }

        //float angleToTarget = Vector3.Angle(towerHead.transform.position, closestEnemy.transform.position);
        float angleToTarget = Vector3.Angle(towerHead.transform.forward, GetTargetDirection(closestEnemy));
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
            Vector3 spawnLocation = towerTurret.transform.position + towerHead.transform.forward * 1.4f;
            var obj = Instantiate(
                          projectilePrefab,
                          spawnLocation,
                          towerHead.transform.rotation,
                          towerTurret.transform)
                          .GetComponent<ProjectileController>();
            obj.ProjectileDamage = DamagePerHit;
            print($"Setting projectile damage to {DamagePerHit}");
            obj.ShootForce = ShootForce;

            lastShotAt = Time.time;
        }
    }
}

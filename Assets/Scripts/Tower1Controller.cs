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

    public float turretSpinSpeed = 2.0f;
    public GameObject? projectilePrefab;
    private GameController? gameController = null;
    private GameObject? towerHead = null; 
    private GameObject? towerTurret = null;
    // Start is called before the first frame update
    private void Start() {
        ShootForce = 50.0f;
        DamagePerHit = 25.0f;

        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        towerHead = transform.Find("TowerHead").gameObject;
        towerTurret = towerHead.transform.Find("TowerTurret").gameObject;
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

    public float maxDistance = 25.0f;
    private GameObject? GetClosestEnemyInRange() {
        List<GameObject> enemiesInRange = new List<GameObject>();
        //print(gameController);
        List<GameObject> enemiesAlive = gameController.GetAliveEnemies();
        enemiesInRange = (from enemy in enemiesAlive
                         where GetDistanceToEnemy(enemy) < maxDistance
                         select enemy)
                         .ToList();

        if (enemiesInRange.Count == 0) {
            //print("Found no enemies in range");
            return null;
        }

        return enemiesInRange
            .OrderBy(enemy => GetDistanceToEnemy(enemy))
            .FirstOrDefault();
    }

    private float maxUpwardAngleCorrection = 7.0f;
    private Vector3 GetTargetDirection(GameObject enemy) {
        float upwardCorrectionAngle = (maxUpwardAngleCorrection * (Vector3.Distance(enemy.transform.position, transform.position)) / maxDistance);
        Vector3 angleOffset = new Vector3(0.0f, upwardCorrectionAngle, 0.0f);
        return enemy.transform.position - towerHead.transform.position + angleOffset;
    }

    private float maxAngleDeltaToShootFrom = 5.0f;
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

    private int shotsPerMinute = 240;
    private float GetWaitTimeBetweenShots() {
        return 60.0f / shotsPerMinute; // Seconds
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
            obj.ShootForce = ShootForce;

            lastShotAt = Time.time;
        }
    }
}

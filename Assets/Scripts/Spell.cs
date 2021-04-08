using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Purchaseable {
    public Spell(GameObject prefab) {
        InstancePrefab = prefab;
    }

    private float damagePerHit;
    public float DamagePerHit {
        get => damagePerHit;
        set => damagePerHit = value;
    }

    private bool canSplash;
    public bool CanSplash {
        get => canSplash;
        set => canSplash = value;
    }

    private int shotsPerMinute;
    public int ShotsPerMinute {
        get => shotsPerMinute;
        set => shotsPerMinute = value;
    }

    private float shootForce;
    public float ShootForce {
        get => shootForce;
        set => shootForce = value;
    }

    public float GetWaitTimeBetweenShots() {
        return 60.0f / ShotsPerMinute;
    }

    private float lastShotAt;
    public float LastShotAt {
        get => lastShotAt;
        set => lastShotAt = value;
    }

    private float towerShotRange;
    public float TowerShotRange {
        get => towerShotRange;
        set => towerShotRange = value;
    }

    private float maxUpwardAngleCorrection = 5.0f;
    public float MaxUpwardAngleCorrection {
        get => maxUpwardAngleCorrection;
        set => maxUpwardAngleCorrection = value;
    }

    private Vector3 spellTowerTurretOffset;
    public Vector3 SpellTowerTurretOffset {
        get => spellTowerTurretOffset;
        set => spellTowerTurretOffset = value;
    }

    private Vector3 instanceSpawnOffset;
    public Vector3 InstanceSpawnOffset {
        get => instanceSpawnOffset;
        set => instanceSpawnOffset = value;
    }

    private ProjectileAnimationController animation;
    public ProjectileAnimationController Animation {
        get => animation;
        set => animation = value;
    }

    public virtual void Animate(float timeSinceSpawned, Transform projectileTransform, Rigidbody projectileRb) {
        Animation.Animate(timeSinceSpawned, projectileTransform, projectileRb);
    }

    public virtual void OnProjectileHit(Transform parentProjectileTransform, Collider splashInvokerCollider) { }

    public virtual GameObject SpawnBaseProjectile(Transform selfTransform, Quaternion enemyDir, Collider spawnerCollider) {
        return SpawnInstance(selfTransform.position + InstanceSpawnOffset, enemyDir, DamagePerHit, spawnerCollider, CanSplash);
    }

    protected GameObject SpawnInstance(Vector3 spawnLoc, Quaternion spawnRot, float damage, Collider spawnerCollider, bool canSplash) {
        Debug.Log($"Spawning: {spawnLoc}, {spawnRot}, {damage}, {shootForce}, {spawnerCollider}");
        GameObject obj = Object.Instantiate(InstancePrefab, spawnLoc, spawnRot);
        obj.layer = Layers.FriendlyProjectiles;

        ProjectileController controller = obj.GetComponent<ProjectileController>();
        controller.ProjectileSpell = this;
        controller.ProjectileDamage = damage;
        controller.CanSplash = canSplash;

        Collider projectileCollider = obj.GetComponent<Collider>();
        Physics.IgnoreCollision(projectileCollider, spawnerCollider);

        return obj;
    }
}

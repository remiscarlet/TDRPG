using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

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

    /// <summary>
    /// This method is called once per <c>FixedUpdate()</c> and should update any physics related movements, ie "animations",
    /// for this projectile.
    ///
    /// Animations should generally be getting handled via the <c>AnimationController</c> derivatives.
    /// </summary>
    /// <param name="timeSinceSpawned"></param>
    /// <param name="projectileTransform"></param>
    /// <param name="projectileRb"></param>
    public virtual void Animate(float timeSinceSpawned, Transform projectileTransform, Rigidbody projectileRb) {
        Animation.Animate(timeSinceSpawned, projectileTransform, projectileRb);
    }

    /// <summary>
    /// Called when the projectile hits a target on the <c>Enemy</c> layer and the spell has <c>CanSplash</c> set to true.
    ///
    /// Use this method to activate any post-impact logic such as creating additional projectiles or applying status effects.
    /// TODO: Probably means I should move damage logic here?
    /// </summary>
    /// <param name="parentProjectileTransform"></param>
    /// <param name="splashInvokerCollider"></param>
    public virtual void OnProjectileHit(Transform parentProjectileTransform, Collider splashInvokerCollider) { }

    /// <summary>
    /// Spawn the initial projectiles - ie non-splash/non-secondary projectiles.
    ///
    /// One might override this method when more than a single projectile is spawned.
    /// One might not use this method when instantiating for example splash projectiles which should be created via OnProjectileHit()
    ///
    /// TODO: Account for multi projectile spawning. Current return type assumes only one projectile.
    /// </summary>
    /// <param name="spawnerTransform"></param>
    /// <param name="enemyDir"></param>
    /// <param name="spawnerCollider"></param>
    /// <returns></returns>
    public virtual GameObject SpawnBaseProjectile(Transform spawnerTransform, Quaternion? enemyDir, Collider spawnerCollider) {
        if (enemyDir == null) {
            throw new Exception("Got null for enemyDir!");
        }

        // Can't seem to assign directly even with ^? The ?? is technically redundant.
        Quaternion rotToEnemy = enemyDir ?? default(Quaternion);
        return SpawnInstance(spawnerTransform.position + InstanceSpawnOffset, rotToEnemy, DamagePerHit, spawnerCollider, CanSplash);
    }

    /// <summary>
    /// Spawn the spell projectile.
    ///
    /// This method handles creation of projectiles and settings its various states.
    /// </summary>
    /// <param name="spawnLoc"></param>
    /// <param name="spawnRot"></param>
    /// <param name="damage"></param>
    /// <param name="spawnerCollider"></param>
    /// <param name="canSplash"></param>
    /// <returns>The spawned projectile <c>GameObject</c></returns>
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

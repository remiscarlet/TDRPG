using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComboTypes {
    public class mm1fb1 : ComboType {
        public mm1fb1() {
            InstancePrefab = ReferenceManager.Prefabs.Fireball;
            DamagePerHit = 150.0f;
            ShotsPerMinute = 60;
            ShootForce = 50.0f;
            TowerShotRange = 50.0f;
            InstanceSpawnOffset = Vector3.zero;
            CanSplash = false; // for now.
        }

        public override void Animate(float timeSinceSpawned, Transform projectileTransform, Rigidbody projectileRb) {
            //base.Animate(timeSinceSpawned, projectileTransform, projectileRb);
        }

        public override void OnProjectileHit(Transform parentProjectileTransform, Collider splashInvokerCollider) {
            //base.OnProjectileHit(parentProjectileTransform, splashInvokerCollider);
        }

        private const int NumberOfProjectiles = 4;
        public override GameObject SpawnBaseProjectile(Transform spawnerTransform, Quaternion enemyDir, Collider spawnerCollider) {
            //return base.SpawnBaseProjectile(spawnerTransform, enemyDir, spawnerCollider);
            for (int i = 0; i < NumberOfProjectiles; i++) {
                // Temporary placeholder spawnLoc offset
                SpawnInstance(spawnerTransform.position + Vector3.one * i, enemyDir, DamagePerHit, spawnerCollider, CanSplash);
            }

            return null;
        }
    }
}

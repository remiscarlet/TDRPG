using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ComboTypes {
    public class mm1fb1 : ComboType {
        public mm1fb1() {
            Animation = new SpellAnimations.mm1fb1();
            InstancePrefab = ReferenceManager.Prefabs.Fireball;
            DamagePerHit = 150.0f;
            ShotsPerMinute = 60;
            ShootForce = 50.0f;
            TowerShotRange = 50.0f;
            InstanceSpawnOffset = Vector3.zero;
            CanSplash = false; // for now.
        }

        private const int NumberOfProjectiles = 1;
        public override GameObject SpawnBaseProjectile(Transform spawnerTransform, Quaternion? _, Collider spawnerCollider) {
            //return base.SpawnBaseProjectile(spawnerTransform, enemyDir, spawnerCollider);
            Vector3 rotVector;
            Quaternion rot;
            for (int i = 0; i < NumberOfProjectiles; i++) {
                // Temporary placeholder spawnLoc offset
                //rot = Quaternion.LookRotation(new Vector3(0, 0, 0));
                rotVector = new Vector3(0, 90, 180);
                rot = Quaternion.Euler(rotVector);
                Debug.Log($"Setting rot to: {rot} with rotVector {rotVector}");
                SpawnInstance(spawnerTransform.position + Vector3.one * i, rot, DamagePerHit, spawnerCollider, CanSplash);
            }

            return null;
        }
    }
}

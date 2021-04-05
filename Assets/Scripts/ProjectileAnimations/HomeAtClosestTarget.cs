using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ProjectileAnimations {
    public class HomeAtClosestTarget : ProjectileAnimation {
        private float animDuration;
        private float projectileForce;
        private float timeAnimStarted = -1.0f;

        public HomeAtClosestTarget(float duration, float force) {
            animDuration = duration;
            projectileForce = force;
        }

        public override float GetDuration() {
            return animDuration;
        }

        public override bool Animate(float timeSinceProjectileSpawned, Transform projectileTransform, Rigidbody projectileRb) {
            float timeSinceAnimStarted;
            if (timeAnimStarted == -1.0f) {
                timeSinceAnimStarted = 0.0f;
                timeAnimStarted = timeSinceProjectileSpawned;
            } else {
                timeSinceAnimStarted = timeSinceProjectileSpawned - timeAnimStarted;
            }

            GameObject enemy = TargetingUtils.GetClosestEnemyInRange(projectileTransform, Mathf.Infinity);
            if (enemy == null) {
                Debug.Log("No enemies left to home in on.");
                return false;
            }

            Vector3 targetPos =
                TargetingUtils.GetTargetPosWithCompensation(projectileTransform, enemy, Mathf.Infinity, 0.0f);

            Vector3 forceVectorNorm = (targetPos - projectileTransform.position).normalized;
            Debug.Log($"Homing AddForce({forceVectorNorm} on projectile to target {targetPos} - timeSinceAnimStarted {timeSinceAnimStarted}");
            projectileRb.AddForce(forceVectorNorm * projectileForce, ForceMode.Force);

            return false;
        }
    }
}

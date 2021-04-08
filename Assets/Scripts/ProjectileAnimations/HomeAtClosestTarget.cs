using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectileAnimations {
    public class HomeAtClosestTarget : ProjectileAnimation {
        private float animDuration;
        private float projectileForce;
        private float timeAnimStarted = -1.0f;

        private const float MaxRadianTurn = 3.0f;
        private const float maxVelocity = 100.0f;

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
                //Debug.Log("No enemies left to home in on.");
                return false;
            }
            //Debug.Log($"projTransform.position: {projectileTransform.position} - transform.fwd {projectileTransform.transform.forward} - enemy pos: {enemy.transform.position}");

            //Vector3 targetPos =
            //    TargetingUtils.GetTargetPosWithCompensation(projectileTransform, enemy, Mathf.Infinity, 0.0f);
            Vector3 targetPos = Vector3.RotateTowards(projectileTransform.transform.forward,
                            enemy.transform.position - projectileTransform.position,
                MaxRadianTurn, 0.0f);

            //Vector3 forceVectorNorm = (targetPos - projectileTransform.position).normalized;
            //Debug.Log(
            //    $"Homing AddForce({targetPos.normalized} * {projectileForce} on projectile to target {targetPos} - timeSinceAnimStarted {timeSinceAnimStarted}");

            //projectileTransform.rotation = Quaternion.RotateTowards(projectileTransform.transform.forward, );
            Quaternion targetRotation = Quaternion.LookRotation(targetPos, projectileTransform.up);
            projectileTransform.rotation = targetRotation;//Quaternion.RotateTowards(currRotation, targetRotation, 1.0f);
            projectileRb.AddForce(targetPos.normalized * projectileForce * 5, ForceMode.Force);

            //Quaternion targetRotation =
            //    Quaternion.LookRotation(targetPos, projectileTransform.up);
            //projectileTransform.rotation = Quaternion.Slerp(projectileTransform.rotation, targetRotation, 1.0f);
            //projectileRb.AddForce(projectileTransform.forward * projectileForce, ForceMode.Force);
            //projectileRb.AddForce(projectileTransform.up * 10.0f, ForceMode.Force);

            //projectileRb.AddForce(
            //    new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f)), ForceMode.Impulse);

            return false;
        }
    }
}

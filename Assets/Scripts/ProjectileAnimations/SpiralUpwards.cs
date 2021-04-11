using System.Runtime.InteropServices;
using UnityEngine;

namespace ProjectileAnimations {
    public class SpiralUpwards : ProjectileAnimation {
        private float duration;
        private float timeAnimStarted = -1.0f;
        private float impulseForce;

        public SpiralUpwards(float dur, float force) {
            duration = dur;
            impulseForce = force;
        }

        public override float GetDuration() {
            return duration;
        }

        private Vector3 projectileForceOffset = new Vector3(-2.0f, 2.0f, 0.0f);
        public override bool Animate(float timeSinceProjectileSpawned, Transform projectileTransform, Rigidbody projectileRb) {
            float timeSinceAnimStarted;
            if (timeAnimStarted == -1.0f) {
                timeAnimStarted = timeSinceProjectileSpawned;
                timeSinceAnimStarted = 0.0f;
            } else {
                timeSinceAnimStarted = timeSinceProjectileSpawned - timeAnimStarted;
            }

            //Debug.Log($"Impulse animate: timeSinceAnimStarted {timeSinceAnimStarted}");
            if (timeSinceAnimStarted >= duration) {
                return true;
            } else {
                Vector3 forceDir = projectileTransform.forward + projectileForceOffset;
                projectileRb.AddForce(forceDir * impulseForce, ForceMode.Impulse);
                projectileTransform.rotation = Quaternion.LookRotation(projectileRb.velocity);
            }

            return false;
        }
    }
}

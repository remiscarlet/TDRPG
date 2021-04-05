using System.Runtime.InteropServices;
using UnityEngine;

namespace ProjectileAnimations {
    public class ImpulseFwdAtStart : ProjectileAnimation {
        private float waitAfterImpulse;
        private float timeAnimStarted = -1.0f;
        private float impulseForce;

        public ImpulseFwdAtStart(float waitTime, float force) {
            waitAfterImpulse = waitTime;
            impulseForce = force;
        }

        public override float GetDuration() {
            return waitAfterImpulse;
        }

        public override bool Animate(float timeSinceProjectileSpawned, Transform projectileTransform, Rigidbody projectileRb) {
            float timeSinceAnimStarted;
            if (timeAnimStarted == -1.0f) {
                timeAnimStarted = timeSinceProjectileSpawned;
                timeSinceAnimStarted = 0.0f;
            } else {
                timeSinceAnimStarted = timeSinceProjectileSpawned - timeAnimStarted;
            }

            Debug.Log($"Impulse animate: timeSinceAnimStarted {timeSinceAnimStarted}");
            if (timeSinceAnimStarted == 0.0f) {
                Debug.Log($"Impulse AddForce({projectileTransform.forward} * {impulseForce} on projectile");
                projectileRb.AddForce(projectileTransform.forward * impulseForce, ForceMode.Impulse);
            } else if (timeSinceAnimStarted >= waitAfterImpulse) {
                return true;
            }

            return false;
        }
    }
}

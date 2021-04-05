using UnityEngine;

namespace SpellAnimations {
    public class FireballSplash : ProjectileAnimationController {
        public FireballSplash() {
            animations = new ProjectileAnimation[]{
                new ProjectileAnimations.ImpulseFwdAtStart(1.0f, 20.0f),
            };
        }

        public override void Animate(float timeSinceSpawned, Transform projectileTransform, Rigidbody projectileRb) {
            GetCurrAnimation(timeSinceSpawned).Animate(timeSinceSpawned, projectileTransform, projectileRb);
        }

        public override ProjectileAnimation GetCurrAnimation(float timeSinceSpawned) {
            return animations[0];
        }
    }
}

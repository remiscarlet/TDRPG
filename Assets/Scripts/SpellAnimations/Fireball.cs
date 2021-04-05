using UnityEngine;

namespace SpellAnimations {
    public class Fireball : ProjectileAnimationController {
        public Fireball() {
            animations = new ProjectileAnimation[]{
                new ProjectileAnimations.ImpulseFwdAtStart(1.0f, 50.0f),
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

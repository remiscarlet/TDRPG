using UnityEngine;

namespace SpellAnimations {
    public class Fireball : ProjectileAnimationController {
        protected override void SetAnimation() {
            animations = new ProjectileAnimation[] {
                new ProjectileAnimations.ImpulseFwdAtStart(1.0f, 50.0f),
            };
        }
    }
}

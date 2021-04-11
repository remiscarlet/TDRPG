using UnityEngine;

namespace SpellAnimations {
    public class FireballSplash : ProjectileAnimationController {
        protected override void SetAnimation() {
            animations = new ProjectileAnimation[] {
                new ProjectileAnimations.ImpulseFwdAtStart(1.0f, 20.0f),
            };
        }
    }
}

using System.Runtime.InteropServices;
using UnityEngine;

namespace SpellAnimations {
    public class MagicMissile : ProjectileAnimationController {
        protected override void SetAnimation() {
            animations = new ProjectileAnimation[] {
                new ProjectileAnimations.ImpulseFwdAtStart(0.5f, 20.0f),
                new ProjectileAnimations.HomeAtClosestTarget(Mathf.Infinity, 50.0f),
            };
        }
    }
}

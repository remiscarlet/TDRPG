using System.Runtime.InteropServices;
using UnityEngine;

namespace SpellAnimations {
    public class mm1fb1 : ProjectileAnimationController {
        protected override void SetAnimation() {
            animations = new ProjectileAnimation[] {
                new ProjectileAnimations.SpiralUpwards(0.5f, 10.0f),
                new ProjectileAnimations.HomeAtClosestTarget(Mathf.Infinity, 50.0f),
            };
        }
    }
}

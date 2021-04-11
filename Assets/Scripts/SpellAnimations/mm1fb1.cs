using System.Runtime.InteropServices;
using UnityEngine;

namespace SpellAnimations {
    public class mm1fb1 : ProjectileAnimationController {
        protected override void SetAnimation() {
            animations = new ProjectileAnimation[] {
                new ProjectileAnimations.SpiralUpwards(3.0f, 30.0f),
                new ProjectileAnimations.HomeAtClosestTarget(Mathf.Infinity, 50.0f),
            };
        }
    }
}

using System.Runtime.InteropServices;
using UnityEngine;

namespace SpellAnimations {
    public class MagicMissile : ProjectileAnimationController {
        private float[] animationDurs;
        public MagicMissile() {
            animations = new ProjectileAnimation[]{
                new ProjectileAnimations.ImpulseFwdAtStart(0.5f, 20.0f),
                new ProjectileAnimations.HomeAtClosestTarget(Mathf.Infinity, 200.0f),
            };

            float durSoFar = 0.0f;
            animationDurs = new float[animations.Length];
            for (int animIndex = 0; animIndex < animations.Length; animIndex++) {
                animationDurs[animIndex] = durSoFar + animations[animIndex].GetDuration();
                durSoFar = animationDurs[animIndex];
            }
        }

        public override void Animate(float timeSinceSpawned, Transform projectileTransform, Rigidbody projectileRb) {
            GetCurrAnimation(timeSinceSpawned).Animate(timeSinceSpawned, projectileTransform, projectileRb);
        }

        public override ProjectileAnimation GetCurrAnimation(float timeSinceSpawned) {
            for (int idx = 0; idx < animations.Length; idx++) {
                if (animationDurs[idx] > timeSinceSpawned) {
                    return animations[idx];
                }
            }

            return animations[animations.Length - 1];
        }
    }
}

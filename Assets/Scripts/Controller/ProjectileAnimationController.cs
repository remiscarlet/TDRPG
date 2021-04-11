using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnimationController {
    protected ProjectileAnimation[] animations;
    protected float[] animationDurs;

    protected ProjectileAnimationController() {
        SetAnimation();

        float durSoFar = 0.0f;
        animationDurs = new float[animations.Length];
        for (int animIndex = 0; animIndex < animations.Length; animIndex++) {
            animationDurs[animIndex] = durSoFar + animations[animIndex].GetDuration();
            durSoFar = animationDurs[animIndex];
        }
    }

    protected virtual void SetAnimation() {
        throw new Exception("Unimplemented exception");
    }

    public virtual void Animate(float timeSinceSpawned, Transform projectileTransform, Rigidbody projectileRb) {
        GetCurrAnimation(timeSinceSpawned).Animate(timeSinceSpawned, projectileTransform, projectileRb);
    }

    protected virtual ProjectileAnimation GetCurrAnimation(float timeSinceSpawned) {
        for (int idx = 0; idx < animations.Length; idx++) {
            if (animationDurs[idx] > timeSinceSpawned) {
                return animations[idx];
            }
        }

        return animations[animations.Length - 1];
    }
}

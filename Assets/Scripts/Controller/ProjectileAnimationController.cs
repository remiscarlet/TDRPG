using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnimationController {
    protected ProjectileAnimation[] animations;

    public virtual void Animate(float timeSinceSpawned, Transform projectileTransform, Rigidbody projectileRb) {
        throw new System.Exception("Unimplemented Exception");
    }

    public virtual ProjectileAnimation GetCurrAnimation(float timeSinceSpawned) {
        throw new System.Exception("Unimplemented Exception");
    }
}

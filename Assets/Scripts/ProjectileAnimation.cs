using UnityEngine;

public class ProjectileAnimation {
    public virtual float GetDuration() {
        throw new System.Exception("Unimplemented Exception");
    }
    public virtual bool Animate(float timeSinceProjectileSpawned, Transform projectileTransform, Rigidbody projectileRb) {
        throw new System.Exception("Unimplemented Exception");
    }
}

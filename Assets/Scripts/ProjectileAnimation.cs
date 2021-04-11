using UnityEngine;

/// <summary>
/// Base class/interface for individual <c>ProjectileAnimation</c> derivatives.
/// </summary>
public class ProjectileAnimation {
    public virtual float GetDuration() {
        throw new System.Exception("Unimplemented Exception");
    }
    public virtual bool Animate(float timeSinceProjectileSpawned, Transform projectileTransform, Rigidbody projectileRb) {
        throw new System.Exception("Unimplemented Exception");
    }
}

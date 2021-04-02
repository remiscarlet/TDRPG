using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : PlayerAbility {
    public Fireball(GameObject prefab) : base(prefab) {
        AbilityName = "Fireball";
        Description = "A 'fire' ball.";
        DamagePerHit = 150.0f;
        ShotsPerMinute = 60.0f;
        ShootForce = 20.0f;
        IconTex = Resources.Load<Texture2D>("Images/PlayerAbility/Fireball");
        Price = 250;
    }

    public override void SpawnInstances(Transform self, Quaternion enemyDir) {
        var obj = Object.Instantiate(
                    InstancePrefab,
                    self.position + self.forward * 2.0f,
                    enemyDir)
                    .GetComponent<ProjectileController>();
        obj.ProjectileDamage = DamagePerHit;
        obj.ShootForce = ShootForce;
    }
}

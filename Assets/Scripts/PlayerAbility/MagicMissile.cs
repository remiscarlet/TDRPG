using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public class MagicMissile : PlayerAbility {
    public MagicMissile(GameObject prefab) : base(prefab) {
        AbilityName = "Magic Missile";
        Description = "This is a 'magic missile'.";
        DamagePerHit = 50.0f;
        ShotsPerMinute = 300;
        ShootForce = 40.0f;
        TowerShotRange = 100.0f;
        IconTex = Resources.Load<Texture2D>("Images/PlayerAbility/Magic_Missile");
        Price = 100;
    }

    public override void SpawnInstances(Transform self, Quaternion enemyDir) {
        foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this)) {
            string name=descriptor.Name;
            object value=descriptor.GetValue(this);
            Debug.Log($"{name}={value}");
        }


        Debug.Log("Spawning MagicMissile instances...");
        Debug.Log(InstancePrefab);
        var obj = Object.Instantiate(
                    InstancePrefab,
                    self.position + self.forward * 1.5f,
                    enemyDir)
                    .GetComponent<ProjectileController>();
        obj.ProjectileDamage = DamagePerHit;
        obj.ShootForce = ShootForce;
    }
}

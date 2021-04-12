using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace Spells {
    public class MagicMissile : Spell {
        public MagicMissile(GameObject prefab) : base(prefab) {
            AbilityName = "Magic Missile";
            Description = "This is a 'magic missile'.";

            Animation = new SpellAnimations.MagicMissile();
            CanSplash = false;
            DamagePerHit = 50.0f;
            ShotsPerMinute = 300;
            ShootForce = 40.0f;
            MaxUpwardAngleCorrection = 0.0f;
            TowerShotRange = 100.0f;
            IconTex = Resources.Load<Texture2D>("Images/Spells/Magic_Missile");
            SpellTowerTurretOffset = new Vector3(0.0f, 45.0f, 0.0f);
            Price = 100;
        }

        public override GameObject SpawnBaseProjectile(Transform self, Quaternion? enemyDir, Collider spawnerCollider) {
            InstanceSpawnOffset = Vector3.zero; //self.forward * 1.5f;
            //enemyDir.SetLookRotation(Vector3);
            GameObject projectile = base.SpawnBaseProjectile(self, enemyDir, spawnerCollider);
            return projectile;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.UI;
using UnityEngine;
using Object = UnityEngine.Object;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Resources = UnityEngine.Resources;
using Vector3 = UnityEngine.Vector3;

namespace Spells {
    public class Fireball : Spell {
        public Fireball(GameObject prefab) : base(prefab) {
            AbilityName = "Fireball";
            Description = "A 'fire' ball.";

            Animation = new SpellAnimations.Fireball();
            CanSplash = true;
            DamagePerHit = 150.0f;
            ShotsPerMinute = 60;
            MaxUpwardAngleCorrection = 3.0f;
            TowerShotRange = 50.0f;
            IconTex = Resources.Load<Texture2D>("Images/Spells/Fireball");
            Price = 250;
        }

        private float splashMinDist = 0.5f;
        private float splashMaxDist = 1.0f;
        private float splashMaxYOffset = 5.0f;
        private Quaternion GetSplashSpawnRot(Transform parentProjectileTransform, float angle) {
            float yOffsetAngle = Random.Range(-splashMaxYOffset, splashMaxYOffset);

            Quaternion yRot = Quaternion.AngleAxis(yOffsetAngle, parentProjectileTransform.forward);
            Quaternion xzRot = Quaternion.AngleAxis(angle, parentProjectileTransform.up);

            return yRot * xzRot;
        }

        private const int NumSplashObjects = 25;
        private const float SplashDmg = 10.0f;
        private Vector3 SplashProjectileScale = new Vector3(0.25f, 0.25f, 0.25f);
        public override void OnProjectileHit(Transform projectileTransform, Collider splashInvokerCollider) {
            for (float angle = 0.0f; angle <= 360.0f; angle += 360.0f / NumSplashObjects) {
                Debug.Log("Spawning splash object");

                Quaternion spawnRot = GetSplashSpawnRot(projectileTransform, angle);
                Debug.Log(spawnRot);

                GameObject splashObj = base.SpawnInstance(projectileTransform.TransformPoint(Vector3.zero),
                                                          spawnRot, SplashDmg, splashInvokerCollider, false);
                splashObj.transform.localScale = SplashProjectileScale;
                //break;
            }
        }

        public override GameObject SpawnBaseProjectile(Transform self, Quaternion enemyDir, Collider spawnerCollider) {
            InstanceSpawnOffset = Vector3.zero; //self.forward * 2.0f;
            GameObject projectile = base.SpawnBaseProjectile(self, enemyDir, spawnerCollider);
            return projectile;
        }
    }
}

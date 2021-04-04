using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : Purchaseable {
    public PlayerAbility(GameObject prefab) {
        InstancePrefab = prefab;
    }

    private float damagePerHit;
    public float DamagePerHit {
        get { return damagePerHit; }
        set { damagePerHit = value; }
    }

    private int shotsPerMinute;
    public int ShotsPerMinute {
        get { return shotsPerMinute; }
        set { shotsPerMinute = value; }
    }

    private float shootForce;
    public float ShootForce {
        get { return shootForce;  }
        set { shootForce = value; }
    }

    public float GetWaitTimeBetweenShots() {
        return 60.0f / ShotsPerMinute;
    }

    private float lastShotAt;
    public float LastShotAt {
        get { return lastShotAt; }
        set { lastShotAt = value; }
    }

    private float towerShotRange;

    public float TowerShotRange {
        get { return towerShotRange; }
        set { towerShotRange = value; }
    }

    public virtual void SpawnInstances(Transform self, Quaternion enemyDir) {
        throw new System.Exception("Unimplemented Exception");
    }
}

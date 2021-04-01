using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility {
    public PlayerAbility(GameObject prefab) {
        InstancePrefab = prefab;
        Debug.Log("Setting instancePrefab");
        Debug.Log(InstancePrefab);
        Debug.Log(prefab);
    }

    private GameObject instancePrefab;
    public GameObject InstancePrefab {
        get { return instancePrefab; }
        set { instancePrefab = value; }
    }

    public float damagePerHit;
    public float DamagePerHit {
        get { return damagePerHit; }
        set { damagePerHit = value; }
    }

    public float shotsPerMinute;
    public float ShotsPerMinute {
        get { return shotsPerMinute; }
        set { shotsPerMinute = value; }
    }

    public float shootForce;
    public float ShootForce {
        get { return shootForce;  }
        set { shootForce = value; }
    }

    public float GetWaitTimeBetweenShots() {
        return 60.0f / ShotsPerMinute;
    }

    public float lastShotAt;
    public float LastShotAt {
        get { return lastShotAt; }
        set { lastShotAt = value; }
    }
    
    public virtual void SpawnInstances(Transform self, Quaternion enemyDir) {
        throw new System.Exception("Unimplemented Exception");
    }
}

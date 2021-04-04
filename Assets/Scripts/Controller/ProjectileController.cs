using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
    private float shootForce;
    public float ShootForce {
        get { return shootForce; }
        set { shootForce = value; }
    }

    private float projectileDamage;
    public float ProjectileDamage {
        get { return projectileDamage; }
        set { projectileDamage = value; }
    }

    private Rigidbody projectileRb;
    // Start is called before the first frame update
    void Start() {
        projectileRb = GetComponent<Rigidbody>();
        projectileRb.AddForce(transform.forward * shootForce, ForceMode.Impulse); 
    }

    // Update is called once per frame
    void Update() {
    }

    void OnCollisionEnter(Collision collision) {
        // C# why do you not allow fallthrough switch statements :(
        //print("Detected projectile collision");
        GameObject gameObj = collision.gameObject;

        if (gameObj.CompareTag("Enemy")) {
            EnemyController enemyController = gameObj.GetComponent<EnemyController>();
            
            if (enemyController.IsAlive()) {
                enemyController.InflictDamage(projectileDamage);
            }
        }

        if (gameObj.CompareTag("Enemy") || gameObj.CompareTag("Ground") || gameObj.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }   
}
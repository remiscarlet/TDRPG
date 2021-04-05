using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
    private float projectileDamage;
    public float ProjectileDamage {
        get { return projectileDamage; }
        set { projectileDamage = value; }
    }

    private Spell projectileSpell;
    public Spell ProjectileSpell {
        get { return projectileSpell; }
        set { projectileSpell = value; }
    }

    private bool canSplash;

    public bool CanSplash {
        get { return canSplash; }
        set { canSplash = value; }
    }

    private float spawnTime;
    private Vector3 spawnLoc;

    private Rigidbody projectileRb;
    private Collider projectileCollider;
    void Start() {
        spawnTime = Time.time;
        spawnLoc = transform.position;
        projectileRb = GetComponent<Rigidbody>();
        projectileCollider = GetComponent<Collider>();

        ProjectileSpell.Animate(0.0f, transform, projectileRb);
    }

    // Update is called once per frame
    void Update() {
        print($"ProjectileController calling Spell.Animate with timeSinceSpawned of {Time.time - spawnTime}");
        ProjectileSpell.Animate(Time.time - spawnTime, transform, projectileRb);
    }

    void OnCollisionEnter(Collision collision) {
        // C# why do you not allow fallthrough switch statements :(
        GameObject gameObj = collision.gameObject;

        if (gameObj.CompareTag("Enemy")) {
            print($"Detected collision: {transform.position}, {spawnLoc}");
            if (CanSplash) {
                ProjectileSpell.OnProjectileHit(transform, collision.collider);
            }

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

#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    private Transform goal;
    public float maxHealth = 100.0f;
    private float currHealth;
    public GameObject damageTextPrefab;
    private GameObject player;
    private bool isAlive = true;
    private GameController gameController;
    private GameObject? healthBarCurr = null;
    private float healthBarCurrXScale;

    public bool IsAlive() {
        return isAlive;
    }

    // Start is called before the first frame update
    private void Start() {
        healthBarCurr = transform.Find("MaxHealth/CurrHealth").gameObject;
        healthBarCurrXScale = healthBarCurr.transform.localScale.x;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        player = GameObject.Find("Player");
        currHealth = maxHealth;
        goal = GameObject.Find("Destination").transform;

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

    // Update is called once per frame
    private void Update() {
        DisplayHealth();
    }

    public void DisplayHealth() {
        float percentHealth = currHealth / maxHealth;
        Vector3 newHealthBarScale = new Vector3(healthBarCurrXScale * percentHealth, healthBarCurr.transform.localScale.y, healthBarCurr.transform.localScale.z);
        healthBarCurr.transform.localScale = newHealthBarScale; 
    }

    public void InflictDamage(float dmg) {
        currHealth -= dmg;
        DisplayDamage(dmg);
        if (currHealth < 0 && isAlive) {
            isAlive = false;
            Die();
        }
    }

    private float offsetBound = 3.0f;
    private Vector3 RandomizeDmgTextPosOffset() {
        return new Vector3(Random.Range(-offsetBound, offsetBound), 3.0f, Random.Range(-offsetBound, offsetBound));
    }

    private void DisplayDamage(float dmg) {
        Vector3 textSpawnLoc = transform.position + RandomizeDmgTextPosOffset();
        GameObject dmgText = Instantiate(damageTextPrefab, textSpawnLoc, transform.rotation, transform);
        dmgText.GetComponent<DamageTextController>().Initialize(dmg);
    }

    private void Die() {
        // Animation eventually?
        gameController.RemoveDeadEnemy(gameObject);
        Destroy(gameObject);
    }
}

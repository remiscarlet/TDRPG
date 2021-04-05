using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnLocations;
    private GameObject enemiesHierarchy;
    private List<GameObject> enemiesAlive;
    private int enemiesKilled;
    private int enemiesSpawnedInWave;
    public int WaveNum { get; set; }

    void Awake() {
        WaveNum = 1;
    }

    // Start is called before the first frame update
    void Start() {
        enemiesHierarchy = GameObject.Find("Enemies");
        enemiesAlive = new List<GameObject>();
        StartCoroutine("SpawnWave");
    }

    private float enemySpawnDelay = 1.0f;
    private IEnumerator SpawnWave() {
        enemiesKilled = 0;
        // For now just always spawn same num enemies as wave * 2
        enemiesSpawnedInWave = WaveNum * 2;

        for (int i = 0; i < enemiesSpawnedInWave; i++) {
            Transform spawnLoc = GetRandomSpawnLocation();

            GameObject enemy = Instantiate(enemyPrefabs[0], spawnLoc.position + new Vector3(0.0f, 1.0f, 0.0f), spawnLoc.rotation, enemiesHierarchy.transform);
            enemy.layer = Layers.Enemy;
            enemiesAlive.Add(enemy);

            yield return new WaitForSeconds(enemySpawnDelay);
        }
        yield return null;
    }

    private Transform GetRandomSpawnLocation() {
        GameObject spawnLoc = spawnLocations[Random.Range(0, spawnLocations.Length)];
        return spawnLoc.transform;
    }

    // Update is called once per frame
    void Update() {

    }

    public void RemoveDeadEnemy(GameObject enemy) {
        enemiesKilled += 1;
        enemiesAlive.Remove(enemy);

        if (enemiesKilled == enemiesSpawnedInWave) {
            StartCoroutine("SpawnNextWave");
        }
    }

    private float nextWaveSpawnDelay = 5.0f;
    private IEnumerator SpawnNextWave() {
        WaveNum += 1;
        // Other stuff?
        yield return new WaitForSeconds(nextWaveSpawnDelay);
        yield return StartCoroutine(SpawnWave());
    }

    public List<GameObject> GetAliveEnemies() {
        return enemiesAlive;
    }
}

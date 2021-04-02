using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnLocations;
    private GameObject enemiesHierarchy;
    private List<GameObject> enemiesAlive;
    private int waveNum = 1;
    public int WaveNum {
        get { return waveNum; }
        set { waveNum = value; }
    }


    // Start is called before the first frame update
    void Start() {
        enemiesHierarchy = GameObject.Find("Enemies");
        enemiesAlive = new List<GameObject>();
        StartCoroutine("SpawnWave");
    }

    private float enemySpawnDelay = 1.0f;
    private IEnumerator SpawnWave() {
        // For now just always spawn same num enemies as wave
        for (int i = 0; i < WaveNum * 2; i++) {
            Transform spawnLoc = GetRandomSpawnLocation();
            enemiesAlive.Add(Instantiate(enemyPrefabs[0], spawnLoc.position + new Vector3(0.0f, 1.0f, 0.0f), spawnLoc.rotation, enemiesHierarchy.transform));
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
        enemiesAlive.Remove(enemy);

        if (enemiesAlive.Count == 0) {
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

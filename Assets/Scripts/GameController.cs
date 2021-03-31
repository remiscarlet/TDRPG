using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] spawnLocations;
    private GameObject enemiesHierarchy;
    private List<GameObject> enemiesAlive;
    private int waveNum = 1;
    // Start is called before the first frame update
    void Start() {
        enemiesHierarchy = GameObject.Find("Enemies");
        enemiesAlive = new List<GameObject>();
        SpawnWave();
    }

    private void SpawnWave() {
        // For now just always spawn same num enemies as wave
        for (int i = 0; i < waveNum; i++) {
            Transform spawnLoc = GetRandomSpawnLocation();
            enemiesAlive.Add(Instantiate(enemyPrefabs[0], spawnLoc.position + new Vector3(0.0f, 1.0f, 0.0f), spawnLoc.rotation, enemiesHierarchy.transform));
        }
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
    }

    public List<GameObject> GetAliveEnemies() {
        return enemiesAlive;
    }
}

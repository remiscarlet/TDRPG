using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour {
    static ReferenceManager instance;
    void Awake() {
        instance = this;
    }

    public GameObject playerObject;
    public PlayerState playerStateComponent;

    public GameObject spawnManagerObject;
    public SpawnManager spawnManagerComponent;

    public static GameObject PlayerObject {
        get { return instance.playerObject; }
    }
    public static PlayerState PlayerStateComponent {
        get { return instance.playerStateComponent; }
    }

    public static GameObject SpawnManagerObject {
        get { return instance.spawnManagerObject; }
    }
    public static SpawnManager SpawnManagerComponent {
        get { return instance.spawnManagerComponent; }
    }

    void Start() {
        playerObject = GameObject.Find("Player");
        playerStateComponent = playerObject.GetComponent<PlayerState>();

        spawnManagerObject = GameObject.Find("SpawnManager");
        spawnManagerComponent = spawnManagerObject.GetComponent<SpawnManager>();
    }
}

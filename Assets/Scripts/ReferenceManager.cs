using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReferenceManager : MonoBehaviour {
    static ReferenceManager instance;
    void Awake() {
        instance = this;

        prefabs = GetComponent<PrefabManager>();

        eventSystemComponent = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        shopMenuRaycastUtilComponent = GameObject.Find("Environment/Shop/Menu").GetComponent<UIRaycasterUtil>();

        playerObject = GameObject.Find("Player");
        cameraObject = playerObject.transform.Find("Player Head/Main Camera").gameObject;
        playerStateComponent = playerObject.GetComponent<PlayerState>();
        print("ReferenceManager");
        print(playerObject);
        print(playerStateComponent);

        spawnManagerObject = GameObject.Find("SpawnManager");
        spawnManagerComponent = spawnManagerObject.GetComponent<SpawnManager>();

        towerManagerComponent = GameObject.Find("GameController").GetComponent<TowerManager>();
    }

    [System.NonSerialized] public PrefabManager prefabs;
    [System.NonSerialized] public EventSystem eventSystemComponent;
    [System.NonSerialized] public UIRaycasterUtil shopMenuRaycastUtilComponent;

    [System.NonSerialized] public GameObject playerObject;
    [System.NonSerialized] public GameObject cameraObject;
    [System.NonSerialized] public PlayerState playerStateComponent;

    [System.NonSerialized] public GameObject spawnManagerObject;
    [System.NonSerialized] public SpawnManager spawnManagerComponent;

    [System.NonSerialized] public TowerManager towerManagerComponent;

    public static PrefabManager Prefabs {
        get => instance.prefabs;
    }

    public static EventSystem EventSystemComponent {
        get => instance.eventSystemComponent;
    }
    public static UIRaycasterUtil ShopMenuRaycastUtilComponent {
        get => instance.shopMenuRaycastUtilComponent;
    }

    public static GameObject PlayerObject {
        get => instance.playerObject;
    }
    public static GameObject CameraObject {
        get => instance.cameraObject;
    }
    public static PlayerState PlayerStateComponent {
        get => instance.playerStateComponent;
    }

    public static GameObject SpawnManagerObject {
        get => instance.spawnManagerObject;
    }
    public static SpawnManager SpawnManagerComponent {
        get => instance.spawnManagerComponent;
    }

    public static TowerManager TowerManagerComponent {
        get => instance.towerManagerComponent;
    }
}

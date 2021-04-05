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

        spawnManagerObject = GameObject.Find("SpawnManager");
        spawnManagerComponent = spawnManagerObject.GetComponent<SpawnManager>();
    }

    [System.NonSerialized] public PrefabManager prefabs;
    [System.NonSerialized] public EventSystem eventSystemComponent;
    [System.NonSerialized] public UIRaycasterUtil shopMenuRaycastUtilComponent;

    [System.NonSerialized] public GameObject playerObject;
    [System.NonSerialized] public GameObject cameraObject;
    [System.NonSerialized] public PlayerState playerStateComponent;

    [System.NonSerialized] public GameObject spawnManagerObject;
    [System.NonSerialized] public SpawnManager spawnManagerComponent;

    public static PrefabManager Prefabs {
        get { return instance.prefabs; }
    }

    public static EventSystem EventSystemComponent {
        get { return instance.eventSystemComponent; }
    }
    public static UIRaycasterUtil ShopMenuRaycastUtilComponent {
        get { return instance.shopMenuRaycastUtilComponent; }
    }

    public static GameObject PlayerObject {
        get { return instance.playerObject; }
    }
    public static GameObject CameraObject {
        get { return instance.cameraObject; }
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
}

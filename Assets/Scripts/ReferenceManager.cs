using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is a singleton class that houses references to a bunch of commonly used
/// components, gameobjects, etc etc.
///
/// Calls to GetComponent() or otherwise searching for the right GameObject to call it on can be expensive.
/// Use this static class to have a quick and cheap way to reference common objects.
/// </summary>
public class ReferenceManager : MonoBehaviour {
    static ReferenceManager instance;
    void Awake() {
        instance = this;

        prefabs = GetComponent<PrefabManager>();
        comboTypes = GetComponent<ComboTypeManager>();

        eventSystemComponent = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        shopMenuRaycastUtilComponent = GameObject.Find("Environment/Shop/Menu").GetComponent<UIRaycasterUtil>();

        comboParentObject = GameObject.Find("Environment/Combos");
        playerObject = GameObject.Find("Player");
        cameraObject = playerObject.transform.Find("Dwarf_Orme_Head/Main Camera").gameObject;
        playerStateComponent = playerObject.GetComponent<PlayerState>();

        spawnManagerObject = GameObject.Find("SpawnManager");
        spawnManagerComponent = spawnManagerObject.GetComponent<SpawnManager>();

        towerManagerComponent = GameObject.Find("GameController").GetComponent<TowerManager>();
    }

    [System.NonSerialized] public PrefabManager prefabs;
    [System.NonSerialized] public ComboTypeManager comboTypes;
    [System.NonSerialized] public EventSystem eventSystemComponent;
    [System.NonSerialized] public UIRaycasterUtil shopMenuRaycastUtilComponent;

    [System.NonSerialized] public GameObject comboParentObject;
    [System.NonSerialized] public GameObject playerObject;
    [System.NonSerialized] public GameObject cameraObject;
    [System.NonSerialized] public PlayerState playerStateComponent;

    [System.NonSerialized] public GameObject spawnManagerObject;
    [System.NonSerialized] public SpawnManager spawnManagerComponent;

    [System.NonSerialized] public TowerManager towerManagerComponent;

    public static PrefabManager Prefabs {
        get => instance.prefabs;
    }

    public static ComboTypeManager ComboTypes {
        get => instance.comboTypes;
    }

    public static EventSystem EventSystemComponent {
        get => instance.eventSystemComponent;
    }
    public static UIRaycasterUtil ShopMenuRaycastUtilComponent {
        get => instance.shopMenuRaycastUtilComponent;
    }

    public static GameObject ComboParentObject {
        get => instance.comboParentObject;
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

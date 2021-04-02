using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReferenceManager : MonoBehaviour {
    static ReferenceManager instance;
    void Awake() {
        instance = this;

        eventSystemComponent = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        shopMenuRaycastUtilComponent = GameObject.Find("Environment/Shop/Menu").GetComponent<UIRaycasterUtil>();
 
        playerObject = GameObject.Find("Player");
        playerStateComponent = playerObject.GetComponent<PlayerState>();

        spawnManagerObject = GameObject.Find("SpawnManager");
        spawnManagerComponent = spawnManagerObject.GetComponent<SpawnManager>();
    }

    public EventSystem eventSystemComponent;
    public UIRaycasterUtil shopMenuRaycastUtilComponent;

    public GameObject playerObject;
    public PlayerState playerStateComponent;

    public GameObject spawnManagerObject;
    public SpawnManager spawnManagerComponent;

    public static EventSystem EventSystemComponent {
        get { return instance.eventSystemComponent; }
    }
    public static UIRaycasterUtil ShopMenuRaycastUtilComponent {
        get { return instance.shopMenuRaycastUtilComponent; }
    }

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
}

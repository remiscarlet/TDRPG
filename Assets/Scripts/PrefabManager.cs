using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Similar to <c>ReferenceManager</c> but for prefabs.
///
/// This class is used via ReferenceManager.
/// Ie, <c>ReferenceManager.Prefabs.DamageText</c>
/// NOT, <c>PrefabManager.DamageText</c>
/// </summary>
public class PrefabManager : MonoBehaviour {
    public GameObject damageText;
    public GameObject DamageText {
        get => damageText;
    }

    public GameObject enemy;
    public GameObject Enemy {
        get => enemy;
    }

    public GameObject fireball;
    public GameObject Fireball {
        get => fireball;
    }

    public GameObject magicMissile;
    public GameObject MagicMissile {
        get => magicMissile;
    }

    public GameObject tower;
    public GameObject Tower {
        get => tower;
    }
}

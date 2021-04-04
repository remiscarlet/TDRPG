using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrefabManager : MonoBehaviour {
    public GameObject damageText;
    public GameObject DamageText {
        get { return damageText; }
    }

    public GameObject enemy;
    public GameObject Enemy {
        get { return enemy; }
    }

    public GameObject fireball;
    public GameObject Fireball {
        get { return fireball; }
    }

    public GameObject magicMissile;
    public GameObject MagicMissile {
        get { return magicMissile; }
    }

    public GameObject tower;
    public GameObject Tower {
        get { return tower; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    static GameState instance;
    void Awake() {
        instance = this;
    }

    private bool shopOpen = false;
    public static bool ShopOpen {
        get { return instance.shopOpen; }
        set { instance.shopOpen = value; }
    }
}

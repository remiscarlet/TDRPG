using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    static GameState instance;
    void Awake() {
        instance = this;

        // This is kind of ugly.
        MapBoundaryMaxY = 1000.0f;
        MapBoundaryMinY = -15.0f;
        MapBoundaryMaxX = 1000.0f;
        MapBoundaryMinX = -1000.0f;
        MapBoundaryMaxZ = 1000.0f;
        MapBoundaryMinZ = -1000.0f;
    }

    private bool shopOpen = false;
    public static bool ShopOpen {
        get { return instance.shopOpen; }
        set { instance.shopOpen = value; }
    }

    // Maybe make a MapState class to account for different levels?
    private float mapBoundaryMinY;
    public static float MapBoundaryMinY {
        get { return instance.mapBoundaryMinY; }
        set { instance.mapBoundaryMinY = value; }
    }

    private float mapBoundaryMaxY;
    public static float MapBoundaryMaxY {
        get { return instance.mapBoundaryMaxY; }
        set { instance.mapBoundaryMaxY = value; }
    }
    
    private float mapBoundaryMinX;
    public static float MapBoundaryMinX {
        get { return instance.mapBoundaryMinX; }
        set { instance.mapBoundaryMinX = value; }
    }

    private float mapBoundaryMaxX;
    public static float MapBoundaryMaxX {
        get { return instance.mapBoundaryMaxX; }
        set { instance.mapBoundaryMaxX = value; }
    }
    
    private float mapBoundaryMinZ;
    public static float MapBoundaryMinZ {
        get { return instance.mapBoundaryMinZ; }
        set { instance.mapBoundaryMinZ = value; }
    }

    private float mapBoundaryMaxZ;
    public static float MapBoundaryMaxZ {
        get { return instance.mapBoundaryMaxZ; }
        set { instance.mapBoundaryMaxZ = value; }
    }
}

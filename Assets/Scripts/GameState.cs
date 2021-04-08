using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
    static GameState instance;
    void Awake() {
        instance = this;

        SetUpLayerCollisions();


        // This is kind of ugly.
        MapBoundaryMaxY = 1000.0f;
        MapBoundaryMinY = -15.0f;
        MapBoundaryMaxX = 1000.0f;
        MapBoundaryMinX = -1000.0f;
        MapBoundaryMaxZ = 1000.0f;
        MapBoundaryMinZ = -1000.0f;
    }

    private void SetUpLayerCollisions() {
        Physics.IgnoreLayerCollision(Layers.EnemyProjectiles, Layers.EnemyProjectiles);
        Physics.IgnoreLayerCollision(Layers.FriendlyProjectiles, Layers.FriendlyProjectiles);
        Physics.IgnoreLayerCollision(Layers.Enemy, Layers.EnemyProjectiles);
        Physics.IgnoreLayerCollision(Layers.Friendly, Layers.FriendlyProjectiles);
    }

    private bool shopOpen = false;
    public static bool ShopOpen {
        get => instance.shopOpen;
        set => instance.shopOpen = value;
    }

    // Maybe make a MapState class to account for different levels?
    private float mapBoundaryMinY;
    public static float MapBoundaryMinY {
        get => instance.mapBoundaryMinY;
        set => instance.mapBoundaryMinY = value;
    }

    private float mapBoundaryMaxY;
    public static float MapBoundaryMaxY {
        get => instance.mapBoundaryMaxY;
        set => instance.mapBoundaryMaxY = value;
    }

    private float mapBoundaryMinX;
    public static float MapBoundaryMinX {
        get => instance.mapBoundaryMinX;
        set => instance.mapBoundaryMinX = value;
    }

    private float mapBoundaryMaxX;
    public static float MapBoundaryMaxX {
        get => instance.mapBoundaryMaxX;
        set => instance.mapBoundaryMaxX = value;
    }

    private float mapBoundaryMinZ;
    public static float MapBoundaryMinZ {
        get => instance.mapBoundaryMinZ;
        set => instance.mapBoundaryMinZ = value;
    }

    private float mapBoundaryMaxZ;
    public static float MapBoundaryMaxZ {
        get => instance.mapBoundaryMaxZ;
        set => instance.mapBoundaryMaxZ = value;
    }
}

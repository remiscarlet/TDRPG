using UnityEngine;

public class DestroyOnOutOfBound : MonoBehaviour {
    void Update() {
        if (transform.position.y < GameState.MapBoundaryMinY ||
            transform.position.y > GameState.MapBoundaryMaxY ||
            transform.position.x < GameState.MapBoundaryMinX ||
            transform.position.x > GameState.MapBoundaryMaxX ||
            transform.position.z < GameState.MapBoundaryMinZ ||
            transform.position.z > GameState.MapBoundaryMaxZ) {
            Destroy(gameObject);
        }
    }
}